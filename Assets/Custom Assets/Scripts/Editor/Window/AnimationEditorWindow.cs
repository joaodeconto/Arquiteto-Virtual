using UnityEngine;
using UnityEditor;

public class AnimationEditorWindow : EditorWindow  {

    [MenuItem ("BlackBugio/Animation")]
    static void Init ()
	{
        AnimationEditorWindow window = (AnimationEditorWindow)EditorWindow.GetWindow (typeof (AnimationEditorWindow));
    }
    
    void OnGUI ()
	{
		if (Selection.transforms.Length != 0)
		{
			if (GUILayout.Button ("Remove all animations"))
			{
				foreach (Transform t in Selection.transforms)
				{
					Transform[] allChilds = t.GetComponentsInChildren<Transform> ();
					foreach (Transform child in allChilds)
					{
						if (child.animation != null)
						{
							DestroyImmediate (child.animation);
						}
					}
				}
			}
		}
		else
		{
			GUILayout.Label ("Select a transform!");
		}
    }
}