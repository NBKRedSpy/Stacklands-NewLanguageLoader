using System;
using System.Collections.Generic;
using System.Text;
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

	}
}
