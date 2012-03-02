using UnityEngine;
using System.Collections;

public class WallSizeSliderHandler : MonoBehaviour {

	public enum SliderWallMeasureType {
		Depth,
		Width,
	}
	
	private WallBuilder wallBuilder;
	
	void Awake(){
		wallBuilder = GameObject.Find("WallBuilder").GetComponent<WallBuilder>();
	}
	
	public SliderWallMeasureType sliderWallMeasureType;
	public UILabel label;
	
	void OnSliderChange(float val){
		switch(sliderWallMeasureType){
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
