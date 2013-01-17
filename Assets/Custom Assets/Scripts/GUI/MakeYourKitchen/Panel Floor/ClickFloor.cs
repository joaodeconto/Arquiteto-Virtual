using UnityEngine;
using System.Collections;

public class ClickFloor : MonoBehaviour {
	
	public Texture2D textureFloor {get; set;}
	public int FloorTextureIndex;
	
	void OnClick ()
	{
		GameObject floor = GameObject.FindWithTag("Chao");
		float floorWidth = floor.transform.localScale.x;
		float floorDepth = floor.transform.localScale.y;
		floor.renderer.material.mainTexture = textureFloor;

		foreach (Material cMaterial in floor.renderer.materials)
		{
			cMaterial.mainTextureScale = new Vector2 (floorWidth, floorDepth);
			cMaterial.SetTextureScale ("_BumpMap", new Vector2 (floorWidth, floorDepth));
			cMaterial.SetTextureScale ("_Cube", new Vector2 (floorWidth, floorDepth));
		}
		
		transform.parent.GetComponent<CatalogFloorButtonHandler>().SelectedFloorIndex = FloorTextureIndex;
	}
}
