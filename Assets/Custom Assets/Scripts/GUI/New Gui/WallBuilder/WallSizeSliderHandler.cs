using UnityEngine;
using System.Collections;

public class WallSizeSliderHandler : MonoBehaviour {

	public enum SliderWallMeasureType {
		Depth,
		Width,
	}
	
	public SliderWallMeasureType sliderWallMeasureType;
	public UILabel label;
	public string TooltipString;
	
	private WallBuilder wallBuilder;
	
	void Awake ()
	{
		wallBuilder = GameObject.Find ("WallBuilder").GetComponent<WallBuilder> ();
		
		if (TooltipString.Length == 0)
		{
			Debug.LogError ("Deve-se preencher a string");
		}
		else
		{
			GameObject objectLabel 	  = label.gameObject;
			
			BoxCollider labelCollider = objectLabel.AddComponent<BoxCollider>();
			labelCollider.center = new Vector3(-0.8f,-1.05f,0.0f);
			labelCollider.size   = new Vector3( 7.0f, 1.55f,0.0f);
			
			TooltipHandler tipHandler = objectLabel.AddComponent<TooltipHandler> ();
			tipHandler.gameObject = objectLabel;
			tipHandler.SetTooltip (I18n.t (TooltipString));
			
		}
	}
	
	void OnSliderChange(float val)
	{
		switch(sliderWallMeasureType)
		{
			case SliderWallMeasureType.Depth:
				wallBuilder.WallDepth = (int)Mathf.Max(wallBuilder.MinWallDepth, val * wallBuilder.MaxWallDepth);
				label.text = wallBuilder.WallDepth + "m";
				break;
			case SliderWallMeasureType.Width:
				wallBuilder.WallWidth = (int)Mathf.Max (wallBuilder.MinWallWidth, val * wallBuilder.MaxWallWidth);	
				label.text = wallBuilder.WallWidth + "m";
				break;
		}
	}
}
