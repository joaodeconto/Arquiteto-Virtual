using UnityEngine;
using System.Collections;

public class UILimitButtonHandler : MonoBehaviour {
	
	public UILimitDraggable limitDraggable;
	
	public TypeIndexHandler type;
	
	private UISlicedSprite sprite;
	private UISlicedSprite Sprite
	{
		get {
			if (sprite == null)
			{
				return transform.GetComponentInChildren<UISlicedSprite>();
			}
			return sprite;
		}
	}
	
	void Update () {
		CheckIndex ();
	}
	
	void OnClick () {
		limitDraggable.SetIndex (type);
	}
	
	void CheckIndex () {
		switch (type)
		{
		case TypeIndexHandler.Next:
			if (limitDraggable.IsLimit)
			{
				Sprite.enabled = false;
			}
			else
			{
				Sprite.enabled = true;
			}
			break;
		case TypeIndexHandler.Prev:
			if (limitDraggable.index <= 0)
			{
				Sprite.enabled = false;
			}
			else
			{
				Sprite.enabled = true;
			}
			break;
		}
	}
}
