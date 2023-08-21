using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

namespace Stacklands_NewLanguageLoader
{


	[HarmonyPatch(typeof(MainMenu), "Update")]
	public static class Example_Patch
	{

		private static bool HasRun = false;
		public static void Postfix()
		{

			if (HasRun)
			{
				return;
			}

			HasRun = true;

			AddLanguageButton();
		}

		private static void AddLanguageButton()
		{
			GameObject buttonsObject = GameObject.Find("Canvas/MenuScreen/Background/Buttons");

			if (buttonsObject == null)
			{
				return;
			}

			//-------------- Languages button
			CustomButton btn = UnityEngine.Object.Instantiate(PrefabManager.instance.ButtonPrefab, buttonsObject.transform);

			btn.transform.localScale = Vector3.one;
			btn.transform.localPosition = Vector3.zero;
			btn.transform.localRotation = Quaternion.identity;
			btn.transform.SetSiblingIndex(btn.transform.GetSiblingIndex() - 1); //Put above the quit button.

			btn.Clicked += () =>
			{
				GameCanvas.instance.SetScreen<SelectLanguageScreen>();
			};

			btn.TextMeshPro.text = "Set Language";

			//----------Image container
			GameObject imgContainer = new GameObject("ImageContainer");
			imgContainer.transform.SetParent(btn.transform);

			//imgContainer.transform.localScale = Vector3.one;
			imgContainer.transform.localScale = Vector3.one;
			imgContainer.transform.localPosition = Vector3.zero;
			imgContainer.transform.localRotation = Quaternion.identity;


			//---------- "Language" icon
			Texture2D tex = new Texture2D(2, 2);
			ImageConversion.LoadImage(tex, File.ReadAllBytes(System.IO.Path.Combine(Plugin.ModsPath, @"language-small.png")));

			Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one);

			Image image = imgContainer.AddComponent<Image>();

			image.sprite = sprite;
			image.preserveAspect = true;

		}
	}
}
