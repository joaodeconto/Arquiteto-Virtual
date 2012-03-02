using UnityEngine;
using System.Collections;

public class WallBuilderButtonHandler : MonoBehaviour {
	
	public enum WallBuilderButtonEnum {
		BuildGround,
		BuildWalls,
		Restart,
	}
	
	public WallBuilderButtonEnum wallBuilderButton;
	private WallBuilder wallBuilder;
	
	void Start(){
		wallBuilder = GameObject.Find("WallBuilder").GetComponent<WallBuilder>();
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
}
