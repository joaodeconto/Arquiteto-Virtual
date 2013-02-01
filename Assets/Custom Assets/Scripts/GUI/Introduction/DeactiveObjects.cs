using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeactiveObjects : MonoBehaviour
{
	public GameObject[] gameObjects;
	public float time;
	
	void Awake ()
	{
		Invoke ("DeactiveGOs", time);
	}
	
	void DeactiveGOs ()
	{
		foreach (GameObject go in gameObjects)
		{
			go.SetActive (false);
		}
	}
	
}