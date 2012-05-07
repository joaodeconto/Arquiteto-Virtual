using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class ConverterMaterial : EditorWindow {
	static ConverterMaterial instance;
	
	private int optionFrom, optionTo;
	private string[] plataform, shadersNames;
	private List<Object> materials;
	private int[] PC_ShaderChoose, Mobile_ShaderChoose;
	private bool[] getMaterial;
	private Vector2 scrollPosition;
	private string searchField;
	
	// File access
	private string path, subpath, file;
	
	[MenuItem ("Edit/Project Settings/Converter Material - BlackBugio")]
	static void Init () {
		instance = (ConverterMaterial)EditorWindow.GetWindow(typeof(ConverterMaterial), false, "Converter Material");
		instance.minSize = new Vector2(475, 410);
		instance.maxSize = new Vector2(500, 410);
	}
	
	void Awake () {
		subpath = "\\ProjectSettings\\";
		path = Directory.GetCurrentDirectory() + subpath;
		file = "MaterialSettings.asset";
		scrollPosition = Vector2.zero;
		plataform = new string[2]{ "PC", "Mobile" };
		searchField = "";
		Refresh ();
	}
	
	void OnGUI () {
		GUILayout.Space(20);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUI.contentColor = Color.green;
		GUILayout.Label("To: " + plataform[optionTo]);
		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		GUI.contentColor = Color.white;
		GUILayout.Space(15);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUILayout.Label("Number of MATERIALS: " + materials.Count);
		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		GUILayout.Space(15);
		
		searchField = EditorGUILayout.TextField("Search Material:", searchField);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Checked All")) {
			if (EditorUtility.DisplayDialog("Are you sure?",
				"Are you sure do you checked all?", "Yes", "No")) {
				for (int j = 0; j != getMaterial.Length; ++j) getMaterial[j] = true;
			}
		}
		if (GUILayout.Button("Unchecked All")) {
			if (EditorUtility.DisplayDialog("Are you sure?",
				"Are you sure do you unchecked all?", "Yes", "No")) {
				for (int j = 0; j != getMaterial.Length; ++j) getMaterial[j] = false;
			}
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(38);
		GUILayout.Label("Material:", GUILayout.Width(150));
		GUILayout.Label("PC:", GUILayout.Width(125));
		GUILayout.Label("Mobile:", GUILayout.Width(125));
		GUILayout.EndHorizontal();
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		for (int i = 0; i != materials.Count; ++i) {
			if (Regex.IsMatch(materials[i].name, searchField, RegexOptions.IgnoreCase)) {
				GUILayout.BeginHorizontal();
				getMaterial[i] = GUILayout.Toggle(getMaterial[i], "", GUILayout.Width(30));
				GUI.enabled = getMaterial[i];
				GUILayout.Label(materials[i].name, GUILayout.Width(150));
		        PC_ShaderChoose[i] = EditorGUILayout.Popup("", PC_ShaderChoose[i], shadersNames, GUILayout.Width(125));
		        Mobile_ShaderChoose[i] = EditorGUILayout.Popup("", Mobile_ShaderChoose[i], shadersNames, GUILayout.Width(125));
				GUI.enabled = true;
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
		
		GUILayout.Space(15);
		
		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.red;
		if (GUILayout.Button("Save")) {
			if (EditorUtility.DisplayDialog("Are you sure?",
				"Are you sure do you want save?", "Yes", "No")) {
				SaveConfiguration(path+file);
			}
		}
		if (GUILayout.Button("Load")) {
			if (EditorUtility.DisplayDialog("Are you sure?",
				"Are you sure do you want load?", "Yes", "No")) {
				LoadConfiguration(path+file);
			}
		}
		GUILayout.EndHorizontal();
		GUI.backgroundColor = Color.grey;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Refresh")) {
			Refresh ();
		}
		GUI.backgroundColor = Color.blue;
		if (GUILayout.Button("Converter")) {
			if (EditorUtility.DisplayDialog("Are you sure?",
				"Are you sure do you want convert?", "Yes", "No")) {
				ConverterChange ();
			}
		}
		GUILayout.EndHorizontal();
	}
	
	string[] GetShaders () {
		List<string> shadersName = new List<string>();
		Object[] allShaders = AssetResources.GetAllAssets(typeof(Shader)) as Object[];
		for (int i = 0; i != allShaders.Length; ++i) {
			EditorUtility.DisplayCancelableProgressBar(	"Load Shaders",
														"Material: " + (int)((i/allShaders.Length)*100) + "%",
														(i/allShaders.Length)*100);
			foreach (Shader thisShader in allShaders) {
				if (!thisShader.name.Contains("Hidden") &&
					!thisShader.name.Contains("EDITOR") &&
					!thisShader.name.Contains("__") &&
					thisShader.name.Length != 0) {
					shadersName.Add(thisShader.name);
				}
			}
		}
		shadersName.Sort();
		return shadersName.ToArray();
	}
	
	void Verify () {
		if (!File.Exists (path+file)) {
			File.Create (path+file).Close();
			
			SaveConfiguration (path+file);
		} else {
			LoadConfiguration (path+file);
		}
	}
	
	void Refresh () {
		optionFrom = 	EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android || 
						EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone ? 0 : 1;
		optionTo = 		EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android || 
						EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone ? 1 : 0;
		materials = new List<Object>(AssetResources.GetAllAssets(typeof(Material)) as Object[]);
		shadersNames = GetShaders ();
		
		PC_ShaderChoose = new int[materials.Count];
		Mobile_ShaderChoose = new int[materials.Count];
		getMaterial = new bool[materials.Count];
		
		int k = 0;
		foreach (Material material in materials) {
			int i = 0;
			foreach (string sn in shadersNames) {
				if (PC_ShaderChoose[k] == -1) {
					int intPathShader = material.shader.name.IndexOf("Mobile/");
					if (intPathShader != -1) {
						string pathShader = material.shader.name.Substring(0, intPathShader);
						//Debug.Log(pathShader + "  -  K" + k + " I" + i);
						if (sn.Equals(pathShader)) {
							PC_ShaderChoose[k] = i;
						}
					} else { 
						if (sn.Equals(material.shader.name)) {
							PC_ShaderChoose[k] = i;
						}
					}
				}
				if (Mobile_ShaderChoose[k] == -1) {
					int intPathShader = material.shader.name.IndexOf("Mobile/");
					if (intPathShader != -1) {
						if (sn.Equals(material.shader.name)) {
							PC_ShaderChoose[k] = i;
						}
					} else {
						if (sn.Equals("Mobile/"+material.shader.name)) {
							Mobile_ShaderChoose[k] = i;
						}
					}
				}
				i++;
			}
			if (PC_ShaderChoose[k] == -1)
				PC_ShaderChoose[k] = Mobile_ShaderChoose[k];
			if (Mobile_ShaderChoose[k] == -1)
				Mobile_ShaderChoose[k] = PC_ShaderChoose[k];
			
			getMaterial[k] = true;
			++k;
		}
		
		Verify ();
	}
	
	void SaveConfiguration (string path) {
		List<string> materialsLog = new List<string>();
		int k = 0;
		foreach (Material material in materials) {
			EditorUtility.DisplayCancelableProgressBar(	"Save Materials Configuration",
														"Material: " + material.name,
														k/materials.Count);
			
			materialsLog.Add(material.GetInstanceID()+"-PcShader="+PC_ShaderChoose[k]+"-MobileShader="+Mobile_ShaderChoose[k]+"-get="+getMaterial[k]);
			
			k++;
		}
		EditorUtility.ClearProgressBar();
		
		string[] values = materialsLog.ToArray();
		AssetResources.SaveTextFile(path, values);
	}
	
	void LoadConfiguration (string path) {
		List<string> configurations = AssetResources.LoadTextFile (path);
		for (int k = 0; k != materials.Count; ++k) {
			EditorUtility.DisplayCancelableProgressBar(	"Load Materials Configuration",
														"Material: " + (int)((k/materials.Count)*100) + "%",
														(k/materials.Count)*100);
			for (int i = 0; i != configurations.Count; ++i) {
				string idInstance = configurations[i].Split('-')[0];
				
				if (idInstance.Length != 0) {
					int id = System.Convert.ToInt32(idInstance);
					
					if (materials[k].GetInstanceID() == id) {
						string pc = configurations[i].Split('-')[1];
						string mb = configurations[i].Split('-')[2];
						string getMat = configurations[i].Split('-')[3];
						int pcId = System.Convert.ToInt32(pc.Split('=')[1]);
						int mbId = System.Convert.ToInt32(mb.Split('=')[1]);
						bool getMatBool = System.Convert.ToBoolean(getMat.Split('=')[1]);
						PC_ShaderChoose[k] = pcId;
						Mobile_ShaderChoose[k] = mbId;
						getMaterial[k] = getMatBool;
						break;
					}
				}
			}
		}
		
		EditorUtility.ClearProgressBar();
	}
	
	void ConverterChange () {
		int k = 0;
		foreach (Material material in materials) {
			EditorUtility.DisplayCancelableProgressBar(	"Converter Materials",
														"Material: " + (int)((k/materials.Count)*100) + "%",
														(k/materials.Count)*100);
			if (getMaterial[k]) {
				if (optionTo == 0) {
					material.shader = Shader.Find(shadersNames[PC_ShaderChoose[k]]);
				} else {
					material.shader = Shader.Find(shadersNames[Mobile_ShaderChoose[k]]);
				}
			}
			++k;
		}
		EditorUtility.ClearProgressBar();
	}
}