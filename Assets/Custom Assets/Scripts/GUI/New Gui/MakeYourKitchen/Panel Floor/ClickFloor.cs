using UnityEngine;
using System.Collections;

public class ClickFloor : MonoBehaviour {
	
	public Texture2D textureFloor {get; set;}
	
	void OnClick () {
		GameObject chaoParent = GameObject.FindWithTag("ChaoParent");
		chaoParent.renderer.material.mainTexture = textureFloor;
	}
}
