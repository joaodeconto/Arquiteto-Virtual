using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(BoundingMeshCollider))]
public class BoundingMeshColliderInspector : Editor
{
	BoundingMeshCollider _target;
	
	private SerializedObject serObj;

	public void OnEnable () 
	{
		_target = target as BoundingMeshCollider;
	}
	
    public override void OnInspectorGUI () 
    {
		if (GUILayout.Button("Add")) _target.boxColliders.Add(Vector3.zero);
		
		if (_target.boxColliders.Count != 0)
		{
			for (int i = 0; i != _target.boxColliders.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				_target.boxColliders[i] = EditorGUILayout.Vector3Field (""+i, _target.boxColliders[i]);
				
				if (GUILayout.Button("-"))
				{
					_target.boxColliders.RemoveAt(i);
					--i;
				}
				EditorGUILayout.EndHorizontal();
			}
		}
    }
	
	void OnSceneGUI ()
	{
		Handles.DrawAAPolyLine (_target.boxColliders.ToArray());
		
		
		if (_target.boxColliders.Count != 0)
		{
			for (int i = 0; i != _target.boxColliders.Count; i++)
			{
				_target.boxColliders[i] = Handles.PositionHandle (_target.boxColliders[i], _target.transform.rotation);
				
			}
		}
	}
}
