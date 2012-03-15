using UnityEngine;
using System.Collections;

public class RotationSun : MonoBehaviour {
	
	[System.Serializable]
	public class VHSlider {
		public float min;
		public float max;
		public float value;
		
		public VHSlider (float min, float max, float value) {
			this.min = min;
			this.max = max;
			this.value = value;
		}
	}
	
	private VHSlider horizontal;
	private VHSlider vertical;
	
	void Start () {
		horizontal = new VHSlider(0f, 359.99f, transform.localEulerAngles.x);
		vertical = new VHSlider(0f, 60f, transform.localEulerAngles.y);
	}
	
	void OnSliderChangeHorizontal (float val)
	{
		horizontal.value = Mathf.Max(horizontal.min, val * horizontal.max);
		transform.localEulerAngles = new Vector3(vertical.value, horizontal.value, 0);
	}
	
	void OnSliderChangeVertical (float val)
	{
		vertical.value = Mathf.Max(vertical.min, val * vertical.max);
		transform.localEulerAngles = new Vector3(vertical.value, horizontal.value, 0);
	}
}
