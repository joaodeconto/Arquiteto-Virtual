using UnityEngine;
using System.Collections;

public enum DoorSideEnum
{
	LEFT,
	RIGHT,
}

public class CheckBoxDoorSideHandler : MonoBehaviour {
	
	public DoorSideEnum doorSideEnum;
	
	void OnClick ()
	{
		bool success = false;
		GameObject selectedModule = GameObject.FindWithTag("MovelSelecionado");
		
		if (selectedModule == null)
		{
			Debug.LogError ("Não foi possível encontrar o móvel selecionado!");
		}
					
		selectedModule.GetComponent<InformacoesMovel>().ToogleDoorSide();	
		
	}
}
