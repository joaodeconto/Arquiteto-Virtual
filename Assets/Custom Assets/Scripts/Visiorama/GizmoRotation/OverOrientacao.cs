using UnityEngine;
using System.Collections;

public enum Axis {
	x, y, z, others
}

public class OverOrientacao : MonoBehaviour
{
	public Axis axis = Axis.others;
	
	private Color corInicial;
	private Camera mainCamera;
	private Camera thisCamera;
	
	void Start () {
		if (renderer != null)
			corInicial = renderer.material.color;
		
		thisCamera = transform.parent.GetComponentInChildren<Camera>() != null ?
					 transform.parent.GetComponentInChildren<Camera>() :
					 transform.parent.transform.parent.GetComponentInChildren<Camera>();
		mainCamera = GameObject.FindWithTag("MainCamera").camera;
	}

	#if UNITY_ANDROID || UNITY_IPHONE
	void Update () {
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Stationary) {
				Ray ray = thisCamera.ScreenPointToRay(touch.position);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					if (hit.transform == transform) Over();
				}
			} else {
				Out();
			}
		}
	}
	#else
	
	void OnMouseEnter () {
		Over ();
	}
	
	void OnMouseExit () {
		Out ();
	}
	#endif
	
	void Over () {
		if (renderer != null) {
			Color cor = renderer.material.color;
			if (axis == Axis.x)
				renderer.material.color = new Color(255, 100, 100);
			if (axis == Axis.y)
				renderer.material.color = new Color(100, 255, 100);
			if (axis == Axis.z)
				renderer.material.color = new Color(100, 100, 255);
			if (axis == Axis.others)
				renderer.material.color = new Color(255, 255, 255);
		}
	}
	
	void Out () {
		if (renderer != null)
			renderer.material.color = corInicial;
	}
}