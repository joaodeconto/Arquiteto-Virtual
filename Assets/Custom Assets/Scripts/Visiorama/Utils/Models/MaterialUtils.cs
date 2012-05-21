using UnityEngine;
using System.Text.RegularExpressions;

public class MaterialUtils
{
	public static void ResizeWallMaterial (GameObject obj,
											float textScaleX,
											float textScaleY)
	{
		ResizeWallMaterial (obj, textScaleX, textScaleY, 0, 0);
	}

	public static void ResizeWallMaterial (	GameObject obj,
											float textScaleX,
											float textScaleY,
											float textOffsetX,
											float textOffsetY)
	{
		Vector2 textScale  = new Vector2 (textScaleX, textScaleY);
		Vector2 textOffset = new Vector2 (textOffsetX,textOffsetY);

		if (obj.transform.childCount > 0)
		{
			foreach (Material cMaterial in obj.transform.GetChild(0).renderer.materials)
			{
				cMaterial.mainTextureScale  = textScale;
				cMaterial.mainTextureOffset = textOffset;
				cMaterial.SetTextureScale  ("_BumpMap", textScale);
				cMaterial.SetTextureOffset ("_BumpMap", textOffset);
			}
		}
		else
		{
			foreach (Material cMaterial in obj.renderer.materials)
			{
				cMaterial.mainTextureScale = textScale;
				cMaterial.mainTextureOffset = textOffset;
				cMaterial.SetTextureScale ("_BumpMap", textScale);
				cMaterial.SetTextureOffset ("_BumpMap", textOffset);
			}
		}
	}
}
