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
}
