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
		if (this.enabled)
		{
			switch(wallBuilderButton){
	//			case WallBuilderButtonEnum.BuildGround:
	//				wallBuilder.BuildGround();
	//				break;
				case WallBuilderButtonEnum.BuildWalls:
					wallBuilder.BuildWalls ();
					foreach (MonoBehaviour component in transform.GetComponents(typeof(MonoBehaviour)))
					{
						component.enabled = false;
					}
					break;
				case WallBuilderButtonEnum.Restart:
					wallBuilder.Restart();
					break;
			}
		}
	}
}
