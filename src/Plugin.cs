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

			LanguageInfoLoader loader = new LanguageInfoLoader();

			loader.LoadModLanguages(LogAllMissingManifests.Value);

			Plugin.Log.Log($"Current SokLoc Language : '{SokLoc.instance.CurrentLanguage}' ");
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
