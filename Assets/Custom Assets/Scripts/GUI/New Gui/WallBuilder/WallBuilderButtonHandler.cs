using UnityEngine;
using System.Collections;

public class WallBuilderButtonHandler : MonoBehaviour {
	
	public enum WallBuilderButtonEnum {
		BuildGround,
		BuildWalls,
		Restart,
	}
	
	public WallBuilderButtonEnum wallBuilderButton;
	public string TooltipString;
	
	private WallBuilder wallBuilder;
	
	void Start(){
	
		wallBuilder = GameObject.Find("WallBuilder").GetComponent<WallBuilder>();
		
		TooltipHandler tipHandler = gameObject.AddComponent<TooltipHandler>();
		tipHandler.gameObject = gameObject;
		tipHandler.SetTooltip(I18n.t (TooltipString));
	}
	
	void OnClick(){
		switch(wallBuilderButton){
			case WallBuilderButtonEnum.BuildGround:
				wallBuilder.BuildGround();
				break;
			case WallBuilderButtonEnum.BuildWalls:
				wallBuilder.BuildWalls ();
				break;
			case WallBuilderButtonEnum.Restart:
				wallBuilder.Restart();
				break;
		}
	}
	
	void Update()
	{
		if(Input.GetKey(KeyCode.P))
		{
			Debug.Break();
		}
	}
}
