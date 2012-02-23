using UnityEngine;
using System.Collections;

public class SomClique : MonoBehaviour {
	
	private static AudioSource audio;
	
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	public static void Play() {
		audio.Play();
	}
}
