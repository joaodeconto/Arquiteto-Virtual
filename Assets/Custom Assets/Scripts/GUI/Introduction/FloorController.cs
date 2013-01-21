using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {

	void OnEnable ()
	{
		foreach (Transform child in transform)
		{
			if (child.localScale.x < 3 || child.localScale.y < 3)
				Destroy (child.gameObject);
		}
	}
}
