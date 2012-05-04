using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Material))]
public class MaterialConverter : Editor {
	
	private Material component;
	
	public int PC_ShaderChoose, Mobile_ShaderChoose;
	
	private List<string> shadersName;

    void OnEnable()
    {
		component = target as Material;
		GetShader();
    }
	
	void OnDisable()
    {
		EditorPrefs.SetInt(component.GetInstanceID()+"-PcShaderChoose", PC_ShaderChoose);
		EditorPrefs.SetInt(component.GetInstanceID()+"-MobileShaderChoose", Mobile_ShaderChoose);
    }
	
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MaterialConverterInspectorGUI();
    }
	
	public void MaterialConverterInspectorGUI()
    {
        PC_ShaderChoose = EditorGUILayout.Popup("PC: ", PC_ShaderChoose, shadersName.ToArray());
        Mobile_ShaderChoose = EditorGUILayout.Popup("Mobile: ", Mobile_ShaderChoose, shadersName.ToArray());
        EditorGUILayout.Space();
    }
	
	void GetShader () {
		PC_ShaderChoose = EditorPrefs.GetInt(component.GetInstanceID()+"-PcShaderChoose", -1);
		Mobile_ShaderChoose = EditorPrefs.GetInt(component.GetInstanceID()+"-MobileShaderChoose", -1);
		shadersName = new List<string>();
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
		if (PC_ShaderChoose == -1 || 
			Mobile_ShaderChoose == -1) {
			int i = 0;
			foreach (string sn in shadersName) {
				if (PC_ShaderChoose == -1) {
					if (sn.Equals(component.shader.name)) {
							PC_ShaderChoose = i;
					}
				}
				if (Mobile_ShaderChoose == -1) {
					if (sn.Equals("Mobile/"+component.shader.name)) {
						Mobile_ShaderChoose = i;
					}
				}
				i++;
			}
		}
		if (Mobile_ShaderChoose == -1)
			Mobile_ShaderChoose = PC_ShaderChoose;
	}
}