using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Stacklands_NewLanguageLoader
{
	internal class FontLoadException : Exception
	{
		public FontLoadException()
		{
		}

		public FontLoadException(string message) : base(message)
		{
		}

		public FontLoadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected FontLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
