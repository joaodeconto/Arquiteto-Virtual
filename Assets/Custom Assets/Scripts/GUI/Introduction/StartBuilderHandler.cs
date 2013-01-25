using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartBuilderHandler : MonoBehaviour
{
	public GameObject mainCamera;
	public GameObject intro;
	public GameObject scenario;
	public GameObject rulers;
	public GameObject wallBuilder;
	
	private Vector2 oldDegrees;
	private float oldRange;
	
	void Awake ()
	{
		Invoke ("DeactiveOthersScenes", 0.001f);
	}
	
	void OnClick ()
	{
//		oldDegrees = mainCamera.GetComponent<PanWithMouse> ().degrees;
//		oldRange = mainCamera.GetComponent<PanWithMouse> ().range;
		Destroy(mainCamera.GetComponent<PanWithMouse> ());
		
		scenario.SetActive (true);
		rulers.SetActive (true);
		
		float timePosition = 0f;
		iTweenEvent ite = iTweenEvent.GetEvent(GameObject.Find ("ButtonStartEvent"), "ButtonStartEvent");
		ite.go = mainCamera;
		ite.Play ();
		
		foreach (KeyValuePair<string,object> i in ite.Values)
		{
			timePosition = i.Key.Contains("time") ? (float)i.Value : 
				i.Key.Contains("speed") ? (float)i.Value : 0f;
			if (timePosition != 0f) break;
		}
		
		float timeRotation = 0f;
		ite = iTweenEvent.GetEvent(GameObject.Find ("ButtonStartRotationEvent"), "ButtonStartEvent");
		ite.go = mainCamera;
		ite.Play ();
		
		foreach (KeyValuePair<string,object> i in ite.Values)
		{
			timeRotation = i.Key.Contains("time") ? (float)i.Value : 
				i.Key.Contains("speed") ? (float)i.Value : 0f;
			if (timeRotation != 0f) break;
		}
		
		float time = timePosition >= timeRotation ? timePosition : timeRotation;
		
		Invoke ("EndAnimation", time);
	}
	
	void DeactiveOthersScenes ()
	{
		scenario.SetActive (false);
		rulers.SetActive (false);
		wallBuilder.SetActive (false);
	}
	
	void EndAnimation ()
	{
		if (intro != null) intro.SetActive (false);
		wallBuilder.SetActive (true);
//		mainCamera.AddComponent<PanWithMouse> ();
//		mainCamera.GetComponent<PanWithMouse> ().degrees = oldDegrees;
//		mainCamera.GetComponent<PanWithMouse> ().range = oldRange;
	}
	
	
}