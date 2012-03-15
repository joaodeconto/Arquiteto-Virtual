using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

[CustomEditor(typeof(TooltipHandler))]
public class TooltipHandlerEditor : Editor
{
	TooltipHandler tooltipHandler;
	
	void OnEnable ()
	{
		tooltipHandler = target as TooltipHandler;
		if (tooltipHandler.gameObject == null && !tooltipHandler.getViaCode) {
			tooltipHandler.gameObject = tooltipHandler.transform.gameObject;
		}
	}
	
	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Development: BlackBugio ®");
		GUILayout.Space (10f);
		
		tooltipHandler.getViaCode = EditorGUILayout.Toggle ("Get Via Code?", tooltipHandler.getViaCode);
		GUILayout.Space (5f);
		
		if (!tooltipHandler.getViaCode)
		{
			tooltipHandler.getViaLabel = EditorGUILayout.Toggle ("Get Via Label?", tooltipHandler.getViaLabel);
			GUILayout.Space (5f);
			if (!tooltipHandler.getViaLabel) {
				GUILayout.Label ("GameObject Reference:");
				tooltipHandler.gameObject = EditorGUILayout.ObjectField (tooltipHandler.gameObject, typeof(GameObject)) as GameObject;
				GUILayout.Space (5f);
				
				if (tooltipHandler.gameObject != null)
				{
					Component[] comps = tooltipHandler.gameObject.GetComponents (typeof(Component));
					tooltipHandler.components = new List<Type> ();
					foreach (Component c in comps)
					{
						if (c.GetType ().GetFields ().Length != 0 &&
							c.GetType ().ToString() != "TooltipHandler") {
							tooltipHandler.components.Add (c.GetType ());
						}
					}
					if (tooltipHandler.components.Count == 0 ||
						tooltipHandler.selectedComp > tooltipHandler.components.Count)
					{
						tooltipHandler.selectedComp = 0;
						return;
					}
					
					GUILayout.Label ("Components:");
					tooltipHandler.selectedComp = EditorGUILayout.Popup (tooltipHandler.selectedComp, ComponentsNames ());
					GUILayout.Space (5f);
					
					if (tooltipHandler.selectedComp != 0)
					{
						tooltipHandler.vars = new List<FieldInfo> ();
						string type = "";
						foreach (FieldInfo f in tooltipHandler.components[tooltipHandler.selectedComp-1].GetFields())
						{
							if (f.FieldType == type.GetType ())
							{
								tooltipHandler.vars.Add (f);
							}
						}
						
						if (tooltipHandler.vars.Count == 0 ||
							tooltipHandler.selectedVar > tooltipHandler.vars.Count)
						{
							tooltipHandler.selectedVar = 0;
							return;
						}
						
						GUILayout.Label ("String Variables:");
						tooltipHandler.selectedVar = EditorGUILayout.Popup (tooltipHandler.selectedVar, VarsNames ());
						GUILayout.Space (5f);
						if (tooltipHandler.selectedVar != 0)
						{
							string val = tooltipHandler.vars [tooltipHandler.selectedVar - 1].GetValue (
											tooltipHandler.gameObject.GetComponent (
											tooltipHandler.components [tooltipHandler.selectedComp - 1])).ToString ();
							tooltipHandler.label = val;
						}
					}
				}
			} else {
			GUILayout.Label ("Digit a label to appear in tooltip:");
			tooltipHandler.label = GUILayout.TextField(tooltipHandler.label);
			GUILayout.Space (5f);
			}
		} else {
			GUILayout.Label ("Get Via Code with this method:\n" +
				"non-static TooltipHandler.SetTooltip(string label);\n\n" +
				"Example:\n\n" +
				"TooltipHandler tooltip;\n" +
				"tooltip = GetComponent<TooltipHandler>();\n" +
				"tooltip.SetTooltip('Any text');");
			GUILayout.Space (5f);
		}
	}
	
	string[] ComponentsNames ()
	{
		string[] names = new string[tooltipHandler.components.Count + 1];
		names [0] = "None";
		int i = 1;
		foreach (Type c in tooltipHandler.components) {
			names [i] = c.Name;
			++i;
		}
		return names;
	}
	
	string[] VarsNames ()
	{
		string[] names = new string[tooltipHandler.vars.Count + 1];
		names [0] = "None";
		int i = 1;
		foreach (FieldInfo v in tooltipHandler.vars) {
			names [i] = v.Name;
			++i;
		}
		return names;
	}
}