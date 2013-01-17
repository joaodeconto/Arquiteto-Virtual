using UnityEngine;
using System;
using System.Collections;
using System.Text;

namespace Visiorama
{
	namespace Utils
	{
		public class StringUtils
		{
			public static string UTF8ByteArrayToString (byte[] characters)
			{
				UTF8Encoding encoding = new UTF8Encoding ();
				string constructedString = encoding.GetString (characters);
				return (constructedString);
			}

			public static byte[] StringToUTF8ByteArray (string _string)
			{
				UTF8Encoding encoding = new UTF8Encoding ();
				byte[] byteArray = encoding.GetBytes (_string);
				return byteArray;
			}
			
			public static string ConvertStringWithSpecialCharacters (string _string)
			{				
				Encoding enc = Encoding.GetEncoding("iso-8859-1");
	            byte[] utf8Bytes = enc.GetBytes(_string);
	            string newString = enc.GetString(utf8Bytes);
				return newString;
			}
		}
	}
}
