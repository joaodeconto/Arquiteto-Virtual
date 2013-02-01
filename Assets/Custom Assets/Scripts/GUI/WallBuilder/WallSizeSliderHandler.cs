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
	
	public TextMesh label3d;
	
	private WallBuilder wallBuilder;
	
	void Awake ()
	{
		wallBuilder = GameObject.FindWithTag ("GameController").transform.FindChild ("WallBuilder").GetComponent<WallBuilder> ();
		
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
			
//			TooltipHandler tipHandler = objectLabel.AddComponent<TooltipHandler> ();
//			tipHandler.gameObject = objectLabel;
//			tipHandler.getViaCode = true;
//			tipHandler.SetTooltip (I18n.GetInstance().t (TooltipString));
		}
		
		Invoke ("SetGround", 0.5f);
	}
	
	void SetGround ()
	{
		switch(sliderWallMeasureType)
		{
			case SliderWallMeasureType.Depth:
				wallBuilder.WallDepth 	  = (int)(wallBuilder.MinWallDepth + GetComponent<UISlider>().sliderValue * ( wallBuilder.MaxWallDepth - wallBuilder.MinWallDepth ) );
				wallBuilder.RealWallDepth = wallBuilder.WallDepth / 10.0f;
				label.text = (wallBuilder.WallDepth / 10) + "," + (wallBuilder.WallDepth % 10) + "";
				label3d.text = label.text + "m";
				break;
			case SliderWallMeasureType.Width:
				wallBuilder.WallWidth 	  = (int)(wallBuilder.MinWallWidth + GetComponent<UISlider>().sliderValue * ( wallBuilder.MaxWallWidth - wallBuilder.MinWallWidth ) );	
				wallBuilder.RealWallWidth = wallBuilder.WallWidth / 10.0f;
				label.text = (wallBuilder.WallWidth / 10) + "," + (wallBuilder.WallWidth % 10) + "";
				label3d.text = label.text + "m";
				break;
		}
		
		wallBuilder.BuildGround();
	}
	
	void OnSliderChange(float val)
	{
		switch(sliderWallMeasureType)
		{
			case SliderWallMeasureType.Depth:
				wallBuilder.WallDepth 	  = (int)(wallBuilder.MinWallDepth + val * ( wallBuilder.MaxWallDepth - wallBuilder.MinWallDepth ) );
				wallBuilder.RealWallDepth = wallBuilder.WallDepth / 10.0f;
				label.text = (wallBuilder.WallDepth / 10) + "," + (wallBuilder.WallDepth % 10) + "";
				label3d.text = label.text + "m";
				break;
			case SliderWallMeasureType.Width:
				wallBuilder.WallWidth 	  = (int)(wallBuilder.MinWallWidth + val * ( wallBuilder.MaxWallWidth - wallBuilder.MinWallWidth ) );	
				wallBuilder.RealWallWidth = wallBuilder.WallWidth / 10.0f;
				label.text = (wallBuilder.WallWidth / 10) + "," + (wallBuilder.WallWidth % 10) + "";
				label3d.text = label.text + "m";
				break;
		}
		
		wallBuilder.BuildGround();
	}
}
