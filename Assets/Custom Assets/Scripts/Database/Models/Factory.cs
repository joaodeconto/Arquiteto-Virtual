using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
//using System.IO;
using System;

using UnityEngine;

public class Factory : MonoBehaviour
{
	public static string XmlSerialize (object o)
	{
		using (var stringWriter = new System.IO.StringWriter())
		{
			XmlWriterSettings settings = new XmlWriterSettings ();

			settings.OmitXmlDeclaration = true;
			settings.Encoding = System.Text.Encoding.GetEncoding ("UTF-8");

			using (var writer = XmlWriter.Create(stringWriter, settings))
			{
				var xmlSerializer = new XmlSerializer (o.GetType ());
				xmlSerializer.Serialize (writer, o);
			}
			return stringWriter.ToString();
	    }
	}

	public static object XmlDeserialize (string data, Type type)
	{
		object o = null;
		using (var stringReader = new System.IO.StringReader(data))
		{
			XmlReaderSettings settings = new XmlReaderSettings ();

//			settings. = true;
//			settings.encod Encoding = System.Text.Encoding.GetEncoding ("UTF-8");

			using (var reader = XmlReader.Create(stringReader, settings)) {

				var xmlSerializer = new XmlSerializer (type);
				o = xmlSerializer.Deserialize (reader);
			}
			return o;
	    }
	}

	public void SaveWeb (string path, Client client)
	{
		System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
		StartCoroutine(SaveWeb(path, enc.GetBytes(XmlSerialize(client))));

//		System.IO.MemoryStream stream = new System.IO.MemoryStream();
//		XmlSerializer serializer = new XmlSerializer (typeof(Client));
//
//		serializer.Serialize (stream, client);
//		stream.Close ();
//
//		byte[] bytes   = stream.ToArray ();
		byte[] bytes   = enc.GetBytes(XmlSerialize(client));
////		string data = System.Text.Encoding.UTF8.GetString (bytes);
//
		Visiorama.Utils.FileUtils.WriteFile("testao.xml", bytes, true);
//		StartCoroutine(SaveWeb(path, bytes));
	}

	private IEnumerator SaveWeb ( string url, byte[] data )
	{
		WWWForm form = new WWWForm ();
		form.AddBinaryData ("data", data);
		WWW www = new WWW (url, form);
		yield return www;
		if (null != www.error)
		{
			Debug.LogError ("chegou ao fim com sucesso");
			Debug.Log(" response: " + www.text );
		}
		else
		{
			Debug.LogError ("chegou ao fim com " + www.error);
			Debug.LogError (" response: " + www.text);
			Debug.LogError (" response: " + www.url);
		}
	}

//	public void SaveFileSystem (string path, object data, Type type)
//	{
//		XmlSerializer serializer = new XmlSerializer (type.GetType ());
//		FileStream stream = new FileStream(path, FileMode.Create);
//		serializer.Serialize (stream, data);
//		stream.Close ();
//	}
//    
//	public void LoadWeb (string url, Type type, GameObject caller, string methodCalledWhenOver)
//	{
//		XmlSerializer serializer = new XmlSerializer (type.GetType ());
//		Stream stream = new FileStream(path, FileMode.Open);
//		return serializer.Deserialize (stream);
//	}
// 
//	//Loads the xml directly from the given string. Useful in combination with www.text.
//	public object LoadFromText (string text, Type type)
//	{
//		return new XmlSerializer (type.GetType ()).Deserialize (new StringReader (text));
//	}
}
