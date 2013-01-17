using UnityEngine;
using System.Collections;

public class GambiChangeObject : MonoBehaviour {
	
	public float changePositionInX = 10f;
	public int limitObject = 10;
	public GameObject camReference;
	
	private int actualNumber = 0;
	
	void OnGUI ()
	{
		if (actualNumber != 0)
		{
			if (GUILayout.Button ("Prev"))
			{
				Change (actualNumber - 1);
			}
		}
		
		if (actualNumber != limitObject)
		{
			if (GUILayout.Button ("Next"))
			{
				Change (actualNumber + 1);
			}
		}
	}
	
	void Change (int number)
	{
				
		float changeX;
		if (number > actualNumber)
		{
			changeX = -changePositionInX;
		}
		else
		{
			changeX = changePositionInX;
		}
		
		iTween.MoveTo(camReference, iTween.Hash(	iT.MoveTo.position, camReference.transform.position + (Vector3.right * changeX),
													iT.MoveTo.time, 3f));
		
		actualNumber = number;
	}
}
