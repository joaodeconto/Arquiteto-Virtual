using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class I18n {

	static private int selectedLanguage;
	static private string[] availableLanguages;
	static private Hashtable[] hashWords;

	#region initialize translations
	static public void Initialize(){
		selectedLanguage  = 0;
		availableLanguages = new string[3]{"Portugu�s","English","Espa�ol"};
		hashWords = new Hashtable[3];
		hashWords[0] = new Hashtable();
		hashWords[1] = new Hashtable();
		hashWords[2] = new Hashtable();
		InitializePortuguese();
		InitializeEnglish();
		InitializeSpanish();
		
		#region Validating I18n
		int[] checkPhrasesNumberArray = new int[availableLanguages.Length];
		for (int i = 0; i != availableLanguages.Length; ++i)
		{
			checkPhrasesNumberArray[i] = 0;
			foreach (DictionaryEntry de in hashWords[i])
			{
				++checkPhrasesNumberArray[i];
			}
		}
		
		if (checkPhrasesNumberArray[0] != checkPhrasesNumberArray[1] ||
			checkPhrasesNumberArray[0] != checkPhrasesNumberArray[2])
		{
			Debug.LogError ("Alguma l�ngua no I18n est� com mais frases que outras. Por favor verifique o arquivo.");
			Debug.Break ();
		}
		#endregion
	}
	
	static private void InitializePortuguese ()
	{
		//Escolha cozinha
		hashWords [0] ["Escolha sua linha favorita"] = "Escolha sua linha favorita";
		
		//Montar paredes
		hashWords [0] ["Medidas"] 	  = "Medidas";
		hashWords [0] ["Largura"] 	  = "Largura";
		hashWords [0] ["Comprimento"] = "Comprimento";
		
		hashWords [0] ["A��es"] 	  = "A��es";
		hashWords [0] ["Preencher �rea"]  = "Preencher �rea";
		hashWords [0] ["Colocar Paredes"] = "Colocar Paredes";
		hashWords [0] ["Reiniciar"]		  = "Reiniciar";
		
		//Monte sua cozinha
		hashWords [0] ["M�dulos"] 	 = "M�dulos";
		hashWords [0] ["Ilumina��o"] = "Ilumina��o";
		hashWords [0] ["Extras"] 	 = "Extras";
		
		hashWords [0] ["C�digo"] = "C�digo";
		hashWords [0] ["LXPXA"] = "LXPXA";
		hashWords [0] ["Descri��o"] = "Descri��o";
		hashWords [0] ["Linha"] = "Linha";
		hashWords [0] ["Categoria"] = "Categoria";
		
		//Montar paredes - Tooltips
		hashWords [0] ["tip-profundidade"]	= "Selecionar a profundidade (em metros) que o ch�o da sua cozinha ter�.";
		hashWords [0] ["tip-largura"]		= "Selecionar a largura (em metros) que o ch�o da sua cozinha ter�.";
		hashWords [0] ["tip-preencher-area"]= "Preencher a �rea escolhida acima com ch�o.";
		hashWords [0] ["tip-colocar-paredes"] = "Colocar paredes ao redor do ch�o criado.";
		hashWords [0] ["tip-reiniciar"] 	  = "Retirar o ch�o anteriormente colocado.";
		
		//Montar cozinha - Tooltips
		hashWords [0] ["tip-modulos"] 	 = "Selecionar os m�dulos para a sua cozinha.";
		hashWords [0] ["tip-ilumina��o"] = "Ajustar a ilumina��o da cena.";
		hashWords [0] ["tip-extras"]	 = "Selecionar acess�rios extras para sua cozinha.";
		hashWords [0] ["tip-play"] 		 = "Habilitar modo primeira-pessoa.";
		hashWords [0] ["tip-screenshot"] = "Tirar uma foto da cozinha.";
		hashWords [0] ["tip-screenshot"] = "Fazer o download do relat�rio dos m�dulos da sua cozinha.";
		hashWords [0] ["tip-paredes"] 	 = "Ativar/Desativar paredes sempre vis�veis.";
		
		hashWords [0] ["tip-mover-vertical"]   = "Mover c�mera na dire��o vertical.";
		hashWords [0] ["tip-mover-horizontal"] = "Mover c�mera na dire��o horizontal.";
		hashWords [0] ["tip-rotacionar-horizontal"] = "Rotacionar c�mera na dire��o horizontal.";
		hashWords [0] ["tip-rotacionar-vertical"]   = "Rotacionar c�mera na dire��o vertical.";
		hashWords [0] ["tip-zoom-mais"]  = "Mover c�mera para frente.";
		hashWords [0] ["tip-zoom-menos"] = "Mover c�mera para tr�s.";
		
		hashWords [0] ["tip-modulo-rotacao"] = "Rotacionar m�dulo selecionado.";
		hashWords [0] ["tip-modulo-focar"] 	 = "Focar c�mera no m�dulo selecionado.";
		hashWords [0] ["tip-modulo-remover"] = "Remover m�dulo selecionado da cena.";
		
	}
	static private void InitializeEnglish(){
		//Escolha cozinha
		hashWords [1] ["Escolha sua linha favorita"] = "Choose your favorite kitchen";
		
		//Montar paredes
		hashWords [1] ["Medidas"] = "Measures";
		hashWords [1] ["Largura"] = "Width";
		hashWords [1] ["Comprimento"] = "Depth";
		
		hashWords [1] ["A��es"] = "Actions";
		hashWords [1] ["Preencher �rea"]  = "Fill area";
		hashWords [1] ["Colocar Paredes"] = "Build walls";
		hashWords [1] ["Reiniciar"] 	  = "Restart";
		
		//Monte sua cozinha
		hashWords [1] ["M�dulos"] = "Modules";
		hashWords [1] ["Ilumina��o"] = "Illumination";
		hashWords [1] ["Extras"] = "Extras";
		
		hashWords [1] ["C�digo"] = "Reference";
		hashWords [1] ["LXPXA"] = "WXDXH";
		hashWords [1] ["Descri��o"] = "Description";
		hashWords [1] ["Linha"] = "Line";
		hashWords [1] ["Categoria"] = "Category";
		
		//Montar paredes - Tooltips
		hashWords [1] ["tip-profundidade"] = "Choose the depth (in meters) that the floor of your kitchen will have.";
		hashWords [1] ["tip-largura"] 	   = "Choose the width (in meters) that the floor of your kitchen will have.";
		hashWords [1] ["tip-preencher-area"]  = "Fill the selected area above with floor.";
		hashWords [1] ["tip-colocar-paredes"] = "Build walls around the floor.";
		hashWords [1] ["tip-reiniciar"] 	  = "Removing the previously placed floor.";
		
		//Montar cozinha - Tooltips
		hashWords [1] ["tip-modulos"] 	 = "Choose the modules for your kitchen.";
		hashWords [1] ["tip-ilumina��o"] = "Set the lighting of the scene.";
		hashWords [1] ["tip-extras"] 	 = "Select additional accessories for your kitchen.";
		hashWords [1] ["tip-play"] 		 = "Enable first-person mode.";
		hashWords [1] ["tip-screenshot"] = "Take a picture of the kitchen.";
		hashWords [1] ["tip-screenshot"] = "Download the report of the modules in your kitchen.";
		hashWords [1] ["tip-paredes"] = "Enable/Disable always visible walls.";
		
		hashWords [1] ["tip-mover-vertical"]   = "Move camera on vertical direction.";
		hashWords [1] ["tip-mover-horizontal"] = "Move camera on horizontal direction.";
		hashWords [1] ["tip-rotacionar-horizontal"] = "Rotate camera on horizontal direction.";
		hashWords [1] ["tip-rotacionar-vertical"]	= "Rotate camera on vertical direction.";
		hashWords [1] ["tip-zoom-mais"]  = "Move camera forward.";
		hashWords [1] ["tip-zoom-menos"] = "Move camera backwards.";
		
		hashWords [1] ["tip-modulo-rotacao"] = "Rotate selected module.";
		hashWords [1] ["tip-modulo-focar"] 	 = "Focus camera on selected module.";
		hashWords [1] ["tip-modulo-remover"] = "Delete selected module.";
	}
	static private void InitializeSpanish ()
	{
		//Escolha cozinha
		hashWords [2] ["Escolha sua linha favorita"] = "Choose your favorite kitchen";
		
		//Montar paredes
		hashWords [2] ["Medidas"] = "Measures";
		hashWords [2] ["Largura"] = "Width";
		hashWords [2] ["Comprimento"] = "Depth";
		
		hashWords [2] ["A��es"] = "Actions";
		hashWords [2] ["Preencher �rea"]  = "Fill area";
		hashWords [2] ["Colocar Paredes"] = "Build walls";
		hashWords [2] ["Reiniciar"] 	  = "Restart";
		
		//Monte sua cozinha
		hashWords [2] ["M�dulos"] = "Modules";
		hashWords [2] ["Ilumina��o"] = "Illumination";
		hashWords [2] ["Extras"] = "Extras";
		
		hashWords [2] ["C�digo"] = "Reference";
		hashWords [2] ["LXPXA"] = "WXDXH";
		hashWords [2] ["Descri��o"] = "Description";
		hashWords [2] ["Linha"] = "Line";
		hashWords [2] ["Categoria"] = "Category";
		
		//Montar paredes - Tooltips
		hashWords [2] ["tip-profundidade"] = "Choose the depth (in meters) that the floor of your kitchen will have.";
		hashWords [2] ["tip-largura"] 	   = "Choose the width (in meters) that the floor of your kitchen will have.";
		hashWords [2] ["tip-preencher-area"]  = "Fill the selected area above with floor.";
		hashWords [2] ["tip-colocar-paredes"] = "Build walls around the floor.";
		hashWords [2] ["tip-reiniciar"] 	  = "Removing the previously placed floor.";
		
		//Montar cozinha - Tooltips
		hashWords [2] ["tip-modulos"] 	 = "Choose the modules for your kitchen.";
		hashWords [2] ["tip-ilumina��o"] = "Set the lighting of the scene.";
		hashWords [2] ["tip-extras"] 	 = "Select additional accessories for your kitchen.";
		hashWords [2] ["tip-play"] 		 = "Enable first-person mode.";
		hashWords [2] ["tip-screenshot"] = "Take a picture of the kitchen.";
		hashWords [2] ["tip-screenshot"] = "Download the report of the modules in your kitchen.";
		hashWords [2] ["tip-paredes"] = "Enable/Disable always visible walls.";
		
		hashWords [2] ["tip-mover-vertical"]   = "Move camera on vertical direction.";
		hashWords [2] ["tip-mover-horizontal"] = "Move camera on horizontal direction.";
		hashWords [2] ["tip-rotacionar-horizontal"] = "Rotate camera on horizontal direction.";
		hashWords [2] ["tip-rotacionar-vertical"]	= "Rotate camera on vertical direction.";
		hashWords [2] ["tip-zoom-mais"]  = "Move camera forward.";
		hashWords [2] ["tip-zoom-menos"] = "Move camera backwards.";
		
		hashWords [2] ["tip-modulo-rotacao"] = "Rotate selected module.";
		hashWords [2] ["tip-modulo-focar"] 	 = "Focus camera on selected module.";
		hashWords [2] ["tip-modulo-remover"] = "Delete selected module.";
	}
	#endregion

	#region get information of the i18n
	static public void ChangeLanguage(int selectedLanguageIndex){
		selectedLanguage = selectedLanguageIndex;		
	}

	static public string[] GetAvailableLanguages(){
		return availableLanguages;
	}
	static public string[] GetCurrentLanguageName(){
		return availableLanguages;
	}
	static public int GetCurrentLanguageIndex(){
		return selectedLanguage;
	}
	#endregion

	#region Translate!
	static public string t(string text){
		return (string)hashWords[selectedLanguage][text];
	}
	#endregion
}