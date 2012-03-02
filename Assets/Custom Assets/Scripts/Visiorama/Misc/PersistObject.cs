using UnityEngine;
using System.Collections;

public class PersistObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
}
