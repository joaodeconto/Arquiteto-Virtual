using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKey(KeyCode.LeftShift) ||
			Input.GetKey(KeyCode.RightShift)) &&
			Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
			
			if ((Input.GetKey(KeyCode.LeftShift) ||
				Input.GetKey(KeyCode.RightShift)) &&
				Input.GetKeyDown(KeyCode.Backspace))
			{
				Application.LoadLevel(Application.loadedLevel);
			} else if (Input.GetKeyDown(KeyCode.Backspace))
			{
				Application.LoadLevel(0);
			}
	}
}