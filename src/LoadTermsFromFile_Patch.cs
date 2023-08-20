using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using HarmonyLib;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;

namespace Stacklands_NewLanguageLoader
{
	[HarmonyPatch(typeof(SokLoc), nameof(SokLoc.LoadTermsFromFile))]
	public static class LoadTermsFromFile_Patch
	{


		/// <summary>
		/// The list of term keys that were filled with the text from the target language.
		/// For all other existing entries, they were filled with English
		/// </summary>
		private static HashSet<string> TargetLanguageKeys = new HashSet<string>();

		public static void Prefix(ref bool __runOriginal, SokLoc __instance, string path)
		{

			// Note: New language set
			// When a new language is specified, the game will automatically clear the terms collections.

			__runOriginal = false;

			try
			{

				LoadedLocSet currentLocSet = __instance.CurrentLocSet;

				Dictionary<string, SokTerm> termLookup = currentLocSet.TermLookup;
				List<SokTerm> allTerms = currentLocSet.AllTerms;


				int initialCount = allTerms.Count;

				//Plugin.Log.Log($"term count {allTerms.Count}");

				if (initialCount == 0)
				{
					//The language has been changed.  The game will reset the localizations before this call.
					Plugin.Log.Log($"Language: '{__instance.CurrentLanguage}'");
					TargetLanguageKeys = new HashSet<string>();
				}

				Plugin.Log.Log($"Loading localization file '{path}'");

				string[][] array = SokLoc.ParseTableFromTsv(File.ReadAllText(path));

				//----Get Language column
				int languageColumnIndex = array[0]?.ToList()?.IndexOf(__instance.CurrentLanguage) ?? -1;


				bool isFallback = false;    //True if the language has fallen back to English

				Plugin.Log.Log($"Language column'{languageColumnIndex}'");

				if (languageColumnIndex == -1)
				{
					if (__instance.CurrentLanguage != "English")
					{
						//The localization file doesn't have the target language.  Try falling back to English
						languageColumnIndex = array[0]?.ToList()?.IndexOf("English") ?? -1;

						Plugin.Log.Log($"No '{__instance.CurrentLanguage}' column.  Falling back to English");
					}
					else
					{
						Plugin.Log.Log($"Attempted fallback.  No English column to use as fallback.");
					}

					isFallback = true;
					

				}

				//----Process entries
				if (languageColumnIndex != -1 )
				{
					for (int i = 1; i < array.Length; i++)
					{
						//Plugin.Log.Log($"Processing line {i +1}");

						if (array[i].Length <= languageColumnIndex)
						{
							//Skip empty entries. 
							//The parser will also return an empty row for the last entry.
							continue;
						}

						//todo:  parser is returning empty lines
						string term = array[i][0];
						string fullText = array[i][languageColumnIndex];
						term = term.Trim().ToLower();
						if (string.IsNullOrEmpty(term))
						{
							continue;
						}
						SokTerm sokTerm = new SokTerm(currentLocSet, term, fullText);

						if(termLookup.TryAdd(term, sokTerm))
						{
							allTerms.Add(sokTerm);
							
							if(isFallback == false) TargetLanguageKeys.Add(term);
						}
						else
						{
							bool hasTargetLanguageTranslation = TargetLanguageKeys.Contains(term);

							if(isFallback == false && hasTargetLanguageTranslation == false)
							{
								//Overwrite.  The existing entry is an English fallback translation
								int index = allTerms.FindIndex(x => x.Id == term);

								if (index == -1)
								{
									throw new NewLanguageException($"Data mismatch.  AllTerms does not contain an entry for {term}");
								}

								allTerms[index] = sokTerm;
								termLookup[term] = sokTerm;

								TargetLanguageKeys.Add(term);
							}
						}
					}
				}


				//Insert any base game terms that are missing from the "Default Language", which is 
				//	currently set as English
				LoadedLocSet fallbackLanguage = SokLoc.FallbackSet;

				IEnumerable<string> missingKeys = fallbackLanguage.TermLookup.Keys
					.Where(x => termLookup.ContainsKey(x) == false)
					.ToList();

				//Check if any keys are missing from the game's defaults.
				foreach (string missingKey in missingKeys)
				{
					SokTerm fallbackTerm = fallbackLanguage.TermLookup[missingKey];
					SokTerm newTerm = new SokTerm(currentLocSet, fallbackTerm.Id, fallbackTerm.FullText);

					termLookup.Add(newTerm.Id, newTerm);
					allTerms.Add(newTerm);

					if(initialCount != 0)
					{
						Plugin.Log.LogWarning($"Languages:  Missing term '{missingKey}' using fallback language text '{fallbackTerm.FullText}'");
					}
				}
			}
			catch (Exception ex)
			{

				Plugin.Log.LogException(ex.ToString());
				throw;
			}
		}
	}
}
