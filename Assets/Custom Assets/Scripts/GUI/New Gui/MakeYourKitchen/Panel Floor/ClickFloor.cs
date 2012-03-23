using UnityEngine;
using System.Collections;

public class ClickFloor : MonoBehaviour {
	
	public Texture2D textureFloor {get; set;}
	public int FloorTextureIndex;
	
	void OnClick ()
	{
		GameObject chaoParent = GameObject.FindWithTag("ChaoParent");
		chaoParent.renderer.material.mainTexture = textureFloor;
		
		transform.parent.GetComponent<CatalogFloorButtonHandler>().SelectedFloorIndex = FloorTextureIndex;
	}
}
