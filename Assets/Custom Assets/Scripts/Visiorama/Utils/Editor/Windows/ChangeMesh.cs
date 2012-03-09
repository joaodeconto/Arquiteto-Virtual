//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class ChangeMesh : EditorWindow {
	static ChangeMesh instance;
	
	private Transform meshObjectContains;
	private string nameObject;
	private Mesh newMesh;
	private bool getChildren;
	private Vector2 scrollpos;
	private List<string> ignores = new List<string>();
	
	// Add menu named
	[MenuItem ("BlackBugio/Change Utils/Change Mesh")]
	static void Init () {
		instance = (ChangeMesh)EditorWindow.GetWindow(typeof(ChangeMesh), false, "Change Mesh");
		instance.ShowUtility();
		instance.Repaint();
		instance.nameObject = "";
		instance.scrollpos = new Vector2(0, 0);
	}
	
    public void OnGUI  () {
		scrollpos = GUILayout.BeginScrollView(scrollpos);
		GUILayout.Label("Development: BlackBugio ®");
		GUILayout.Space(10f);
		
		GUILayout.Label("Actual Mesh Transform:");
		meshObjectContains = EditorGUILayout.ObjectField(meshObjectContains, typeof(Transform)) as Transform;
		GUILayout.Space(5f);
		
		GUILayout.Label("Name of Object:");
		nameObject = EditorGUILayout.TextField(nameObject);
		GUILayout.Space(5f);
		
		GUILayout.Label("New Mesh:");
		newMesh = EditorGUILayout.ObjectField(newMesh, typeof(Mesh)) as Mesh;
		GUILayout.Space(5f);
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Get Children:");
		getChildren = EditorGUILayout.Toggle(getChildren);
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		
		if (getChildren) {
			GUILayout.Label("Ignore the GameObject name not to get the material.");
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Ignore:");
			if (GUILayout.Button("Add (+)")) {
				ignores.Add("");
			}
			GUILayout.EndHorizontal ();
			if (ignores.Count != 0) {
				for (int i = 0; i != ignores.Count; i++) {
					GUILayout.BeginHorizontal ();
					ignores[i] = EditorGUILayout.TextField(ignores[i], GUILayout.MinWidth(250f));
					if (GUILayout.Button("Remove (-)")) {
						ignores.RemoveAt(i);
						AssetDatabase.Refresh();
						break;
					}
					GUILayout.EndHorizontal ();
				}
			}
			GUILayout.Space(15f);
		}
		else { GUILayout.Space(10f); }
		
		if (GUILayout.Button("Apply")) {
			if (meshObjectContains != null && nameObject != "" && newMesh != null) {
				NewMesh(meshObjectContains);
			}
			else {
				if (meshObjectContains == null) {
					Debug.LogError("Please, put a transform to change the mesh.\n(Actual Mesh Transform)");
				}
				if (nameObject == "") {
					Debug.LogError("Please, put the name of object to change the mesh.\n(Name of Object)");
				}
				if (newMesh == null) {
					Debug.LogError("Please, put a mesh to change the transform mesh.\n(New Mesh)");
				}
			}
		}
		GUILayout.Space(5f);
//		
//		if (GUILayout.Button("WindowSize")) {
//			Debug.Log(instance.position);
//		}
		
		GUILayout.EndScrollView();
		
		EditorGUIUtility.ExitGUI();
    }
	
	void NewMesh (Transform transformMeshs) {
		Regex regexName = new Regex(nameObject);
		Transform[] allChilds = transformMeshs.GetComponentsInChildren<Transform>();
		List<Regex> regexIgnores = new List<Regex>();
		if (ignores.Count != 0) {
			foreach(string ignore in ignores) {
				regexIgnores.Add(new Regex(ignore));
			}
		}
		
		float totalItems = allChilds.Length;
		float progress = 0;
		
		bool breaker = false;
		foreach (Transform tm in allChilds) {
			EditorUtility.DisplayProgressBar(
                "Add Materials",
                "Checking Object: "+tm.name,
                progress/totalItems);
			if (regexName.IsMatch(tm.name)) {
				if (getChildren) {
					if (tm.GetComponentsInChildren<MeshFilter>().Length	!= 0) {
						foreach(MeshFilter mf in tm.GetComponentsInChildren<MeshFilter>()) {
							if (regexIgnores.Count != 0) {
								foreach(Regex regexIgnore in regexIgnores) {
									if (regexIgnore.IsMatch(mf.gameObject.name)
									    && regexIgnore.ToString() != "") {
										breaker = true;
									}
								}
							}
							if (breaker) { breaker = false; continue;}
							mf.mesh = newMesh;
						}
					}
				}
				else {
					if (tm.GetComponent<MeshFilter>() != null) {
						tm.GetComponent<MeshFilter>().mesh = newMesh;
					}
				}
			}
			progress++;
		}
		
		AssetDatabase.Refresh();
	}
}
//#endif