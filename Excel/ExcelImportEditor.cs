using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Odbc;
using System.Reflection;

public class ExcelImportEditor : EditorWindow {
	
	static ExcelImportEditor instance;
	
	private GameObject gameObject;
	private string path, lastPath, file, sheet, lastSheet;
	private MonoScript monoScript, lastMonoScript;
	private Vector2 scrollPosition;
	private List<string> columns, fields;
	private int[] fieldsMonoScript;
	private bool[] boolFieldsMonoScript;
	private int referenceField, referenceColumn;
	private bool useReference, hideApply, sheetNotExist;
	
	[MenuItem("BlackBugio/Excel/Import")]
	static void Init () {
		instance = (ExcelImportEditor)EditorWindow.GetWindow(typeof(ExcelImportEditor), false, "Excel Import");
		instance.gameObject = null;
		instance.path = instance.file = instance.sheet = "";
		instance.monoScript = instance.lastMonoScript = null;
		instance.scrollPosition = Vector2.zero;
		instance.columns = new List<string>();
		instance.minSize = new Vector2(350, 500);
		instance.maxSize = new Vector2(450, 550);
	}
	
	void OnGUI () {
		GUILayout.Label("Development: BlackBugio ®");
		
		GUILayout.Space(10f);
		
		GUILayout.Label ("Game Object Reference:");
		gameObject = EditorGUILayout.ObjectField (gameObject, typeof(GameObject), true) as GameObject;
		
		GUILayout.Space(5f);
		
		GUILayout.Label("Path File:");
		
		if (GUILayout.Button(file, "textfield")) {
			file = EditorUtility.OpenFilePanel(file, Application.dataPath, "xls");
			path = file;
			if (path.IndexOf("Assets") != -1) {
				if (file.IndexOf(".xls") != -1) {
					string[] splitFile = file.Split('/');
					file = splitFile[splitFile.Length-1];
				}
				else {
					path = file = "";
					Debug.LogError("The File the extension is not \".xls\".\n(Path File)");
					return;
				}
			}
			else {
				path = file = "";
				Debug.LogError("Please, open folder in \"Assets\".\n(Path File)");
				return;
			}
		}
		
		GUILayout.Space(5f);
		
		GUILayout.Label("Sheet:");
		sheet = GUILayout.TextField(sheet);
		
		if (gameObject == null || file.Equals("") || sheet.Equals("")) {
			GUI.enabled = false;
			hideApply = true;
		} else hideApply = false;
		
		GUILayout.Label ("Script Reference:");
		monoScript = EditorGUILayout.ObjectField (monoScript, typeof(MonoScript), false) as MonoScript;
		
		GUILayout.Space(5f);
		
		if (monoScript != null) {
//			if (monoScript.GetType() == this.GetType()) {
//				monoScript = null;
//				return;
//			}
			if (monoScript != lastMonoScript ||
				path != lastPath ||
				sheet != lastSheet) {
				GetColums (path, sheet);
				lastMonoScript = monoScript;
				lastPath = path;
				lastSheet = sheet;
				fieldsMonoScript = new int[monoScript.GetClass().GetFields().Length];
				boolFieldsMonoScript = new bool[fieldsMonoScript.Length];
				for (int k = 0; k != boolFieldsMonoScript.Length; k++) boolFieldsMonoScript[k] = true;
				fields = new List<string>();
				foreach (FieldInfo f in monoScript.GetClass().GetFields())
				{
					if (f.FieldType == string.Empty.GetType())
					{
						if (f.IsPublic) {
							fields.Add(f.Name);
						}
					}
				}
				referenceField = referenceColumn = 0;
				useReference = true;
			}
			
			if (columns.Count != 0 && fields.Count != 0 && !hideApply && !sheetNotExist) {
				scrollPosition = GUILayout.BeginScrollView(scrollPosition);
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("Reference");
				useReference = GUILayout.Toggle(useReference, "", GUILayout.Width(38));
				if (!useReference) GUI.enabled = false;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Public String variable", GUILayout.MinWidth(125), GUILayout.MaxWidth(250));
				GUILayout.Label("=", GUILayout.Width(38));
				GUILayout.Label("Column", GUILayout.MinWidth(125));
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				referenceField = EditorGUILayout.Popup(referenceField, fields.ToArray(), GUILayout.MinWidth(125), GUILayout.MaxWidth(250));
				GUILayout.Label("=", GUILayout.Width(38));
				referenceColumn = EditorGUILayout.Popup(referenceColumn, columns.ToArray(), GUILayout.MinWidth(125));
				GUILayout.EndHorizontal();
				GUI.enabled = true;
				
				GUILayout.Space(5);
				
				GUILayout.Label("Input");
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(38));
				GUILayout.Label("Public String variable", GUILayout.MinWidth(125), GUILayout.MaxWidth(250));
				GUILayout.Label("=", GUILayout.Width(38));
				GUILayout.Label("Column", GUILayout.MinWidth(125));
				GUILayout.EndHorizontal();
				
				int i = 0;
				foreach (string field in fields)
				{
					GUILayout.BeginHorizontal();
					boolFieldsMonoScript[i] = GUILayout.Toggle(boolFieldsMonoScript[i], "", GUILayout.Width(38));
					if (!boolFieldsMonoScript[i]) GUI.enabled = false;
					GUILayout.Label(field, GUILayout.MinWidth(125), GUILayout.MaxWidth(250));
					GUILayout.Label("=", GUILayout.Width(38));
					fieldsMonoScript[i] = EditorGUILayout.Popup(fieldsMonoScript[i], columns.ToArray(), GUILayout.MinWidth(125));
					GUILayout.EndHorizontal();
					i++;
					GUI.enabled = true;
				}
				GUILayout.EndScrollView();
			}
		}
		
		GUILayout.Space(10f);
		
		if (monoScript == null || columns.Count == 0 || 
			fields.Count == 0 || sheetNotExist)
			GUI.enabled = false;
		
		if (GUI.enabled) {
			GUI.backgroundColor = Color.green;
		} else {
			GUI.backgroundColor = Color.grey;
		}
		
		if (GUILayout.Button("Apply")) {
			if (EditorUtility.DisplayDialog("Are you sure do you apply this?",
            	"GameObject: " + gameObject.name + 
				"\nScript: " + monoScript.GetClass().ToString() +
				"\nExcel File: " + file, "Yes", "No")) {
				ReadExcel (path, sheet);
			}
		}
	}
	
	void GetColums (string pathFile, string sheet) {
		// Must be saved as excel 2003 workbook, not 2007, mono issue really
		string connection = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq="+pathFile+";"; 
		string query = "SELECT * FROM ["+sheet+"$]"; 
		
		// our odbc connector
		OdbcConnection odbcConnection = new OdbcConnection(connection);
		// our command object 
		OdbcCommand odbcCommand = new OdbcCommand(query, odbcConnection); 
		// table to hold the data 
		DataTable data = new DataTable("Data"); 
		
		// open the connection 
		odbcConnection.Open();
		// lets use a datareader to fill that table!
		OdbcDataReader readerData;
		try {
			readerData = odbcCommand.ExecuteReader(); 
		} catch (OdbcException oe) {
			odbcConnection.Close();
			sheetNotExist = true;
			Debug.LogError ("The SHEET name \""+sheet+"\" do not exist or was incorrectly written.\n" + oe.Message + "\n");
			ClearLog ();
			return;
		}
		sheetNotExist = false;
		// now lets blast that into the table by sheer man power! 
		data.Load(readerData); 
		// close that reader! 
		readerData.Close(); 
		// close your connection to the spreadsheet! 
		odbcConnection.Close();
		
		columns.Clear();
		if(data.Columns.Count > 0) {
			foreach (DataColumn column in data.Columns) {
				columns.Add(column.ColumnName);
			}
		}
	}
	
	void ReadExcel(string pathFile, string sheet) {
		// Must be saved as excel 2003 workbook, not 2007, mono issue really
		string connection = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq="+pathFile+";"; 
		string query = "SELECT * FROM ["+sheet+"$]"; 
		
		// our odbc connector
		OdbcConnection odbcConnection = new OdbcConnection(connection);
		// our command object 
		OdbcCommand odbcCommand = new OdbcCommand(query, odbcConnection); 
		// table to hold the data 
		DataTable data = new DataTable("Data"); 
		
		// open the connection 
		odbcConnection.Open();
		// lets use a datareader to fill that table!
		OdbcDataReader readerData;
		try {
			readerData = odbcCommand.ExecuteReader(); 
		} catch (OdbcException oe) {
			Debug.LogError ("The SHEET name \""+sheet+"\" do not exist or was incorrectly written.\n" + oe.Message + "\n");
			odbcConnection.Close();
			return;
		}
		// now lets blast that into the table by sheer man power! 
		data.Load(readerData); 
		// close that reader! 
		readerData.Close(); 
		// close your connection to the spreadsheet! 
		odbcConnection.Close();
		
		if(data.Rows.Count > 0) {
			if (!useReference) {
				int j = 0;
				Component[] components = gameObject.GetComponentsInChildren<Component>();
				int k = 1;
				foreach (Component component in components) {
					if (component != null) {
						EditorUtility.DisplayProgressBar(	"Import Excel Information",
															"Game Object: " + component.gameObject.name,
															(float)k/components.Length);
						
						if (j >= data.Rows.Count) break;
						
						int i = 0;
						if (component.GetType() == monoScript.GetClass()) {
							foreach (FieldInfo f in component.GetType().GetFields())
							{
								if (f.FieldType == string.Empty.GetType())
								{
									if (f.IsPublic) {
										if (boolFieldsMonoScript[i])
											f.SetValue(component, data.Rows[j][data.Columns[fieldsMonoScript[i]]].ToString());
										i++;
									}
								}
							}
							j++;
						}
					}
					k++;
				}
			} else {
				Component[] components = gameObject.GetComponentsInChildren<Component>();
				int k = 1;
				foreach (Component component in components) {
					if (component != null) {
						EditorUtility.DisplayProgressBar(	"Import Excel Information",
															"Game Object: " + component.gameObject.name,
															(float)k/components.Length);
						
						for (int i = 0; i != data.Rows.Count; i++) {
							int j = 0; 
							if (component.GetType() == monoScript.GetClass()) {
								
								if ((component.GetType().GetField(fields[referenceField]).GetValue(component).ToString().Equals( 
										data.Rows[i][data.Columns[referenceColumn]].ToString()))
									)
								{
									
									foreach (FieldInfo f in component.GetType().GetFields())
									{
										if (f.FieldType == string.Empty.GetType())
										{
											if (f.IsPublic) {
												if (boolFieldsMonoScript[j])
													f.SetValue(component, data.Rows[i][data.Columns[fieldsMonoScript[j]]].ToString());
												j++;
											}
										}
									}
									break;
								}
							}
						}
						k++;
					}
				}
			}
			EditorUtility.ClearProgressBar();
		}
	}
	
	void ClearLog () {
	    Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
	    System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
	    MethodInfo method = type.GetMethod("Clear");
		method.Invoke (new object (), null);
	}
}