using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Stacklands_NewLanguageLoader
{
	internal class NewLanguageException : Exception
	{
		public NewLanguageException()
		{
		}

		public NewLanguageException(string message) : base(message)
		{
		}

		public NewLanguageException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NewLanguageException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
