using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace Stacklands_NewLanguageLoader
{

	[HarmonyPatch(typeof(SelectLanguageScreen), "Start")]
	 public static class SelectLanguageClick_Patch
	{
		public static void Postfix(SelectLanguageScreen __instance)
		{

			var gameObject = __instance.ButtonsParent.gameObject;

			foreach (var button in gameObject.GetComponentsInChildren<CustomButton>())
			{
				button.Clicked += Button_Clicked;
			} 
		}

		private static void Button_Clicked()
		{

			//Losing fonts on change.  Not sure why...
			if(LanguageInfoLoader.LoadedLanguages.TryGetValue(SokLoc.instance.CurrentLanguage, out LanguageDefinition languageDefinition))
			{

				//Debug
				////Reboot the game if the language has a custom font.  Not sure why, but the game is losing the font on change.
				//if(languageDefinition.Font != null)
				//{
				//	WorldManager.RebootGame();
				//}
			}
					
			//Not sure why, but the UI doesn't update unless the language is loaded twice.
			SokLoc.instance.SetLanguage(SokLoc.instance.CurrentLanguage);
		}
	}
}
