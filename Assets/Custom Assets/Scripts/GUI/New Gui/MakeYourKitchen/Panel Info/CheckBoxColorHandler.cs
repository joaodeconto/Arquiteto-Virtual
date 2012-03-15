using UnityEngine;
using System.Collections;

public class CheckBoxColorHandler : MonoBehaviour {

	void OnClick ()
	{
		//Obter a cor da checkbox
		string colorName = name.Split(' ')[1];
		BrandColorEnum[] colors = Line.CurrentLine.colors;
		bool success  = false;
		
		for(int i = 0; i != colors.Length; ++i)
		{
			if (colorName.Equals(BrandColor.GetColorName(colors[i])))
			{
				GameObject selectedModule = GameObject.FindWithTag("MovelSelecionado");
				if (selectedModule == null) break;
				
				if (i == Line.CurrentLine.colors.Length - 1)
					selectedModule.GetComponent<InformacoesMovel> ().Colorize ((uint)i, 0);
				else 
					selectedModule.GetComponent<InformacoesMovel> ().Colorize ((uint)i, (uint)i + 1);
				
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
