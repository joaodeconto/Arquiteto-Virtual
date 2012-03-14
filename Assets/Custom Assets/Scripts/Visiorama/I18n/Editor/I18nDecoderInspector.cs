using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(I18nDecoder))]
public class I18nDecoderInspector : Editor
{
	private I18nDecoder decoder;
	private UILabel uiLabelTemp;
	
	void OnEnable ()
	{
		decoder = target as I18nDecoder;
	}
	
	public override void OnInspectorGUI ()
	{
		GUILayout.Label("Blackbugio:");
		GUILayout.Space(10f);
		uiLabelTemp = EditorGUILayout.ObjectField("Adicionar label:", 
												  uiLabelTemp, 
												  typeof(UILabel), 
												  true) as UILabel;
		if (uiLabelTemp != null)
		{
			decoder.labels.Add (uiLabelTemp);
			uiLabelTemp = null;
		}
		
		GUILayout.Space (10f);
		GUILayout.Label("Labels para converter:");
		GUILayout.Space(5f);
		if (decoder.labels.Count != 0) {
			for (int i = 0; i != decoder.labels.Count; ++i)
			{
				GUILayout.BeginHorizontal();
					GUILayout.Label(decoder.labels[i].gameObject.name + " - "+ decoder.labels[i].text);
					decoder.labels[i] = EditorGUILayout.ObjectField(decoder.labels[i],
																    typeof(UILabel),
																    true,
																    GUILayout.Width(200f)) as UILabel;
																    
					if(GUILayout.Button("Deletar",GUILayout.Width(60f)))
					{
						decoder.labels.RemoveAt (i);
						break;
					}
				GUILayout.EndHorizontal();
			}
		}
	}
}
