using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime;
using System.Text;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace Stacklands_NewLanguageLoader.Patches
{


    /// <summary>
    /// Sets the font for the UI based on the selected language.
    /// </summary>
    [HarmonyPatch(typeof(FontManager), nameof(FontManager.GetFont))]
    public static class GetFont_Patch
    {


        //public static TMP_FontAsset OriginalWorldFont = null;
        /// <summary>
        /// The font language and related font that is currently being returned.
        /// </summary>
        private static LanguageDefinition CurrentLanguage = null;

        /// <summary>
        /// Sets the font based on the language provided.
        /// </summary>
        /// <param name="language"></param>
        public static void SetLanguage(LanguageDefinition language)
        {

            Plugin.Log.Log($"FontManager.GetFont Setting Font language '{language?.NativeDisplayName}' {language?.ModDirectory}");

            if (language == null)
            {

				//Not a custom font.  Revert to the game's default font.
				CurrentLanguage = null;
                return;
            }

            if (CurrentLanguage == language)
            {

				//Same language.  If the previous attempt to load was a success or fail, the result is the same.
				//	matches both being null as well.
				return;
            }
            else
            {
                CurrentLanguage = language;
            }
        }

        public static void Prefix(ref bool __runOriginal, ref TMP_FontAsset __result, string languageOverride)
        {

            __runOriginal = false;

            //Todo:  Check for language override.
            if (String.IsNullOrEmpty(languageOverride) == false)
            {


				if (LanguageInfoLoader.LoadedLanguages.TryGetValue(languageOverride, out LanguageDefinition languageDef))
                {

					__result = languageDef.Font;

                    //Doesn't seem to require a material to be set.  I think it is
                    //  only cards that are impacted by the world material issue.
                    return;
                }
                else
                {
					//Fallback to system's logic.

					//See "NOTE - World font restore" below
					//SetWorldFont(OriginalWorldFont);

					__runOriginal = true;
                    return;
                }
            }
            else if (CurrentLanguage?.Font == null)
            {
                //NOTE - World font restore.  I would think the world would need to be reset, but it doesn't for some reason.
                //  if the original font is restored, the game's base fonts no longer work.
                //  World and the specific ones are both affected.
				//SetWorldFont(OriginalWorldFont);

                __runOriginal = true;
                return;
            }
            else
            {
                SetWorldFont(CurrentLanguage.Font);

                __result = CurrentLanguage.Font;
                __runOriginal = false;
            }
        }

        private static void SetWorldFont(TMP_FontAsset font)
        {
			FontManager.instance.WorldFontMaterial.CopyPropertiesFromMaterial(font.material);
			FontManager.instance.WorldFontAsset = font;

		}
	}
}
