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
	
	void Start()
	{
		wallBuilder = GameObject.FindWithTag ("GameController").transform.FindChild ("WallBuilder").GetComponent<WallBuilder> ();
	}
	
	
	void OnClick()
	{
		if (this.enabled)
		{
			switch(wallBuilderButton){
	//			case WallBuilderButtonEnum.BuildGround:
	//				wallBuilder.BuildGround();
	//				break;
				case WallBuilderButtonEnum.BuildWalls:
					float time = 0;
					foreach (TweenColor tc in GameObject.Find ("UI BlurInterface").GetComponentsInChildren<TweenColor>())
					{
						if (time < tc.duration)
						{
							time = tc.duration;
						}
						
						if (tc.GetComponent<UILabel>() != null) tc.GetComponent<UILabel>().text = "Carregando";
					
						tc.Play (false);
					}
					Invoke ("ChangeScene", time);
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
	
	void ChangeScene ()
	{
		wallBuilder.BuildWalls ();
	}
}
