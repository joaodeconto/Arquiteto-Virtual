using UnityEngine;
using System.Collections;

public class MouseUtils : MonoBehaviour
{

	public static bool MouseClickedInArea (Rect window)
	{
		//Monkey patch ¬¬
		Vector3 position = Input.mousePosition;
		position.y = Screen.height - position.y;

		if (window.Contains (position)) {
			return true;
		}

		return false;
	}

	private static float doubleClickStart = -1;

	public static bool MouseButtonDoubleClickDown (int button, float timer)
	{
		if (Input.GetMouseButtonDown (button)) {
			if ((Time.time - doubleClickStart) < timer) {
				doubleClickStart = -1;
				return true;
			} else {
				doubleClickStart = Time.time;
			}
		}
		return false;
	}

	public static bool MouseButtonDoubleClickUp (int button, float timer)
	{
		if (Input.GetMouseButtonUp (button)) {
			if ((Time.time - doubleClickStart) < timer) {
				doubleClickStart = -1;
				return true;
			} else {
				doubleClickStart = Time.time;
			}
		}
		return false;
	}

	private static object lastObj;

	public static bool MouseButtonDoubleClickDown (int button, float timer, object obj)
	{
		if (Input.GetMouseButtonDown (button)) {
			if (lastObj == obj && (Time.time - doubleClickStart) < timer) {
				doubleClickStart = -1;
				return true;
			} else {
				doubleClickStart = Time.time;
				lastObj = obj;
			}
		}
		return false;
	}

	public static bool MouseButtonDoubleClickUp (int button, float timer, object obj)
	{
		if (Input.GetMouseButtonUp (button)) {
			if (lastObj == obj && (Time.time - doubleClickStart) < timer) {
				doubleClickStart = -1;
				return true;
			} else {
				doubleClickStart = Time.time;
				lastObj = obj;
			}
		}
		return false;
	}

	private static int clickMonkeyPatch = 0;

	public static bool GUIMouseButtonDoubleClickDown (int button, float timer)
	{
		if (Input.GetMouseButtonDown (button)) {
			if (clickMonkeyPatch > 2 && (Time.time - doubleClickStart) < timer) {
				doubleClickStart = -1;
				clickMonkeyPatch = 0;
				return true;
			} else {
				if ((Time.time - doubleClickStart) >= timer)
					clickMonkeyPatch = 0;
				doubleClickStart = Time.time;
				clickMonkeyPatch++;
			}
		}
		return false;
	}

	public static bool GUIMouseButtonDoubleClickUp (int button, float timer)
	{
		if (Input.GetMouseButtonUp (button)) {
			if (clickMonkeyPatch > 2 && (Time.time - doubleClickStart) < timer) {
				doubleClickStart = -1;
				clickMonkeyPatch = 0;
				return true;
			} else {
				if ((Time.time - doubleClickStart) >= timer)
					clickMonkeyPatch = 0;
				doubleClickStart = Time.time;
				clickMonkeyPatch++;
			}
		}
		return false;
	}

}