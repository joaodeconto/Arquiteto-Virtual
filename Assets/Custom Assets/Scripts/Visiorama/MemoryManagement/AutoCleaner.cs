using UnityEngine;
using System.Collections;

public class AutoCleaner : MonoBehaviour {

	public float TimeBetweenMemoryClean;
	
	// Use this for initialization
	void Start () {
	
		//Prevent zero
		if(TimeBetweenMemoryClean < 0.01f){
			TimeBetweenMemoryClean = 0.001f;
		}
		InvokeRepeating("Clean",TimeBetweenMemoryClean,TimeBetweenMemoryClean);
	}

	private void Clean(){
		System.GC.Collect ();
		System.GC.WaitForPendingFinalizers ();
	}	
}
