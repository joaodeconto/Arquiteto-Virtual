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
			Debug.LogError ("Alguma língua no I18n está com mais frases que outras. Por favor verifique o arquivo.");
			Debug.Break ();
		}
		#endregion
	}
	
	static private void InitializePortuguese ()
	{
		//Escolha cozinha
		hashWords [0] ["Escolha sua cozinha favorita"] = "Escolha sua cozinha favorita";
		
		//Intro
		hashWords [0] ["Iniciar"] 	  = "Iniciar";
		hashWords [0] ["Medidas da sua cozinha"] 	  = "Medidas da sua cozinha";
		
		//Montar paredes
		hashWords [0] ["Medidas"] 	  = "Medidas";
		hashWords [0] ["Profundidade"]= "Profundidade";
		hashWords [0] ["Comprimento"] = "Comprimento";
		
				
		//Color Picker
		hashWords [0] ["Cor"] = "Cor";
		hashWords [0] ["Teto"] = "Teto";
		hashWords [0] ["Parede"] = "Parede";
		hashWords [0] ["Descolorir"] = "Descolorir";
		
		//Monte sua cozinha
		hashWords [0] ["Módulos"] 	 = "Módulos";
		hashWords [0] ["Iluminação"] = "Iluminação";
		hashWords [0] ["Extras"] 	 = "Extras";
		hashWords [0] ["Opções"] = "Opções";
		hashWords [0] ["Piso"] = "Piso";
		
		hashWords [0] ["Lustre"] = "Lustre";
		hashWords [0] ["Spot"] = "Spot";
		hashWords [0] ["Cores"] = "Cores";
		hashWords [0] ["Portas"] = "Portas";
		hashWords [0] ["Tampos"] = "Tampos";
		hashWords [0] ["Pressione ESPAÇO para voltar"] = "Pressione ESPAÇO para voltar";
		
		hashWords [0] ["Código"] = "Código";
		hashWords [0] ["LXPXA"] = "LXPXA";
		hashWords [0] ["Descrição"] = "Descrição";
		hashWords [0] ["Linha"] = "Linha";
		hashWords [0] ["Categoria"] = "Categoria";
		
		//Montar paredes - Tooltips
		hashWords [0] ["tip-profundidade"]	= "Selecionar a profundidade (em metros)";
		hashWords [0] ["tip-largura"]		= "Selecionar a largura (em metros)";
		hashWords [0] ["tip-colocar-paredes"] = "Após colocar as medidas pressione iniciar.";
		
		
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

		//Cores
		hashWords [0] ["black"] = "Preto";
		hashWords [0] ["gray"] 	= "Titanium";
		hashWords [0] ["green"] = "Verde";
		hashWords [0] ["violet"]= "Vinho";
		hashWords [0] ["white"] = "Branco";

		//Tipos de tops
		hashWords [0] ["cooktop"] = "Cooktop";
		hashWords [0] ["com pia"] = "Pia";
		hashWords [0] ["com tampo"] = "Com Tampo";
		hashWords [0] ["sem tampo"] = "Sem Tampo";

		//Texturas de tampos
		hashWords [0] ["labrador"] 		= "Labrador";
		hashWords [0] ["marmore cinza"] = "Mármore Cinza";
		hashWords [0] ["marmore preto"] = "Mármore Preto";
		hashWords [0] ["onix bianco"]	= "Onix Bianco";

	}
	static private void InitializeEnglish(){
		//Escolha cozinha
		hashWords [1] ["Escolha sua cozinha favorita"] = "Choose your favorite kitchen";
		
		//Intro
		hashWords [1] ["Iniciar"] 	  = "Start";
		hashWords [1] ["Medidas da sua cozinha"] 	  = "Kitchen Measures";
		
		//Montar paredes
		hashWords [1] ["Medidas"] = "Measures";
		hashWords [1] ["Profundidade"]= "Depth";
		hashWords [1] ["Comprimento"] = "Width";
		
				
		//Color Picker
		hashWords [1] ["Cor"] = "Color";
		hashWords [1] ["Teto"] = "Roof";
		hashWords [1] ["Parede"] = "Wall";
		hashWords [1] ["Descolorir"] = "Discolor";
		
		//Monte sua cozinha
		hashWords [1] ["Módulos"] = "Modules";
		hashWords [1] ["Iluminação"] = "Illumination";
		hashWords [1] ["Extras"] = "Extras";
		hashWords [1] ["Opções"] = "Options";
		hashWords [1] ["Piso"] = "Floor";
		
		hashWords [1] ["Lustre"] = "Chandelier";
		hashWords [1] ["Spot"] = "Spot";
		hashWords [1] ["Cores"] = "Colors";
		hashWords [1] ["Portas"] = "Doors";
		hashWords [1] ["Tampos"] = "Tos";
		hashWords [1] ["Pressione ESPAÇO para voltar"] = "Press SPACE to return";
		
		hashWords [1] ["Código"] = "Reference";
		hashWords [1] ["LXPXA"] = " WXDXH";
		hashWords [1] ["Descrição"] = "Description";
		hashWords [1] ["Linha"] = "Line";
		hashWords [1] ["Categoria"] = "Category";
		
		//Montar paredes - Tooltips
		hashWords [1] ["tip-profundidade"] = "Choose the depth (in meters) that the floor of your kitchen will have.";
		hashWords [1] ["tip-largura"] 	   = "Choose the width (in meters) that the floor of your kitchen will have.";
		hashWords [1] ["tip-colocar-paredes"] = "Build walls around the floor.";
				
		//Montar cozinha - Tooltips
		hashWords [1] ["tip-modulos"] 	 = "Choose the modules for your kitchen.";
		hashWords [1] ["tip-iluminação"] = "Set the lighting of the scene.";
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

		hashWords [1] ["black"]	= "Black";
		hashWords [1] ["gray"] 	= "Titanium";
		hashWords [1] ["green"] = "Green";
		hashWords [1] ["violet"]= "Wine";
		hashWords [1] ["white"] = "White";

		//Tipos de tops
		hashWords [1] ["cooktop"] = "Cooktop";
		hashWords [1] ["com pia"] = "Sink";
		hashWords [1] ["com tampo"] = "With top";
		hashWords [1] ["sem tampo"] = "Without top";

		//Texturas de tampos
		hashWords [1] ["labrador"] 		= "Labrador";
		hashWords [1] ["marmore cinza"] = "Gray Marble";
		hashWords [1] ["marmore preto"] = "Black Marble";
		hashWords [1] ["onix bianco"] 	= "Onix Bianco";
	}
	static private void InitializeSpanish ()
	{
		//Escolha cozinha
		hashWords [2] ["Escolha sua cozinha favorita"] = "Escoja su cocina favorita";
		
		//Intro
		hashWords [2] ["Iniciar"] 	  = "Iniciar";
		hashWords [2] ["Medidas da sua cozinha"] 	  = "Medidas de su cocina";
		
		//Montar paredes
		hashWords [2] ["Medidas"] = "Medidas";
		hashWords [2] ["Comprimento"] = "Anchura";
		hashWords [2] ["Profundidade"] = "Longitud";
		
				
		//Color Picker
		hashWords [2] ["Cor"] = "Cor";
		hashWords [2] ["Teto"] = "Techo";
		hashWords [2] ["Parede"] = "Pared";
		hashWords [2] ["Descolorir"] = "Descolorar";
		
		//Monte sua cozinha
		hashWords [2] ["Módulos"] = "Módulos";
		hashWords [2] ["Iluminação"] = "Iluminación";
		hashWords [2] ["Extras"] = "Extras";
		hashWords [2] ["Opções"] = "Opciones";
		hashWords [2] ["Piso"] = "Piso";
		
		hashWords [2] ["Lustre"] = "Lámpara";
		hashWords [2] ["Spot"] = "Spot";
		hashWords [2] ["Cores"] = "Colores";
		hashWords [2] ["Portas"] = "Puertas";
		hashWords [2] ["Tampos"] = "Tapas";
		hashWords [2] ["Pressione ESPAÇO para voltar"] = "Oprima SPACE para volver";
		
		hashWords [2] ["Código"] = "Código";
		hashWords [2] ["LXPXA"] = "AXLXA";
		hashWords [2] ["Descrição"] = "Descripción";
		hashWords [2] ["Linha"] = "Línea";
		hashWords [2] ["Categoria"] = "Categoría";
		
		//Montar paredes - Tooltips
		hashWords [2] ["tip-profundidade"] = "Seleccione la profundidad (en metros)";
		hashWords [2] ["tip-largura"] 	   = "Seleccione la largura (en metros)";
		hashWords [2] ["tip-colocar-paredes"] = "Después de poner las medidas pulse Iniciar.";
				
		//Montar cozinha - Tooltips
		hashWords [2] ["tip-modulos"] 	 = "Elija los módulos para su cocina.";
		hashWords [2] ["tip-iluminação"] = "Ajuste la iluminación de la escena.";
		hashWords [2] ["tip-extras"] 	 = "Seleccione los accesorios adicionales para su cocina.";
		hashWords [2] ["tip-play"] 		 = "Habilitar modo de primera persona.";
		hashWords [2] ["tip-screenshot"] = "Tome una fotografía de la cocina.";
		hashWords [2] ["tip-screenshot"] = "Descargue el informe de los módulos en su cocina.";
		hashWords [2] ["tip-paredes"]    = "Activar / Desactivar paredes siempre visibles.";
		
		hashWords [2] ["tip-mover-vertical"]   = "Mover la cámara en dirección vertical.";
		hashWords [2] ["tip-mover-horizontal"] = "Mover la cámara en dirección horizontal.";
		hashWords [2] ["tip-rotacionar-horizontal"] = "Rotar la cámara en dirección horizontal.";
		hashWords [2] ["tip-rotacionar-vertical"]	= "Rotar la cámara en dirección vertical.";
		hashWords [2] ["tip-zoom-mais"]  = "Mover cámara hacia delante.";
		hashWords [2] ["tip-zoom-menos"] = "Mueve la cámara hacia atrás.";
		
		hashWords [2] ["tip-modulo-rotacao"] = "Gira módulo seleccionado.";
		hashWords [2] ["tip-modulo-focar"] 	 = "Enfoque en el módulo seleccionado.";
		hashWords [2] ["tip-modulo-remover"] = "Eliminar módulo seleccionado.";

		hashWords [2] ["black"]	= "Negro";
		hashWords [2] ["gray"] 	= "Titanium";
		hashWords [2] ["green"] = "Verde";
		hashWords [2] ["violet"]= "Vino";
		hashWords [2] ["white"] = "Blanco";

		//Tipos de tops
		hashWords [2] ["cooktop"] = "Cooktop";
		hashWords [2] ["com pia"] = "Lavabo";
		hashWords [2] ["com tampo"] = "Tapa";
		hashWords [2] ["sem tampo"] = "sin Tapa";

		//Texturas de tampos
		hashWords [2] ["labrador"] 		= "Labrador";
		hashWords [2] ["marmore cinza"] = "Mármol Gris";
		hashWords [2] ["marmore preto"] = "Mármol Negro";
		hashWords [2] ["onix bianco"] 	= "Onix Bianco";
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
