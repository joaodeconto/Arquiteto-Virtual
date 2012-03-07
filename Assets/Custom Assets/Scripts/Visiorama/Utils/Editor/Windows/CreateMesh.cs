//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Reflection;

class CreateMesh : EditorWindow {
	static CreateMesh instance;
	
	private Transform meshObjectContains;
	private string path, namePath;
	private List<string> ignores = new List<string>();
	
	// Add menu named
	[MenuItem ("BlackBugio/Create Utils/Create Mesh")]
	static void Init () {
		instance = (CreateMesh)EditorWindow.GetWindow(typeof(CreateMesh), false, "Create Mesh");
		instance.ShowUtility();
		instance.Repaint();
		instance.ClearLog();
		instance.path = instance.namePath = "";
	}
	
    public void OnGUI  () {
		GUILayout.Label("Development: BlackBugio ®");
		GUILayout.Space(10f); 
		
		GUILayout.Label("Mesh Contains Transform:");
		meshObjectContains = EditorGUILayout.ObjectField(meshObjectContains, typeof(Transform)) as Transform;
		GUILayout.Space(5f);
		
		GUILayout.Label("Path Mesh Output:");
		if (GUILayout.Button(path, "textfield")) {
			path = EditorUtility.OpenFolderPanel(path, Application.dataPath, "");
			if (path.IndexOf("Assets") != -1) {
				path = path.Substring(path.IndexOf("Assets"));
			}
			else {
				path = "";
				Debug.LogError("Please, save your mesh in the folder 'Assets'.\n(Path Mesh Output)");
			}
		}
		GUILayout.Space(5f);
		
		GUILayout.Label("Name Mesh Output:");
		namePath = EditorGUILayout.TextField(namePath);
		GUILayout.Space(10f);
		
		GUILayout.Label("Ignore the GameObject name not to get the collider.");
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
		
		if (GUILayout.Button("Apply")) {
			if (path != "") {
				CombineMesh(meshObjectContains);
			}
			else {
				Debug.LogError("Please, choose a folder to save your mesh.\n(Path Mesh Output)");
			}
		}
		GUILayout.Space(5f);
		
		if (GUILayout.Button("Clear Log")) {
			ClearLog();
		}
		
		EditorGUIUtility.ExitGUI();
    }
	
	void ClearLog () {
	    Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
	    System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
	    MethodInfo method = type.GetMethod("Clear");
		method.Invoke (new Object (), null);
	}
	
	void CombineMesh (Transform transformMeshs) {
		List<Regex> regexIgnores = new List<Regex>();
		if (ignores.Count != 0) {
			foreach(string ignore in ignores) {
				regexIgnores.Add(new Regex(ignore));
			}
		}
		
		MeshFilter[] meshFilters = transformMeshs.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		
		float totalMeshs = meshFilters.Length;
		float progress = 0;
		
		bool breaker = false;
        for (int i = 0; i != totalMeshs; ++i) {
			EditorUtility.DisplayProgressBar(
                "Add Materials",
                "Checking Object: "+meshFilters[i].name,
                i/totalMeshs);
			if (regexIgnores.Count != 0) {
				foreach(Regex regexIgnore in regexIgnores) {
					if (regexIgnore.IsMatch(meshFilters[i].gameObject.name)
					    && regexIgnore.ToString() != "") {
						breaker = true;
					}
				}
			}
			if (breaker) { breaker = false; continue;}
		    combine[i].mesh = meshFilters[i].sharedMesh;
		    combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}
		
		Object prefab = EditorUtility.CreateEmptyPrefab(path+"/"+namePath+".prefab");
		GameObject createPrefab = new GameObject();
		createPrefab.AddComponent<MeshFilter>();
		//createPrefab.AddComponent<MeshRenderer>();
        createPrefab.GetComponent<MeshFilter>().mesh = new Mesh();
        createPrefab.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        createPrefab.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        createPrefab.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
		AssetDatabase.CreateAsset(createPrefab.GetComponent<MeshFilter>().mesh, path+"/"+namePath+".asset");
		AssetDatabase.SaveAssets();
		createPrefab.AddComponent<MeshCollider>();
		createPrefab.GetComponent<MeshCollider>().sharedMesh = createPrefab.GetComponent<MeshFilter>().mesh;
		EditorUtility.ReplacePrefab(createPrefab, prefab, ReplacePrefabOptions.ConnectToPrefab);
		AssetDatabase.Refresh();
		ClearLog();
		DestroyImmediate(createPrefab);
	}
}
//#endif