using UnityEngine;
using System.Collections;

public class SetInterfaceButtonHandler : MonoBehaviour {
	
	public string interfaceName;
		
	void OnClick ()
	{
		GameController.GetInstance ().GetInterfaceManager ().SetInterface (interfaceName);
	}
}
