using System;
using System.Collections.Generic;
using System.Text;
using TMPro;

namespace Stacklands_NewLanguageLoader
{
	internal class FontLoadInfo
	{
		public FontLoadInfo(LanguageDefinition language)
		{
			Language = language;
			FontAsset = null;
		}

		/// <summary>
		/// The loaded font asset.
		/// </summary>
		public TMP_FontAsset FontAsset { get; set; } = null;

        public LanguageDefinition Language { get; set; }
    }
}
