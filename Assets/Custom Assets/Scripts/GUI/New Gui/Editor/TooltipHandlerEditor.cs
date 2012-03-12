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
		if (tooltipHandler.gameObject == null)
			tooltipHandler.gameObject = tooltipHandler.transform.gameObject;
	}
	
	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Development: BlackBugio ®");
		GUILayout.Space (10f);
		
		tooltipHandler.gameObject = EditorGUILayout.ObjectField (tooltipHandler.gameObject, typeof(GameObject)) as GameObject;
		GUILayout.Space (5f);
		
		if (tooltipHandler.gameObject != null) {
			Component[] comps = tooltipHandler.gameObject.GetComponents (typeof(Component));
			tooltipHandler.components = new List<Type> ();
			foreach (Component c in comps) {
				if (c.GetType ().GetFields ().Length != 0) {
					tooltipHandler.components.Add (c.GetType ());
				}
			}
			
			tooltipHandler.selectedComp = EditorGUILayout.Popup (tooltipHandler.selectedComp, ComponentsNames ());
			GUILayout.Space (5f);
			
			if (tooltipHandler.selectedComp != 0) {
				tooltipHandler.vars = new List<FieldInfo> ();
				foreach (FieldInfo f in tooltipHandler.components[tooltipHandler.selectedComp-1].GetFields()) {
					if (f.FieldType == tooltipHandler.label.GetType ()) {
						tooltipHandler.vars.Add (f);
					}
				}
				tooltipHandler.selectedVar = EditorGUILayout.Popup (tooltipHandler.selectedVar, VarsNames ());
				if (tooltipHandler.selectedVar != 0) {
					string val = tooltipHandler.vars [tooltipHandler.selectedVar - 1].GetValue (
									tooltipHandler.gameObject.GetComponent (
									tooltipHandler.components [tooltipHandler.selectedComp - 1])).ToString ();
					tooltipHandler.SetTooltip (val);
				}
			}
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