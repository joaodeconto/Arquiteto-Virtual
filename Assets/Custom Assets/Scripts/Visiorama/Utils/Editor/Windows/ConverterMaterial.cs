using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ConverterMaterial : EditorWindow {
	static ConverterMaterial instance;
	
	private int optionFrom, optionTo;
	private string[] plataform;
	private List<Object> materials;
	private int PC_ShaderChoose, Mobile_ShaderChoose;
	
	[MenuItem ("UnityFramework/Converter Utils/Material")]
	static void Init () {
		instance = (ConverterMaterial)EditorWindow.GetWindow(typeof(ConverterMaterial), false, "Converter Material");
		instance.minSize = new Vector2(375, 110);
		instance.maxSize = new Vector2(400, 110);
	}
	
	void Start () {
	}
	
	void Awake () {
		plataform = new string[2]{ "PC", "Mobile" };
		Refresh ();
	}
	
	void OnGUI () {
		GUILayout.Space(20);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUILayout.Label("From: " + plataform[optionFrom]);
		GUILayout.Space(10);
		GUILayout.Label("To: " + plataform[optionTo]);
		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		GUILayout.Space(15);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUILayout.Label("Number of MATERIALS: " + materials.Count);
		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		GUILayout.Space(15);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Refresh")) {
			Refresh ();
			Verify ();
		}
		GUI.backgroundColor = Color.blue;
		if (GUILayout.Button("Converter")) {
			if (EditorUtility.DisplayDialog("Are you sure?",
				"Are you sure do you want convert?", "Yes", "No")) {
				//Function Converter
			}
		}
		GUILayout.EndHorizontal();
	}
	
	string[] GetShaders () {
		List<string> shadersName = new List<string>();
		Object[] allShaders = AssetResources.GetAllAssets(typeof(Shader)) as Object[];
		foreach (Shader thisShader in allShaders) {
			if (!thisShader.name.Contains("Hidden") &&
				!thisShader.name.Contains("EDITOR") &&
				!thisShader.name.Contains("__") &&
				thisShader.name.Length != 0) {
				shadersName.Add(thisShader.name);
			}
		}
		shadersName.Sort();
		return shadersName.ToArray();
	}
	
	void Verify () {
//		string path = AssetResources.Helper.__FILE__;
//		int lastIndex = path.LastIndexOf('/');
//		path = path.Remove(lastIndex) + "/";
//		string file = "teste.txt";
//		Debug.Log(path+file);
		
		string subpath = "\\ProjectSettings\\";
		string path = Directory.GetCurrentDirectory() + subpath;
		string file = "MaterialSettings.asset";
		if (!File.Exists (path+file)) {
			File.Create (path+file).Close();
			
			SaveConfiguration (path+file);
		} else {
			LoadConfiguration (path+file);
		}
	}
	
	void Refresh () {
		optionFrom = 	Application.platform != RuntimePlatform.Android && 
						Application.platform != RuntimePlatform.IPhonePlayer ? 0 : 1;
		optionTo = 		Application.platform != RuntimePlatform.Android && 
						Application.platform != RuntimePlatform.IPhonePlayer ? 1 : 0;
		materials = new List<Object>(AssetResources.GetAllAssets(typeof(Material)) as Object[]);
	}
	
	void SaveConfiguration (string path) {
		string[] shadersNames = GetShaders ();
		
		List<string> materialsLog = new List<string>();
		int k = 0;
		foreach (Material material in materials) {
			EditorUtility.DisplayCancelableProgressBar(	"Save Materials Configuration",
														"Material: " + material.name,
														k/materials.Count);
			if (PC_ShaderChoose == -1 || 
				Mobile_ShaderChoose == -1) {
				int i = 0;
				foreach (string sn in shadersNames) {
					if (PC_ShaderChoose == -1) {
						if (sn.Equals(material.shader.name)) {
								PC_ShaderChoose = i;
						}
					}
					if (Mobile_ShaderChoose == -1) {
						if (sn.Equals("Mobile/"+material.shader.name)) {
							Mobile_ShaderChoose = i;
						}
					}
					i++;
				}
				k++;
			}
			if (Mobile_ShaderChoose == -1)
				Mobile_ShaderChoose = PC_ShaderChoose;
			
			materialsLog.Add(material.GetInstanceID()+"-PcShader="+PC_ShaderChoose+"-MobileShader="+Mobile_ShaderChoose);
			
		}
		string[] values = materialsLog.ToArray();
		
//		System.Text.StringBuilder strbuilder = new System.Text.StringBuilder();
//		
//		foreach(string line in values)
//		{
//			strbuilder.Append (line);
//		}
//		
//		System.IO.File.WriteAllText("asd.asd", strbuilder.ToString());
		
		EditorUtility.ClearProgressBar();
		
		AssetResources.SaveTextFile(path, values);
	}
	
	void LoadConfiguration (string path) {
		string[] shaderNames = GetShaders ();
		List<string> configurations = AssetResources.LoadTextFile (path);
		
		int k = 0;
		foreach (Material material in materials) {
			EditorUtility.DisplayCancelableProgressBar(	"Save Materials Configuration",
														"Material: " + material.name,
														k/materials.Count);
			foreach (string config in configurations) {
				Debug.Log(material.GetInstanceID() + " contains: " + configurations.Contains(config.Split('-')[0]));
			}
			k++;
		}
		
		EditorUtility.ClearProgressBar();
	}
	
	void ConverterChange () {
		string[] shadersNames = GetShaders ();
		
		foreach (Material material in materials) {
			PC_ShaderChoose = EditorPrefs.GetInt(material.GetInstanceID()+"-PcShaderChoose", -1);
			Mobile_ShaderChoose = EditorPrefs.GetInt(material.GetInstanceID()+"-MobileShaderChoose", -1);
			if (PC_ShaderChoose == -1 || 
				Mobile_ShaderChoose == -1) {
				int i = 0;
				foreach (string sn in shadersNames) {
					if (PC_ShaderChoose == -1) {
						if (sn.Equals(material.shader.name)) {
								PC_ShaderChoose = i;
						}
					}
					if (Mobile_ShaderChoose == -1) {
						if (sn.Equals("Mobile/"+material.shader.name)) {
							Mobile_ShaderChoose = i;
						}
					}
					i++;
				}
			}
			if (Mobile_ShaderChoose == -1)
				Mobile_ShaderChoose = PC_ShaderChoose;
			
			if (optionTo == 0) {
				material.shader = Shader.Find(shadersNames[PC_ShaderChoose]);
			} else {
				material.shader = Shader.Find(shadersNames[Mobile_ShaderChoose]);
			}
		}
	}
}