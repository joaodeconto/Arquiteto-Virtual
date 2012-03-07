using UnityEngine;
using System.Collections;

public class PersistObject : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (Application.loadedLevelName == "WallBuilder" || 
			Application.loadedLevelName == "MakeYourKitchen") {
			DontDestroyOnLoad(gameObject);
		}
	}
	
	void LateUpdate () {
		if (Application.loadedLevelName != "WallBuilder" && 
			Application.loadedLevelName != "MakeYourKitchen") {
			Destroy(gameObject);
		}
	}
}
