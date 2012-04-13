using UnityEngine;
using System.Collections;

public class CheckBoxColorHandler : MonoBehaviour {
	
	private InfoController infoController;
	
	void Start () {
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
	}
	
	void OnClick ()
	{
		//Obter a cor da checkbox
		string colorName = name.Split(' ')[1];
		BrandColorEnum[] colors = Line.CurrentLine.colors;
		bool success  = false;
		
		for(int selectedColorIndex = 0; selectedColorIndex != colors.Length; ++selectedColorIndex)
		{
			if (colorName.Equals(BrandColor.GetColorName(colors[selectedColorIndex])))
			{
				GameObject selectedModule = GameObject.FindWithTag("MovelSelecionado");
				if (selectedModule == null) break;
								
				Line.CurrentLine.GlobalDetailColorIndex = selectedColorIndex;
				
				GameObject selectedMobile = GameObject.FindGameObjectWithTag ("MovelSelecionado");
				if (selectedMobile != null) {
					selectedMobile.GetComponent<InformacoesMovel> ().ChangeDetailColor (selectedColorIndex);										
				}
							
				GameObject[] furniture = GameObject.FindGameObjectsWithTag ("Movel");
				if (furniture != null && furniture.Length != 0) {
					foreach (GameObject mobile in furniture) {
						mobile.GetComponent<InformacoesMovel> ().ChangeDetailColor (selectedColorIndex);										
					}
				}
				
				infoController.selectedColorIndex = selectedColorIndex;
				success = true;
				break;
			}
		}
		
		if (!success)
		{
			Debug.LogError ("Algo aconteceu errado! Nome da cor: " + colorName);
			Debug.LogError ("Nomes das cores padrão");	
			foreach (BrandColorEnum item in colors)
			{
				Debug.LogError ("Cor: " + BrandColor.GetColorName(item));	
			}
		}
	}
}
