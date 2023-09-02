using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace Stacklands_NewLanguageLoader
{
	public class LanguageDefinition
	{
		
		/// <summary>
		/// The column text to use in the localization.tsv
		/// This is also used by the Select Language option screen as the "English" display name for the language.
		/// </summary>
		public string ColumnLanguageName { get; set; }

		/// <summary>
		/// The Unity Language enum.  Used for the game's automatic language detection.
		/// If there is not a matching entry in the enum, use Unknown
		/// </summary>
		public SystemLanguage UnityLanguage { get; set; } = SystemLanguage.Unknown;

		/// <summary>
		/// The native version of the language name displayed in the game's language selection screen.
		/// </summary>
		/// <example>Tiếng Việt</example>
		public string NativeDisplayName { get; set; }


		/// <summary>
		/// The Unity  path to the font in the bundle.
		/// For example: 'assets/assetsbundleswanted/wingdng2 sdf.asset'
		/// </summary>
		public string FontUnityAssetPath { get; set; } = null;

		/// <summary>
		/// The path to the Unity asset bundle file in the mod's directory.
		/// Example: "font.bundle"
		/// </summary>
		public string FontBundleFile { get;set; } = null;


		public string ModDirectory { get; set; } = "";

		public TMP_FontAsset Font { get; private set; } = null;

		/// <summary>
		/// Loads the font if the language has a custom font.
		/// </summary>
		/// <returns>If a 'soft error' occurs, returns the error.  Otherwise null.</returns>
		public bool LoadFont(out string error)
		{

			if(TryLoadFontAsset(out string loadError, out TMP_FontAsset font))
			{
				error = string.Empty;
				Font = font;
				return true;
			}
			else
			{
				error = loadError;
				return false;
			}
		}

		/// <summary>
		/// Attempts to load a font asset if FontBundleFile is set.
		/// </summary>
		/// <param name="error">Filled with error will be an empty string if no error </param>
		/// <param name="font">Will contain the TMP_FontAsset if successfully loaded</param>W
		/// <returns>True if a font was successfully</returns>
		/// <exception cref="FontLoadException">Contains any Unity exceptions or unexpected errors.</exception>
		private bool TryLoadFontAsset(out string error, out TMP_FontAsset font)
		{
			error = string.Empty;
			font = null;

			if (string.IsNullOrWhiteSpace(FontBundleFile))
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(FontUnityAssetPath)) {

				error = "FontUnityAssetPath is not set.";
				return false;
			}

			string fontBundlePath = Path.Combine(ModDirectory, FontBundleFile);

			try
			{

				if (!File.Exists(fontBundlePath))
				{
					error = $"Unable to find asset bundle file '{fontBundlePath}'";
					return false;
				}

				AssetBundle bundle = AssetBundle.LoadFromFile(fontBundlePath);

				//Verify the asset path is valid, or an exception will occur from Unity on use.
				string[] assetNames = bundle.GetAllAssetNames();

				if (assetNames.Any(x => x == FontUnityAssetPath))
				{
					font = bundle.LoadAsset<TMP_FontAsset>(FontUnityAssetPath);
					return true;
				}
				else
				{
					StringBuilder sb = new StringBuilder();

					sb.Append(
$@"Unable to find asset bundle for font: {FontUnityAssetPath} in asset file '{fontBundlePath}.
Asset paths found in assembly:
");
					//Asset not found.
					foreach (string assetName in assetNames)
					{
						sb.AppendLine($"'{assetName}'");
					}

					error = sb.ToString();
					return false;
				}

			}
			catch (Exception ex)
			{
				throw new FontLoadException($"Error loading font asset.  File: '{fontBundlePath}' Asset Path: '{FontUnityAssetPath}'", ex);
			}
		}

		public bool IsFontDefined()
		{
			//Intentionally do not check the asset path so the load will throw an exception for invalid configuration.
			return !(string.IsNullOrWhiteSpace(FontBundleFile));
		}
	}
}
