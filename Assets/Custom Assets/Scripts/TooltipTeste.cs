using UnityEngine;
using System.Collections;

public class TooltipTeste : MonoBehaviour {
	
	public TooltipHandler tooltip;
	
	// Use this for initialization
	void Start () {
		tooltip.SetTooltip("Teste\n Teste Teste Teste Teste Teste Teste\nTeste Teste Teste");
	}
}
