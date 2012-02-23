using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Visiorama.Utils;

class AddObjectTransformInChild : EditorWindow {
	static AddObjectTransformInChild instance;
	
	private Transform transformToAdd, reference;
	private string nameObject, nameChildrenObject;
	private List<Object> objects;
	private bool setPosition, getChildrenPosition;
	private Vector3 position;
	private Vector2 scrollpos;
	
	// Add menu named
	[MenuItem ("BlackBugio/Add Utils/Object Transform In Child")]
	static void Init () {
		instance = (AddObjectTransformInChild)EditorWindow.GetWindow(typeof(AddObjectTransformInChild), false, "Add Object Transform In Child");
		instance.ShowUtility();
		instance.Repaint();
		instance.nameObject = "";
		instance.nameChildrenObject = "";
		instance.setPosition = false;
		instance.objects = new List<Object>();
		instance.scrollpos = new Vector2(0, 0);
	}
	
    public void OnGUI () {
		EditorGUI.indentLevel = 0;
		scrollpos = GUILayout.BeginScrollView(scrollpos);
		GUILayout.Label("Development: BlackBugio ®");
		GUILayout.Space(10f);
		
		GUILayout.Label("Select transform which will be added:");
		transformToAdd = EditorGUILayout.ObjectField(transformToAdd, typeof(Transform)) as Transform;
		GUILayout.Space(5f);
		
		GUILayout.Label("Name of object to filter which will be added:");
		nameObject = EditorGUILayout.TextField(nameObject);
		GUILayout.Space(5f);
		
		GUILayout.Label("Objects add in transform.");
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Object:");
		if (GUILayout.Button("Add (+)")) {
			objects.Add(new Object());
		}
		GUILayout.Label(" ");
		if (objects.Count != 0) {
			if (GUILayout.Button("Remove All")) {
				objects.Clear();
			}
		}
		GUILayout.EndHorizontal ();
		if (objects.Count != 0) {
			for (int i = 0; i != objects.Count; i++) {
				GUILayout.BeginHorizontal ();
				objects[i] = EditorGUILayout.ObjectField(objects[i], typeof(GameObject), GUILayout.Width(200f)) as GameObject;
				if (GUILayout.Button("Remove (-)")) {
					objects.RemoveAt(i);
					AssetDatabase.Refresh();
					break;
				}
				GUILayout.Label(" ");
				if (i != 0) {
					if (GUILayout.Button("«")) {
						ArrayUtils.Swap(objects, i, i-1);
					}
				}
				else {
					GUILayout.Label("", GUILayout.Width(19f));
				}
				
				if (i != (objects.Count - 1)) {
					if (GUILayout.Button("»")) {
						ArrayUtils.Swap(objects, i, i+1);
					}
				}
				GUILayout.EndHorizontal ();
			}
		}
		GUILayout.Space(5f);
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Set Position:");
		setPosition = EditorGUILayout.Toggle(setPosition);
		GUILayout.EndHorizontal();
		
		if (setPosition) {
			EditorGUI.indentLevel = 2;
			GUILayout.BeginHorizontal();
			GUILayout.Label("     Get Children Position:");
			getChildrenPosition = EditorGUILayout.Toggle(getChildrenPosition);
			GUILayout.EndHorizontal();
			if (getChildrenPosition) {
				EditorGUI.indentLevel = 2;
				GUILayout.Label("     Name of children reference position:");
				nameChildrenObject = EditorGUILayout.TextField(nameChildrenObject);
			} else {
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Reference:");
				reference = EditorGUILayout.ObjectField(reference, typeof(Transform)) as Transform;
				GUILayout.EndHorizontal();
				GUILayout.Label("     Reference - Use to set Position of Transform", GUILayout.ExpandWidth(true));
				position = EditorGUILayout.Vector3Field("Postion:", position);
				if (reference != null) {
					position = reference.position;
					reference = null;
				}
			}
			EditorGUI.indentLevel = 0;
		}
		GUILayout.Space(15f);
		
		if (GUILayout.Button("Apply")) {
			if (transformToAdd != null && nameObject != "" && objects.Count > 0) {
				Add();
			}
			else {
				if (transformToAdd == null) {
					Debug.LogError("Please, put a transform to filter which will be added.\n(Transform)");
				}
				if (nameObject == "") {
					Debug.LogError("Please, put the name of object to filter which will be added.\n(Name of Object)");
				}
				if (objects.Count <= 0) {
					Debug.LogError("Please, put one or more objects to attach in Transform.");
				}
			}
		}
		
		GUILayout.EndScrollView();
		
		EditorGUIUtility.ExitGUI();
    }
		
	void Add () {
		Regex regexName = new Regex(nameObject);
		Transform[] allChilds = transformToAdd.GetComponentsInChildren<Transform>();
		bool warning = false;
		List<int> iWarning = new List<int>();
		
		foreach (Transform t in allChilds) {
			if (regexName.IsMatch(t.name)) {
				if (setPosition) {
					if (getChildrenPosition) {
						foreach (GameObject obj in objects) {
							GameObject go = Instantiate(obj) as GameObject;
							Regex regexNameChildren = new Regex(nameChildrenObject);
							Transform[] allChildsThisTransform = t.GetComponentsInChildren<Transform>();
							foreach (Transform child in allChildsThisTransform) {
								if (regexNameChildren.IsMatch(child.name)) {
									go.transform.position = child.position;
								}
							}
							go.transform.parent = t;
							go.transform.rotation = Quaternion.identity;
						}
					}
					else {
						foreach (GameObject obj in objects) {
							//GameObject go = new GameObject(obj.name, obj.GetComponents(typeof(Component)));
							GameObject go = Instantiate(obj) as GameObject;
							go.transform.position = position;
							go.transform.parent = t;
							go.transform.rotation = Quaternion.identity;
						}
					}
				}
				else {
					foreach (GameObject obj in objects) {
						//GameObject go = new GameObject(obj.name, obj.GetComponents(typeof(Component)));
						GameObject go = Instantiate(obj) as GameObject;
						go.transform.position = t.position;
						go.transform.parent = t;
						go.transform.rotation = Quaternion.identity;
					}
				}
			}
		}
		
		ClearLog ();
		
		if (warning) {
			foreach (int iw in iWarning) {
				Debug.LogError("The GameObject of index '" + iw + "' is null");
			}
		}
		
		AssetDatabase.Refresh();
	}
	
	
	
	void ClearLog () {
	    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
	    System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
	    System.Reflection.MethodInfo method = type.GetMethod("Clear");
		method.Invoke (new Object (), null);
	}
}