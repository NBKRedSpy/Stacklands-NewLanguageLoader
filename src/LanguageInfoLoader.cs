using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using Newtonsoft.Json;
using UnityEngine;
using static UnityEngine.Scripting.GarbageCollector;

namespace Stacklands_NewLanguageLoader
{
	internal class LanguageInfoLoader
	{
		/// <summary>
		/// The name of the file which contains the LanguageDefinition info.
		/// </summary>
		public static readonly string LanguageDefinitionFileName = "add-language-info.json";

		public static readonly string LanguageDataFileName = "localization.tsv";

		public static readonly string StacklandsModManifestFileName = "manifest.json";

		public static Dictionary<string, LanguageDefinition> LoadedLanguages = 
			new Dictionary<string, LanguageDefinition>();

		internal void LoadModLanguages(bool logAllMissing)
		{


			//Must load the languages manually since the new language mods do not show up in the 
			//	LoadedMods until they are found, which will always be after this mod.

			List<(string ManifestPath, ModManifest Manifest)> languageMods = GetNewLanguageMods();


			Plugin.Log.Log($"Language counts: {languageMods.Count}");

			foreach ((string ManifestPath, ModManifest Manifest) languageMod in languageMods)
			{
				try
				{
					//---Definition

					LanguageDefinition language;


					FileInfo modManifestFile = new FileInfo(languageMod.ManifestPath);
					string defintionFileName = Path.Join(modManifestFile.DirectoryName,
						LanguageInfoLoader.LanguageDefinitionFileName);


					if(!File.Exists(defintionFileName))
					{
						if(logAllMissing)
						{
							Plugin.Log.LogError($"Unable to find language definition file '{defintionFileName}'");
						}

						//Do not throw error as a mod may mark this mod as option or required, but not actually be a 
						//	new language.  Could be just a mod that only has a new language defined for some reason.
						//Would avoid a user needing to put a dependency on every language mod that the mod has a 
						//	column for.
						break;

					}

					Plugin.Log.Log($"Loading language mod: {defintionFileName}");


					language = JsonConvert.DeserializeObject<LanguageDefinition>(File.ReadAllText(defintionFileName));
					language.ModDirectory = modManifestFile.DirectoryName;


					RegisterLanguage(language);
					Plugin.Log.Log($"Registered '{language.ColumnLanguageName}' '{language.NativeDisplayName}'");


					//todo:  duplicate mods (Steam and local) are not skipping.
					//TODO:  Verify that disabled language mods are not being affected.
					LoadedLanguages.Add(language.ColumnLanguageName, language);
				}
				catch (Exception ex)
				{
					throw new NewLanguageException($"Error loading language for mod '{languageMod.Manifest.Name}'", ex);
				}
			}
		}


		/// <summary>
		/// Returns a list of mods that are "new language" dependent mods, and enabled.
		/// </summary>
		/// <returns>The path to the manifest file and the manifest data for new language mods that are enabled.</returns>
		public List<(string ManifestPath, ModManifest Manifest)> GetNewLanguageMods()
		{

			List<string> modPaths = ModManager.GetModPaths();
			var modManifests = new List<(string, ModManifest)>();

			HashSet<string> loadedManifestIds = new HashSet<string>();	

			foreach (string modPath in modPaths)
			{
				try
				{

					string manifestFilePath = Path.Join(modPath, LanguageInfoLoader.StacklandsModManifestFileName);

					if (File.Exists(manifestFilePath))
					{
						ModManifest manifest = JsonConvert.DeserializeObject<ModManifest>(File.ReadAllText(manifestFilePath));

						if (SaveManager.instance.CurrentSave.DisabledMods.Contains(manifest.Id))
						{
							continue;
						}

						if(loadedManifestIds.Contains(manifest.Id))
						{
							Plugin.Log.Log($"Language Loading:  Mod with id '{manifest.Id}' has already been loaded.  Skipping.  Directory '{manifestFilePath}'");
							continue;
						}

						if (
							manifest.Dependencies.Contains(Plugin.ModManifestId,StringComparer.OrdinalIgnoreCase) ||
							manifest.OptionalDependencies.Contains(Plugin.ModManifestId, StringComparer.OrdinalIgnoreCase))
						{
							modManifests.Add((manifestFilePath, manifest));
							loadedManifestIds.Add(manifest.Id);
						}
					}
				}
				catch (Exception ex)
				{
					throw new NewLanguageException($"Error loading mod info for '{modPath}'", ex);
				}
			}

			return modManifests;

		}

		//Registers the language with the game
		private void RegisterLanguage(LanguageDefinition language)
		{
			var langs = SokLoc.Languages.ToList<SokLanguage>();
			langs.Insert(0,new SokLanguage { LanguageName = language.ColumnLanguageName, 
				UnitySystemLanguage = language.UnityLanguage});

			SokLoc.Languages = langs.ToArray();

			var langNamesField = typeof(SokLoc).GetField("localLanguageNames", BindingFlags.NonPublic | BindingFlags.Static);
			Dictionary<string, string> langNames = (Dictionary<string, string>)langNamesField.GetValue(null);
			langNames.Add(language.ColumnLanguageName, language.NativeDisplayName);
			langNamesField.SetValue(null, langNames);
		}

	}
}
