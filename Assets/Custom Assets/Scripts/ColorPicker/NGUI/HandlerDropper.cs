using UnityEngine;
using System.Collections;

public class HandlerDropper : MonoBehaviour {

	void OnClick() {
		SendMessageUpwards("DropperOn");
	}
}
