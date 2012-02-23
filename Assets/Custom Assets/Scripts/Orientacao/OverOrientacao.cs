using UnityEngine;
using System.Collections;

public enum Axis {
	x, y, z, others
}

public class OverOrientacao : MonoBehaviour
{
	public Axis axis = Axis.others;
	Color corInicial;
	
	void Start () {
		if (renderer != null)
			corInicial = renderer.material.color;
	}
	
	// Update is called once per frame
	void OnMouseEnter () {
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
	
	void OnMouseExit () {
		if (renderer != null)
			renderer.material.color = corInicial;
	}
}