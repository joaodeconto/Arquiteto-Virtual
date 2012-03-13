using UnityEngine;
using System.Collections;

public class TooltipTeste : MonoBehaviour {
	
	public TooltipHandler tooltip;
	public string label;
	
	// Use this for initialization
	void Start () {
		tooltip.SetTooltip(label);
	}
}
