using UnityEngine;
using System.Collections;

public class PersistObject : MonoBehaviour {
	
	bool awake = false;
	
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
		Invoke ("AwakeTrue", 0.5f);
	}
	
	void AwakeTrue () { awake = true; }
	
	void OnLevelWasLoaded (int level)
	{
		if (level == 0)
		{
			if (awake) Destroy(gameObject);
			else awake = true;
		}
	}
	
}
