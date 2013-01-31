using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackHandler : MonoBehaviour {
	
	public GameObject eventReceiver;
	public iTweenEvent[] iTweenEvents;
	public GameObject[] ActiveObjects;
	public GameObject[] ActiveObjectsEndAnimation;
	public GameObject[] DeativeObjects;
	public GameObject[] DeativeObjectsEndAnimation;
	
	void Awake ()
	{
		for (int i = 0; i != iTweenEvents.Length; i++)
		{
			if (eventReceiver != null) iTweenEvents[i].go = eventReceiver;
			else if (iTweenEvents[i].go == null) iTweenEvents[i].go = gameObject;
		}
	}
	
	void OnClick ()
	{
		float time = 0f;
		
		for (int i = 0; i != iTweenEvents.Length; i++)
		{
			foreach (KeyValuePair<string,object> val in iTweenEvents[i].Values)
			{
				time += val.Key.Contains("time") ? (float)val.Value : 
					val.Key.Contains("speed") ? (float)val.Value : 0f;
				
				if (time != 0f) break;
			}
			iTweenEvents[i].Play(false);
		}
		
		foreach (GameObject go in ActiveObjects)
		{
			go.SetActive (true);
		}
		foreach (GameObject go in DeativeObjects)
		{
			go.SetActive (false);
		}
	
		Invoke ("EndTween", time);
	}
	
	void EndTween ()
	{
		foreach (GameObject go in ActiveObjectsEndAnimation)
		{
			go.SetActive (true);
		}
		foreach (GameObject go in DeativeObjectsEndAnimation)
		{
			go.SetActive (false);
		}
	}
}
