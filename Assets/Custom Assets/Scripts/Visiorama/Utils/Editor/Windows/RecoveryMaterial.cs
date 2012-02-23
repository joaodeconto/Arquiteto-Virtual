//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class RecoveryMaterial : EditorWindow {
	static RecoveryMaterial instance;
	
	GameObject objeto;
	Material material;
	string filter;
	int ignoresArray;
	List<string> ignores = new List<string>();
	
	// Add menu named
	[MenuItem ("BlackBugio/Recovery Utils/Recovery Material")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		instance = (RecoveryMaterial)EditorWindow.GetWindow(typeof(RecoveryMaterial), false, "Recovery Material");
		instance.ShowUtility();
		instance.Repaint();
	}
	
    public void OnGUI  () {
		GUILayout.Label("Development: BlackBugio ®");
		GUILayout.Space(10f);
		
		GUILayout.Label("Objeto principal para aplicar materiais.");
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Objeto:");
		objeto = EditorGUILayout.ObjectField(objeto, typeof(GameObject)) as GameObject;
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(5f);
		
		GUILayout.Label("Esse item faz com que o material seja colocado no item selecionado.");
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Material:");
		material = EditorGUILayout.ObjectField(material, typeof(Material)) as Material;
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(5f);
		
		GUILayout.Label("Filtrar com a palavra quais itens terão os materiais trocados.");
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Filtro:");
		filter = EditorGUILayout.TextField(filter);
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(5f);
		
		GUILayout.Label("Ignorar palavras para não colocar material.");
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Ignorar:");
		if (GUILayout.Button("+")) {
			ignores.Add("");
		}
		if (GUILayout.Button("-")) {
			if (ignores.Count != 0) {
				ignores.RemoveAt(ignores.Count - 1);
			}
		}
		GUILayout.EndHorizontal ();
		if (ignores.Count != 0) {
			for (int i = 0; i != ignores.Count; i++) {
				ignores[i] = EditorGUILayout.TextField(ignores[i]);
			}
		}
		GUILayout.Space(15f);
		
		if (GUILayout.Button("Apply")) {
			Regex regex = new Regex(filter);
			
			List<Regex> regexIgnore = new List<Regex>();
			if (ignores.Count != 0) {
				foreach(string ignore in ignores) {
					regexIgnore.Add(new Regex(ignore));
				}
			}
			
			bool breaker = false;
			foreach(Transform obj in objeto.GetComponentsInChildren<Transform>()) {
				if (regex.IsMatch(obj.name)) {
					if (regexIgnore.Count != 0) {
						foreach(Transform childObj in obj.GetComponentsInChildren<Transform>()) {
							foreach(Regex reg in regexIgnore) {
								if (reg.IsMatch(childObj.name)) {
									breaker = true;
								}
							}
		                }
					}
					if (breaker) { breaker = false; continue;}
					foreach (Renderer r in obj.GetComponentsInChildren<Renderer>()) {
						r.material = material;
					}
				}
			}
		}
    }
}
//#endif