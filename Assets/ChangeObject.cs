using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeObject : MonoBehaviour {
	
	public Transform lineObject;
	
	private List<GameObject> objects = new List<GameObject> ();
	private int actualNumber = -1;
	private GameObject actualObject;
	
	void Start ()
	{
		foreach (Transform typeObject in lineObject)
		{
			foreach (Transform obj in typeObject)
			{
				objects.Add (obj.gameObject);
			}
		}
		
		Change (0);
	}
	
	void OnGUI ()
	{
		if (actualNumber != 0)
		{
			if (GUILayout.Button ("Prev"))
			{
				Change (actualNumber - 1);
			}
		}
		
		if (actualNumber != objects.Count-1)
		{
			if (GUILayout.Button ("Next"))
			{
				Change (actualNumber + 1);
			}
		}
	}
	
	void Change (int number)
	{
		if (number < 0) return;
		if (number >= objects.Count) return;
		
		float lastObjectTo, objectTo;
		if (number > actualNumber)
		{
			lastObjectTo = 100f;
			objectTo = -100f;
		}
		else
		{
			lastObjectTo = -100f;
			objectTo = 100f;
		}
		
//		if (actualNumber != -1)
//		{
//			iTween.MoveTo(actualObject, iTween.Hash(	iT.MoveTo.position, Vector3.right * lastObjectTo,
//														iT.MoveTo.time, 3f,
//														iT.MoveTo.oncomplete, "DestroyObject"));
//		}
		
		DestroyObject ();
		
		GameObject newObject = Instantiate (objects[number], Vector3.right * objectTo, Quaternion.identity) as GameObject;
		
		iTween.MoveTo(newObject, iTween.Hash(	iT.MoveTo.position, Vector3.zero,
												iT.MoveTo.time, 3f));
		
		actualObject = newObject;
		
		actualNumber = number;
	}
	
	void DestroyObject ()
	{
		DestroyImmediate (actualObject);
	}
	
	void AttachObject (GameObject newObject)
	{
		actualObject = newObject;
	}
}
