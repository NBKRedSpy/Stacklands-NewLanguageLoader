using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Xml.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stacklands_NewLanguageLoader
{
	public class Plugin : Mod
	{
		public static ModLogger Log;

		public static readonly string ModManifestId = "Stacklands_NewLanguageLoader";

		public static string ModsPath;


		/// <summary>
		/// Adds a debug entry for any manifests that are missing.
		/// </summary>
		ConfigEntry<bool> LogAllMissingManifests;

		/// <summary>
		/// If true, will save the game's languages to a file.
		/// </summary>
		ConfigEntry<bool> DumpLanguage;

		/// <summary>
		/// For any log warning and info writes, does not write out the 
		/// stack trace.  
		/// </summary>
		ConfigEntry<bool> DisableStackMessages;

		public void Awake()
		{

			Log = Logger;
			ModsPath = this.Path;

			InitConfigEntries();

			if (DumpLanguage.Value)
			{
				ExtractLanguageData();
			}

			if(DisableStackMessages.Value)
			{
				Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
				Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
			}

			//Todo:  Font diagnostics
			LanguageInfoLoader loader = new LanguageInfoLoader();

			loader.LoadModLanguages(LogAllMissingManifests.Value);

			Logger.Log($"Current SokLoc Language : '{SokLoc.instance.CurrentLanguage}' ");
			Harmony.PatchAll();

		}

		private void InitConfigEntries()
		{
			LogAllMissingManifests = GetEntry(nameof(LogAllMissingManifests), false, false);
			DumpLanguage = GetEntry(nameof(DumpLanguage), false, true);
			DisableStackMessages = GetEntry(nameof(DisableStackMessages), false, false);
		}

		public override void Ready()
		{
			//Load the fonts in Ready().  
			//For some reason, fonts won't load in the Awake.  Maybe a context or game state issue?
			//There appears to be no issue loading the fonts this late.  Everything seems to work anyway.
			foreach (var language in LanguageInfoLoader.LoadedLanguages.Values)
			{
				try
				{
					//Loads the font if it is set.
					if(string.IsNullOrEmpty(language.FontUnityAssetPath) == false)
					{
						Logger.Log($"Loading custom font '{language.ColumnLanguageName}'");
					}
					
					if (language.LoadFont(out string error) == false)
					{
						Logger.Log($"Error loading font for language '{language.ColumnLanguageName}'. Error: '{error}'");
					}
					//Saving for debugging options.
					//else
					//{
					//	//Debug
					//	Log.Log($"Characters count: {language.Font.characterTable.Count}");

					//	foreach (var c in language.Font.characterTable)
					//	{
					//		Log.Log($"	'{Convert.ToChar(c.unicode)}' ({c.unicode.ToString("X4")})");
					//	}


					//}

				}
				catch (Exception ex)
				{
					Plugin.Log.LogError($"Failed to set language '{language?.ColumnLanguageName}'.  {ex.ToString()}");
				}
			}


		}


		/// <summary>
		/// Creates an Config Entry using localization entries.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configPropertyName"></param>
		/// <param name="defaultValue"></param>
		/// <param name="displayText"></param>
		/// <param name="toolTip"></param>
		/// <param name="restartAfterChange"></param>
		/// <returns></returns>
		public ConfigEntry<T> GetEntry<T>(string configPropertyName, T defaultValue, bool restartAfterChange = false)
		{
			return Config.GetEntry<T>(configPropertyName, defaultValue, new ConfigUI()
			{
				NameTerm = string.Join("_", ModManifestId, configPropertyName,"Name"),
				TooltipTerm = string.Join("_", ModManifestId, configPropertyName, "ToolTip"),
				RestartAfterChange = restartAfterChange,
			});

		}

		private void ExtractLanguageData()
		{
			string filePath = System.IO.Path.Combine(Path, "lang.tsv");

			StringBuilder sb = new StringBuilder();

			foreach (var lang in LocResources.Default.LocTable)
			{
				if (string.IsNullOrEmpty(lang[0]))
				{
					//Skip if there is no key defined. 
					//I'm not sure why there are empty items in the array.
					continue;
				}


				int i = 0;
				foreach (var item in lang)
				{
					if (i != 0) sb.Append('\t');

					i++;
					sb.Append(item.Replace("\n", "#"));     //The SokLoc loader translation.
				}

				sb.AppendLine();

			}

			File.WriteAllText(filePath, sb.ToString());
		}
		

	}
}
