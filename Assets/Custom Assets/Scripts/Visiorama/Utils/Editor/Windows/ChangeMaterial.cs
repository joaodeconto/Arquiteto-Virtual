using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Visiorama.Utils;

class ChangeMaterial : EditorWindow {
	static ChangeMaterial instance;
	
	private string[] style;
	private int selectStyle;
	private Transform materialObjectContains;
	private string nameObject;
	private bool getChildren;
	private Material newMaterial;
	private List<Object> newMaterials;
	private Vector2 scrollpos;
	private List<string> ignores = new List<string>();
	private Material materialTarget, materialApply;
	
	// Add menu named
	[MenuItem ("BlackBugio/Change Utils/Change Material")]
	static void Init () {
		instance = (ChangeMaterial)EditorWindow.GetWindow(typeof(ChangeMaterial), false, "Change Material");
		instance.ShowUtility();
		instance.Repaint();
		instance.nameObject = "";
		instance.style = new string[] {"Main Material", "In Order", "Add", "Change Target"};
		instance.selectStyle = 0;
		instance.newMaterials = new List<Object>();
		instance.scrollpos = new Vector2(0, 0);
	}
	
    public void OnGUI () {
		scrollpos = GUILayout.BeginScrollView(scrollpos);
		GUILayout.Label("Development: BlackBugio ®");
		GUILayout.Space(10f);
		
//		GUILayout.Label("Style:");
//		style = (Styles)EditorGUILayout.EnumPopup(style);
//		GUILayout.Space(5f);
		
		GUILayout.Label("Method:");
		selectStyle = GUILayout.Toolbar(selectStyle, style);
		GUILayout.Space(5f);
		
		if (selectStyle == 0) {
			GUILayout.Label("Actual Material Transform:");
			materialObjectContains = EditorGUILayout.ObjectField(materialObjectContains, typeof(Transform)) as Transform;
			GUILayout.Space(5f);
			
			GUILayout.Label("Name of Object:");
			nameObject = EditorGUILayout.TextField(nameObject);
			GUILayout.Space(5f);
			
			GUILayout.Label("New Material:");
			newMaterial = EditorGUILayout.ObjectField(newMaterial, typeof(Material)) as Material;
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
				if (materialObjectContains != null && nameObject != "" && newMaterial != null) {
					NewMaterial();
				}
				else {
					if (materialObjectContains == null) {
						Debug.LogError("Please, put a transform to change the material.\n(Actual Material Transform)");
					}
					if (nameObject == "") {
						Debug.LogError("Please, put the name of object to change the material.\n(Name of Object)");
					}
					if (newMaterial == null) {
						Debug.LogError("Please, put a material to change the transform material.\n(New Material)");
					}
				}
			}
		}
		else if (selectStyle == 1) {
			GUILayout.Label("Actual Material Transform:");
			materialObjectContains = EditorGUILayout.ObjectField(materialObjectContains, typeof(Transform)) as Transform;
			GUILayout.Space(5f);
			
			GUILayout.Label("Name of Object:");
			nameObject = EditorGUILayout.TextField(nameObject);
			GUILayout.Space(5f);
			
			GUILayout.Label("Organized material in the order related.");
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Material:");
			if (GUILayout.Button("Add (+)")) {
				newMaterials.Add(new Object());
			}
			GUILayout.Label(" ");
			if (newMaterials.Count != 0) {
				if (GUILayout.Button("Remove All")) {
					newMaterials.Clear();
				}
			}
			GUILayout.EndHorizontal ();
			if (newMaterials.Count != 0) {
				for (int i = 0; i != newMaterials.Count; i++) {
					GUILayout.BeginHorizontal ();
					newMaterials[i] = EditorGUILayout.ObjectField(newMaterials[i], typeof(Material), GUILayout.Width(200f)) as Material;
					if (GUILayout.Button("Remove (-)")) {
						newMaterials.RemoveAt(i);
						AssetDatabase.Refresh();
						break;
					}
					GUILayout.Label(" ");
					if (i != 0) {
						if (GUILayout.Button("«")) {
							ArrayUtils.Swap(newMaterials, i, i-1);
						}
					}
					else {
						GUILayout.Label("", GUILayout.Width(19f));
					}
					
					if (i != (newMaterials.Count - 1)) {
						if (GUILayout.Button("»")) {
							ArrayUtils.Swap(newMaterials, i, i+1);
						}
					}
					GUILayout.EndHorizontal ();
				}
			}
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
				if (materialObjectContains != null && nameObject != "" && newMaterials.Count > 0) {
					OrganizeMaterial();
				}
				else {
					if (materialObjectContains == null) {
						Debug.LogError("Please, put a transform to order the material.\n(Actual Material Transform)");
					}
					if (nameObject == "") {
						Debug.LogError("Please, put the name of object to order the material.\n(Name of Object)");
					}
					if (newMaterials.Count <= 0) {
						Debug.LogError("Please, put one or more materials to order of the transform material.\n(Material)");
					}
				}
			}
		}
		else if (selectStyle == 2) {
			GUILayout.Label("Actual Material Transform:");
			materialObjectContains = EditorGUILayout.ObjectField(materialObjectContains, typeof(Transform)) as Transform;
			GUILayout.Space(5f);
			
			GUILayout.Label("Name of Object:");
			nameObject = EditorGUILayout.TextField(nameObject);
			GUILayout.Space(5f);
			
			GUILayout.Label("Add material in the order related.");
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Material:");
			if (GUILayout.Button("Add (+)")) {
				newMaterials.Add(new Object());
			}
			GUILayout.Label(" ");
			if (newMaterials.Count != 0) {
				if (GUILayout.Button("Remove All")) {
					newMaterials.Clear();
				}
			}
			GUILayout.EndHorizontal ();
			if (newMaterials.Count != 0) {
				for (int i = 0; i != newMaterials.Count; i++) {
					GUILayout.BeginHorizontal ();
					newMaterials[i] = EditorGUILayout.ObjectField(newMaterials[i], typeof(Material), GUILayout.Width(200f)) as Material;
					if (GUILayout.Button("Remove (-)")) {
						newMaterials.RemoveAt(i);
						AssetDatabase.Refresh();
						break;
					}
					GUILayout.Label(" ");
					if (i != 0) {
						if (GUILayout.Button("«")) {
							ArrayUtils.Swap(newMaterials, i, i-1);
						}
					}
					else {
						GUILayout.Label("", GUILayout.Width(19f));
					}
					
					if (i != (newMaterials.Count - 1)) {
						if (GUILayout.Button("»")) {
							ArrayUtils.Swap(newMaterials, i, i+1);
						}
					}
					GUILayout.EndHorizontal ();
				}
			}
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
				if (materialObjectContains != null && nameObject != "" && newMaterials.Count > 0) {
					AddMaterials ();
				}
				else {
					if (materialObjectContains == null) {
						Debug.LogError("Please, put a transform to change the material.\n(Actual Material Transform)");
					}
					if (nameObject == "") {
						Debug.LogError("Please, put the name of object to change the material.\n(Name of Object)");
					}
					if (newMaterials.Count <= 0) {
						Debug.LogError("Please, put one or more materials to order of the transform material.\n(Material)");
					}
				}
			}
		}
		else if (selectStyle == 3) {
			GUILayout.Label("Actual Material Transform:");
			materialObjectContains = EditorGUILayout.ObjectField(materialObjectContains, typeof(Transform)) as Transform;
			GUILayout.Space(5f);
			
			GUILayout.Label("Material Target:");
			materialTarget = EditorGUILayout.ObjectField(materialTarget, typeof(Material)) as Material;
			GUILayout.Space(5f);
			
			GUILayout.Label("Material Apply:");
			materialApply = EditorGUILayout.ObjectField(materialApply, typeof(Material)) as Material;
			GUILayout.Space(15f);
			
			if (GUILayout.Button("Apply")) {
				if (materialObjectContains != null && materialTarget != null && materialApply != null) {
					ApplyMaterial();
				} else {
					if (materialObjectContains == null) {
						Debug.LogError("Please, put a transform to change the material.\n(Actual Material Transform)");
					}
					if (nameObject == "") {
						Debug.LogError("Please, put a Material Target.");
					}
					if (newMaterials.Count <= 0) {
						Debug.LogError("Please, put a Material Apply.");
					}
				}
			}
		}
		
		GUILayout.EndScrollView();
		
		EditorGUIUtility.ExitGUI();
    }
	
	void AddMaterials () {
		Regex regexName = new Regex(nameObject);
		Transform[] allChilds = materialObjectContains.GetComponentsInChildren<Transform>();
		bool warning = false;
		List<int> iWarning = new List<int>();
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
					if (tm.GetComponentsInChildren<Renderer>().Length != 0) {
						foreach(Renderer rd in tm.GetComponentsInChildren<Renderer>()) {
							if (regexIgnores.Count != 0) {
								foreach(Regex regexIgnore in regexIgnores) {
									if (regexIgnore.IsMatch(rd.gameObject.name)
									    && regexIgnore.ToString() != "") {
										breaker = true;
									}
								}
							}
							if (breaker) { breaker = false; continue;}
							Material[] actualMaterials = rd.materials;
							Material[] addMaterials = new Material[newMaterials.Count + actualMaterials.Length];
							for (int i = 0; i != actualMaterials.Length; ++i) {
								addMaterials[i] = actualMaterials[i];
							}
							for (int i = actualMaterials.Length; i != (newMaterials.Count + actualMaterials.Length); ++i) {
								if (newMaterials[i-actualMaterials.Length] != null) {
									addMaterials[i] = (Material)newMaterials[i-actualMaterials.Length];
								}
								else {
									warning = true;
									iWarning.Add(i-actualMaterials.Length);
								}
							}
							rd.materials = addMaterials;
						}
					}
				}
				else {
					if (tm.GetComponent<Renderer>() != null) {
						Material[] actualMaterials = tm.renderer.materials;
						Material[] addMaterials = new Material[newMaterials.Count + actualMaterials.Length];
						for (int i = 0; i != actualMaterials.Length; ++i) {
							addMaterials[i] = actualMaterials[i];
						}
						for (int i = actualMaterials.Length; i != (newMaterials.Count + actualMaterials.Length); ++i) {
							if (newMaterials[i-actualMaterials.Length] != null) {
								addMaterials[i] = (Material)newMaterials[i-actualMaterials.Length];
							}
							else {
								warning = true;
								iWarning.Add(i-actualMaterials.Length);
							}
						}
						tm.renderer.materials = addMaterials;
					}
				}
			}
			progress++;
		}
		
		ClearLog ();
		
		if (warning) {
			foreach (int iw in iWarning) {
				Debug.LogError("The Material of index '" + iw + "' is null");
			}
		}
		
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}

    void NewMaterial () {
		Regex regexName = new Regex(nameObject);
		Transform[] allChilds = materialObjectContains.GetComponentsInChildren<Transform>();
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
                "New Material",
                "Checking Object: "+tm.name,
                progress/totalItems);
			if (regexName.IsMatch(tm.name)) {
				if (getChildren) {
					if (tm.GetComponentsInChildren<Renderer>().Length != 0) {
						foreach(Renderer rd in tm.GetComponentsInChildren<Renderer>()) {
							if (regexIgnores.Count != 0) {
								foreach(Regex regexIgnore in regexIgnores) {
									if (regexIgnore.IsMatch(rd.gameObject.name)
									    && regexIgnore.ToString() != "") {
										breaker = true;
									}
								}
							}
							if (breaker) { breaker = false; continue;}
							rd.material = newMaterial;
						}
					}
				}
				else {
					if (tm.GetComponent<Renderer>() != null) {
						tm.renderer.material = newMaterial;
					}
				}
			}
			progress++;
		}
		
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}
	
	void OrganizeMaterial () {
		
		Regex regexName = new Regex(nameObject);
		Transform[] allChilds = materialObjectContains.GetComponentsInChildren<Transform>();
		bool warning = false;
		List<int> iWarning = new List<int>();
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
	                "Organizing Materials",
	                "Checking Object: "+tm.name,
	                progress/totalItems);
			Debug.Log(regexName.IsMatch(tm.name));
			if (regexName.IsMatch(tm.name)) {
				if (getChildren) {
					if (tm.GetComponentsInChildren<Renderer>().Length != 0) {
						foreach(Renderer rd in tm.GetComponentsInChildren<Renderer>()) {
							if (regexIgnores.Count != 0) {
								foreach(Regex regexIgnore in regexIgnores) {
									if (regexIgnore.IsMatch(rd.gameObject.name)
									    && regexIgnore.ToString() != "") {
										breaker = true;
									}
								}
							}
							if (breaker) { breaker = false; continue;}
							Material[] actualMaterials = rd.materials;
							actualMaterials = new Material[newMaterials.Count];
							for (int i = 0; i != newMaterials.Count; ++i) {
								if (newMaterials[i] != null) {
									actualMaterials[i] = (Material)newMaterials[i];
								}
								else {
									warning = true;
									iWarning.Add(i);
								}
							}
							rd.materials = actualMaterials;
						}
					}
				}
				else {
					if (tm.GetComponent<Renderer>() != null) {
						Material[] actualMaterials = tm.renderer.materials;
						actualMaterials = new Material[newMaterials.Count];
						for (int i = 0; i != newMaterials.Count; ++i) {
							if (newMaterials[i] != null) {
								actualMaterials[i] = (Material)newMaterials[i];
							}
							else {
								warning = true;
								iWarning.Add(i);
							}
						}
						tm.renderer.materials = actualMaterials;
					}
				}
			}
			progress++;
		}
		
		ClearLog ();
		
		if (warning) {
			foreach (int iw in iWarning) {
				Debug.LogError("The Material of index '" + iw + "' is null");
			}
		}
		
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}
	
	void ApplyMaterial () {
		Renderer[] allRenderers = materialObjectContains.GetComponentsInChildren<Renderer>();
		
		float totalItems = allRenderers.Length;
		float progress = 0;
		
		foreach (Renderer rd in allRenderers) {
			EditorUtility.DisplayProgressBar(
	                "Organizing Materials",
	                "Materal Object: "+rd.name,
	                progress/totalItems);
			Material[] actualMaterials = rd.materials;
			Material[] applyMaterials = new Material[actualMaterials.Length];
			for (int i = 0; i != rd.materials.Length; ++i) {
				applyMaterials[i] = actualMaterials[i];
				Regex regexName = new Regex(materialTarget.name);
				if (regexName.IsMatch(rd.materials[i].name)) {
					applyMaterials[i] = materialApply;
				}
			}
			rd.materials = applyMaterials;
			progress++;
		}
		
		ClearLog ();
		
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}
	
	void ClearLog () {
	    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
	    System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
	    System.Reflection.MethodInfo method = type.GetMethod("Clear");
		method.Invoke (new Object (), null);
	}
}