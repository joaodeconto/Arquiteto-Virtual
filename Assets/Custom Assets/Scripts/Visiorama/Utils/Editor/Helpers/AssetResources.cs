using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AssetResources
{
    public static string[] GetAllAssets()
    {
        string[] tmpAssets1 = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
        string[] tmpAssets2 = Array.FindAll(tmpAssets1, name => !name.EndsWith(".meta"));
        string[] allAssets;

        allAssets = Array.FindAll(tmpAssets2, name => !name.EndsWith(".unity"));

        for (int i = 0; i < allAssets.Length; i++)
        {
            allAssets[i] = allAssets[i].Substring(allAssets[i].IndexOf("/Assets") + 1);
            allAssets[i] = allAssets[i].Replace(@"\", "/");
        }

        return allAssets;
    }

    public static UnityEngine.Object[] GetAllAssets (Type type) {
		List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
		foreach(string asset in AssetResources.GetAllAssets()) {
			UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(asset, type);
			if (obj != null) {
				objects.Add(obj);
			}
		}
		
		return objects.ToArray();
	}
	
	public static string[] GetAllAssetsName (Type type) {
		List<string> objects = new List<string>();
		foreach(string asset in AssetResources.GetAllAssets()) {
			UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(asset, type);
			if (obj != null) {
				objects.Add(obj.name);
			}
		}
		
		return objects.ToArray();
	}
	
	public static List<string> LoadTextFile (string path) {
		StreamReader sr = new StreamReader(path);
        string line = sr.ReadLine();
		string[] getLines = line.Split('/');
		sr.Close();
		return new List<string>(getLines);
	}
	
	public static void SaveTextFile (string path, string[] values) {
		StreamWriter sw = new StreamWriter(path);
		
        foreach (string v in values)
        {
            sw.Write(v+"/");
        }
		
		sw.Close();
	}
	
	public class Helper {
		public static string ReportError (string Message)
		{
			// Get the frame one step up the call tree
			System.Diagnostics.StackFrame CallStack = new System.Diagnostics.StackFrame (1, true);
			
			// These will now show the file and line number of the ReportError
			string SourceFile = CallStack.GetFileName ();
			int SourceLine = CallStack.GetFileLineNumber ();
			
			return "Error: " + Message + "\nFile: " + SourceFile + "\nLine: " + SourceLine.ToString ();
		}
		
		public static int __LINE__ {
			get {
				System.Diagnostics.StackFrame CallStack = new System.Diagnostics.StackFrame (1, true);
				int line = new int ();
				line += CallStack.GetFileLineNumber ();
				return line;
			}
		}
		
		public static string __FILE__ {
			get {
				System.Diagnostics.StackFrame CallStack = new System.Diagnostics.StackFrame (1, true);
				string temp = CallStack.GetFileName ();
				string file = string.Copy (string.IsNullOrEmpty (temp) ? "" : temp);
				file = file.Replace('\\', '/');
				return string.IsNullOrEmpty (file) ? "" : file;
			}
		}
	}

}