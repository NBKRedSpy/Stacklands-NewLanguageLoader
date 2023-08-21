using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using HarmonyLib;
using UnityEngine;

namespace Stacklands_NewLanguageLoader
{
	public class Plugin : Mod
	{
		public static ModLogger Log;

		public static readonly string ModManifestId = "Stacklands_NewLanguageLoader";

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
			LogAllMissingManifests = Config.GetEntry<bool>("Debug: Log any missing manifests", false,
				new ConfigUI()
				{
					Tooltip =
@$"For any mods that have this mod as a dependency or optional dependency, 
log if that mod did not have a 
{LanguageInfoLoader.LanguageDataFileName}' file.
This is not an error as a required or optional dependency doesn't necessarily
mean it is a new language mod",
					RestartAfterChange = true,
				});

			DumpLanguage = Config.GetEntry<bool>("Dump Game's Localizations",false, new ConfigUI() {
Tooltip = @"Debug: On startup, save the game's default localization
data to a lang.tsv file in the mod's directory",
				RestartAfterChange = true,
			});

			DisableStackMessages = Config.GetEntry<bool>("Debug: Disable Stack Trace", false,
				new ConfigUI
				{
					Tooltip =
@"Debug: Make the log file more compact for mod info and warning logs by not including
the stack trace.  This will affect all mods",
					RestartAfterChange = true,
				});
		}

		public override void Ready()
		{

			//This might work to force the language to refresh.
			//SokLoc.instance.SetLanguage("Vietnamese");
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
