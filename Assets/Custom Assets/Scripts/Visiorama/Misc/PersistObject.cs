using UnityEngine;
using System.Collections;

public class PersistObject : MonoBehaviour {

	public int[] levels;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// 
	void OnLevelWasLoaded (int level) {
		if (levels.Length != 0) {
			for (int i = 0; i < levels.Length; i++) {
				if (levels[i] == level) {
					Destroy(gameObject);
				}
			}
		}
	}
	
}
