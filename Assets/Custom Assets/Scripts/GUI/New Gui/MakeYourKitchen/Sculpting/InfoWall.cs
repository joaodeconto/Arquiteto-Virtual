using UnityEngine;
using System.Collections;

[System.Serializable]
public class InfoWall : MonoBehaviour
{
	public Transform wall;
	public Transform rightWall;
	public Transform leftWall;
	public Color color;
	public Texture texture;
	
	public void SetColor (Color color)
	{
		this.color = color;
	}

	public void CopyFrom (InfoWall info)
	{
		this.wall 		= info.wall;
		this.rightWall	= info.rightWall;
		this.leftWall 	= info.leftWall;
		this.color 		= info.color;
		this.texture	= info.texture;
	}
}