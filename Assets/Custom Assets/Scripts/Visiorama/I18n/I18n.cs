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
		availableLanguages = new string[3]{"Português","English","Español"};
		hashWords = new Hashtable[3];
		hashWords[0] = new Hashtable();
		hashWords[1] = new Hashtable();
		hashWords[2] = new Hashtable();
		InitializePortuguese();
		InitializeEnglish();
		//InitializeSpanish();
		
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
		
		if (checkPhrasesNumberArray[0] != checkPhrasesNumberArray[1]/* ||
			checkPhrasesNumberArray[0] == checkPhrasesNumberArray[2]*/)
		{
			Debug.LogError ("Alguma língua no I18n está com mais frases que outras. Por favor verifique o arquivo.");
			Debug.Break ();
		}
		#endregion
	}
	static private void InitializePortuguese ()
	{
		//Escolha cozinha
		hashWords [0] ["Escolha sua linha favorita"] = "Escolha sua linha favorita";
		
		//Montar paredes
		hashWords [0] ["Medidas"] = "Medidas";
		hashWords [0] ["Largura"] 	  = "Largura";
		hashWords [0] ["Comprimento"] = "Comprimento";
		hashWords [0] ["Ações"] 	  = "Ações";
		
		hashWords [0] ["Preencher Área"]  = "Preencher Área";
		hashWords [0] ["Colocar Paredes"] = "Colocar Paredes";
		hashWords [0] ["Recomeçar"]		  = "Recomeçar";
		
		//Monte sua cozinha
		hashWords [0] ["Módulos"] 	 = "Módulos";
		hashWords [0] ["Iluminação"] = "Iluminação";
		hashWords [0] ["Extras"] 	 = "Extras";
		
		hashWords [0] ["Lin."] = "Lin.";
		hashWords [0] ["Cat."] = "Cat.";
		hashWords [0] ["Ref."] = "Ref.";
		hashWords [0] ["Lar."] = "Lar.";
		hashWords [0] ["Alt."] = "Alt.";
		hashWords [0] ["Pro."] = "Pro.";
		
		//Montar paredes - Tooltips
		hashWords [0] ["tip-profundidade"]	= "Selecionar a profundidade (em metros) que o chão da sua cozinha terá.";
		hashWords [0] ["tip-largura"]		= "Selecionar a largura (em metros) que o chão da sua cozinha terá.";
		hashWords [0] ["tip-preencher-area"]= "Preencher a área escolhida à cima com chão.";
		hashWords [0] ["tip-colocar-paredes"] = "Colocar paredes ao redor do chão criado.";
		hashWords [0] ["tip-reiniciar"] 	  = "Retirar o chão anteriormente colocado.";
		
		//Montar cozinha - Tooltips
		hashWords [0] ["tip-modulos"] 	 = "Selecionar os módulos para a sua cozinha.";
		hashWords [0] ["tip-iluminação"] = "Ajustar a iluminação da cena.";
		hashWords [0] ["tip-extras"]	 = "Selecionar acessórios extras para sua cozinha.";
		hashWords [0] ["tip-play"] 		 = "Habilitar modo primeira-pessoa.";
		hashWords [0] ["tip-screenshot"] = "Tirar uma foto da cozinha.";
		hashWords [0] ["tip-screenshot"] = "Fazer o download do relatório dos módulos da sua cozinha.";
		hashWords [0] ["tip-paredes"] 	 = "Ativar/Desativar paredes sempre visíveis.";
		
		hashWords [0] ["tip-mover-vertical"]   = "Mover câmera na direção vertical.";
		hashWords [0] ["tip-mover-horizontal"] = "Mover câmera na direção horizontal.";
		hashWords [0] ["tip-rotacionar-horizontal"] = "Rotacionar câmera na direção horizontal.";
		hashWords [0] ["tip-rotacionar-vertical"]   = "Rotacionar câmera na direção vertical.";
		hashWords [0] ["tip-zoom-mais"]  = "Mover câmera para frente.";
		hashWords [0] ["tip-zoom-menos"] = "Mover câmera para trás.";
		
		hashWords [0] ["tip-modulo-rotacao"] = "Rotacionar módulo selecionado.";
		hashWords [0] ["tip-modulo-focar"] 	 = "Focar câmera no módulo selecionado.";
		hashWords [0] ["tip-modulo-remover"] = "Remover módulo selecionado da cena.";
		
	}
	
	static private void InitializeEnglish(){
		//Escolha cozinha
		hashWords [1] ["Escolha sua linha favorita"] = "Escolha sua linha favorita";
		
		//Montar paredes
		hashWords [1] ["Medidas"] = "Medidas";
		hashWords [1] ["Largura"] = "Largura";
		hashWords [1] ["Comprimento"] = "Comprimento";
		hashWords [1] ["Ações"] = "Ações";
		
		hashWords [1] ["Preencher Área"] = "Preencher Área";
		hashWords [1] ["Colocar Paredes"] = "Colocar Paredes";
		hashWords [1] ["Recomeçar"] = "Recomeçar";
		
		//Monte sua cozinha
		hashWords [1] ["Módulos"] = "Módulos";
		hashWords [1] ["Iluminação"] = "Iluminação";
		hashWords [1] ["Extras"] = "Extras";
		
		hashWords [1] ["Lin."] = "Lin.";
		hashWords [1] ["Cat."] = "Cat.";
		hashWords [1] ["Ref."] = "Ref.";
		hashWords [1] ["Lar."] = "Lar.";
		hashWords [1] ["Alt."] = "Alt.";
		hashWords [1] ["Pro."] = "Pro.";
		
		//Montar paredes - Tooltips
		hashWords [1] ["tip-profundidade"] = "Selecionar a profundidade (em metros) que o chão da sua cozinha terá.";
		hashWords [1] ["tip-largura"] = "Selecionar a largura (em metros) que o chão da sua cozinha terá.";
		hashWords [1] ["tip-preencher-area"] = "Preencher a área escolhida à cima com chão.";
		hashWords [1] ["tip-colocar-paredes"] = "Colocar paredes ao redor do chão criado.";
		hashWords [1] ["tip-reiniciar"] = "Retirar o chão anteriormente colocado.";
		
		//Montar cozinha - Tooltips
		hashWords [1] ["tip-modulos"] = "Selecionar os módulos para a sua cozinha.";
		hashWords [1] ["tip-iluminação"] = "Ajustar a iluminação da cena.";
		hashWords [1] ["tip-extras"] = "Selecionar acessórios extras para sua cozinha.";
		hashWords [1] ["tip-play"] = "Habilitar modo primeira-pessoa.";
		hashWords [1] ["tip-screenshot"] = "Tirar uma foto da cozinha.";
		hashWords [1] ["tip-screenshot"] = "Fazer o download do relatório dos módulos da sua cozinha.";
		hashWords [1] ["tip-paredes"] = "Ativar/Desativar paredes sempre visíveis.";
		
		hashWords [1] ["tip-mover-vertical"] = "Mover câmera na direção vertical.";
		hashWords [1] ["tip-mover-horizontal"] = "Mover câmera na direção horizontal.";
		hashWords [1] ["tip-rotacionar-horizontal"] = "Rotacionar câmera na direção horizontal.";
		hashWords [1] ["tip-rotacionar-vertical"] = "Rotacionar câmera na direção vertical.";
		hashWords [1] ["tip-zoom-mais"] = "Mover câmera para frente.";
		hashWords [1] ["tip-zoom-menos"] = "Mover câmera para trás.";
		
		hashWords [1] ["tip-modulo-rotacao"] = "Rotacionar módulo selecionado.";
		hashWords [1] ["tip-modulo-focar"] = "Focar câmera no módulo selecionado.";
		hashWords [1] ["tip-modulo-remover"] = "Remover módulo selecionado da cena.";
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