using UnityEngine;
using UnityEditor;

public class UIWidget3D : EditorWindow  {
	
	string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    
    [MenuItem ("BlackBugio/GUI/Widget")]
    static void Init ()
	{
        UIWidget3D window = (UIWidget3D)EditorWindow.GetWindow (typeof (UIWidget3D));
    }
    
    void OnGUI () {
		if (Selection.transforms.Length != 0)
		{
			if (GUILayout.Button ("Create a Button 3D"))
			{
				foreach (Transform t in Selection.transforms)
				{
					t.gameObject.AddComponent <BoxCollider> ();
					t.gameObject.AddComponent <UIButton3D> ();
					t.gameObject.AddComponent <UIButton3DPosition> ();
					t.gameObject.AddComponent <UIButton3DScale> ();
				}
			}
		}
		else
		{
			GUILayout.Label ("Select a transform!");
		}
    }
}
