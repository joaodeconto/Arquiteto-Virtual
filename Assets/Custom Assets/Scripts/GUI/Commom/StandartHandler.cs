using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandartHandler : MonoBehaviour {
	
	public GameObject eventReceiver;
	public iTweenEvent[] iTweenEvents;
	public UITweener[] uiTweeners;
	public bool activeUiTweener;
	public bool playbackUiTweener;
	public GameObject[] ActiveObjects;
	public GameObject[] ActiveObjectsEndAnimation;
	public Behaviour[] ActiveComponents;
	public Behaviour[] ActiveComponentsEndAnimation;
	public GameObject[] DeactiveObjects;
	public GameObject[] DeactiveObjectsEndAnimation;
	public Behaviour[] DeactiveComponents;
	public Behaviour[] DeactiveComponentsEndAnimation;
	
	bool clicked = false;
	
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
		if (clicked) return;
		
		clicked = true;
		
		float time = 0f;
		
		for (int i = 0; i != iTweenEvents.Length; i++)
		{
			foreach (KeyValuePair<string,object> val in iTweenEvents[i].Values)
			{
				float t = 0f;
				t += val.Key.Contains("time") ? (float)val.Value : 0f;
				
				if (t > time)
				{
					time = t;
					break;
				}
			}
			iTweenEvents[i].Play();
		}
		
		foreach (UITweener uit in uiTweeners)
		{
			uit.enabled = activeUiTweener;
			if (!activeUiTweener)
			{
				uit.Play (!playbackUiTweener);
				if (uit.duration > time)
				{
					time = uit.duration;
				}
			}
		}
		
		foreach (Behaviour b in ActiveComponents)
		{
			b.enabled = true;
		}
		
		foreach (Behaviour b in DeactiveComponents)
		{
			b.enabled = false;
		}
		
		foreach (GameObject go in ActiveObjects)
		{
			go.SetActive (true);
		}
		foreach (GameObject go in DeactiveObjects)
		{
			go.SetActive (false);
		}
	
		Invoke ("EndTween", time);
	}
	
	void EndTween ()
	{
		clicked = false;
		
		foreach (Behaviour b in ActiveComponentsEndAnimation)
		{
			b.enabled = true;
		}
		
		foreach (Behaviour b in DeactiveComponentsEndAnimation)
		{
			b.enabled = false;
		}
		
		foreach (GameObject go in ActiveObjectsEndAnimation)
		{
			go.SetActive (true);
		}
		foreach (GameObject go in DeactiveObjectsEndAnimation)
		{
			go.SetActive (false);
		}
	}
}
