using UnityEngine;
using System.Collections;

public class MouseUtils : MonoBehaviour {

	public static bool MouseClickedInArea (Rect window) {
		
		//Monkey patch ¬¬
		Vector3 position = Input.mousePosition;
		position.y = Screen.height - position.y;
		
		if (window.Contains (position)) {
			return true;
		}
		
		return false;
	}
	
	private static float doubleClickStart = -1;
	private static int monkeyPatchClick = 0;
	public static bool MouseButtonDoubleClickDown (int button, float timer) {
		if (Input.GetMouseButtonDown (button)) {
			if (monkeyPatchClick > 1 && (Time.time - doubleClickStart) < timer) {
		        doubleClickStart = -1;
				monkeyPatchClick = 0;
				return true;
		    }
		    else {
				if ((Time.time - doubleClickStart) > timer)
					monkeyPatchClick = 0;
		        doubleClickStart = Time.time;
				monkeyPatchClick++;
		    }
		}
		return false;
	}
	
	public static bool MouseButtonDoubleClickUp (int button, float timer) {
		if (Input.GetMouseButtonUp (button)) {
			if (monkeyPatchClick > 1 && (Time.time - doubleClickStart) < timer) {
		        doubleClickStart = -1;
				monkeyPatchClick = 0;
				return true;
		    }
		    else {
				if ((Time.time - doubleClickStart) > timer)
					monkeyPatchClick = 0;
		        doubleClickStart = Time.time;
				monkeyPatchClick++;
		    }
		}
		return false;
	}
}
