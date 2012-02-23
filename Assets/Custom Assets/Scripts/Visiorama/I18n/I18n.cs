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
	}

	static private void InitializePortuguese(){
		
		#region Introdução
		hashWords[0]["Bem-vindo à Cozinha Virtual"]	= "Bem-vindo à Cozinha Virtual";
		hashWords[0]["Nova Cozinha"]				= "Nova Cozinha";
		hashWords[0]["Modelo"]						= "Modelo";
		hashWords[0]["Tutorial"]					= "Tutorial";
		hashWords[0]["Catalogo"]					= "Catálogo";
		hashWords[0]["Construir cozinha"]			= "Construir cozinha";
		hashWords[0]["Cozinha padrão"]				= "Cozinha padrão";
		hashWords[0]["Escolha uma marca"]			= "Escolha uma marca";
		#endregion
		
		#region tutorial
		hashWords[0]["Construtor"] = "Construtor";
		hashWords[0]["Construtor-texto-1"] = "Defina a área de construção desejada inserindo os valores de largura e profundidade de sua cozinha.";
		hashWords[0]["Construtor-texto-2"] = "Clique nos ladrilhos para editar o formato da área.";
		hashWords[0]["Construtor-texto-3"] = "Cada ladrilho para editar o formato.";
		#endregion
		
		#region Escolha das paredes
		hashWords[0]["Colocacão de paredes"]= "Colocacão de paredes";
		hashWords[0]["Montar paredes"]		= "Montar paredes";
		hashWords[0]["Retângulo"]			= "Retângulo";
		hashWords[0]["Parede em L"]			= "Parede em \"L\"";
		hashWords[0]["Quadrado"]			= "Quadrado";
		#endregion
		
		#region Escolha da linha da cozinha
		hashWords[0]["Escolha sua cozinha"]	= "Escolha sua cozinha";
		#endregion
		
		#region Menu de descrição do móvel
		hashWords[0]["Lin"]	= "Lin";
		hashWords[0]["Ref"]	= "Ref";
		hashWords[0]["Cat"]	= "Cat";
		hashWords[0]["Alt"]	= "Alt";
		hashWords[0]["Lar"]	= "Lar";
		hashWords[0]["Pro"]	= "Pro";
		#endregion
		
		#region Popup exclusão
		hashWords[0]["Confirma exclusão?"]	= "Confirma exclusão?";
		#endregion
		
		#region tooltip
		hashWords[0]["tip-rotacao-objeto"]	= "Rotação\nClique aqui para rotacionar o objeto.";
		hashWords[0]["tip-focar-objeto"]	= "Focar\nClique aqui para focar o objeto com a câmera.";
		hashWords[0]["tip-excluir-objeto"]	= "Excluir\nClique aqui para excluir o objeto selecionado.";
		
		hashWords[0]["tip-construcao-paredes-preenche"]	= "Preenche área\nPreenche uma área com as medidas descritas à cima.";
		hashWords[0]["tip-construcao-paredes-coloca"]	= "Colocar paredes\nCria as paredes da cozinha e inicia o modo de edição da cozinha.";
		hashWords[0]["tip-construcao-paredes-reiniciar"]= "Colocar paredes\nLimpa área preenchida.";

		hashWords[0]["tip-menu-catalogo-accordion-textura"] = "Escolher a cor das paredes e tipo de piso.";
		hashWords[0]["tip-menu-catalogo-accordion-tampo"]	= "Troca a cor dos tampos.";
		hashWords[0]["tip-menu-catalogo-accordion-portas"]	= "Trocar o lado da porta";
		hashWords[0]["tip-menu-catalogo-accordion-cor-detalhe"]	= "Quando um móvel estiver selecionado, pode-se trocar a sua cor de detalhe.";
		hashWords[0]["tip-menu-catalogo-accordion-iluminacao"]	= "Mudar a iluminação do arquiteto.";
		hashWords[0]["tip-menu-catalogo-accordion-moveis"]		= "Colocar novos móveis no arquiteto.";
		#endregion
				
		#region GUI catálogo
		//Botões accordion
		hashWords[0]["menu-catalogo-accordion-textura"] = "Textura";	
		hashWords[0]["menu-catalogo-accordion-tampo"] 	= "Tampo";	
		hashWords[0]["menu-catalogo-accordion-portas"] 	= "Portas";	
		hashWords[0]["menu-catalogo-accordion-cor-detalhe"] = "Cor de Detalhe";	
		hashWords[0]["menu-catalogo-accordion-iluminacao"] 	= "Iluminação";
		hashWords[0]["menu-catalogo-accordion-moveis"] 		= "Móveis";
		
		//Troca de textura chão/parede
		hashWords[0]["Parede"] 					= "Parede";
		hashWords[0]["Piso"]   					= "Piso";
		hashWords[0]["Luz Principal"]			= "Luz Principal";
		hashWords[0]["Posicionamento Solar"]	= "Posicionamento Solar";
		#endregion
		
		#region GUI Editor Área
		hashWords[0]["preencher área"]			= "preencher área";
		hashWords[0]["colocar parede"]			= "colocar parede";
		hashWords[0]["reiniciar"]				= "reiniciar";
		hashWords[0]["Lar"]						= "Lar";
		hashWords[0]["Alt"]						= "Alt";
		#endregion
		
	}
	
	static private void InitializeEnglish(){
		
		#region Introdução
		hashWords[1]["Bem-vindo à Cozinha Virtual"]	= "Welcome To The Virtual Kitchen";
		hashWords[1]["Nova Cozinha"]				= "New Kitchen";
		hashWords[1]["Modelo"]						= "Template";
		hashWords[1]["Tutorial"]					= "Tutorial";
		hashWords[1]["Catalogo"]					= "Catalog";
		hashWords[1]["Construir cozinha"]			= "Building Kitchen";
		hashWords[1]["Cozinha padrão"]				= "Standart Kitchen";
		hashWords[1]["Escolha uma marca"]			= "Choice a brand";
		#endregion
		
		#region tutorial
		hashWords[1]["Construtor"] = "Builder";
		hashWords[1]["Construtor-texto-1"] = "Set the construction area by entering the desired values ​​for width and depth of your kitchen.";
		hashWords[1]["Construtor-texto-2"] = "Click on the tiles to edit the shape of the area.";
		hashWords[1]["Construtor-texto-3"] = "Each tile to edit the format.";
		#endregion
		
		#region Escolha das paredes
		hashWords[1]["Colocacão de paredes"]= "Building Walls";
		hashWords[1]["Montar paredes"]		= "Building ";
		hashWords[1]["Retângulo"]			= "Rectangle";
		hashWords[1]["Parede em L"]			= "Walls On \"L\"";
		hashWords[1]["Quadrado"]			= "Square";
		#endregion
		
		#region Escolha da linha da cozinha
		hashWords[1]["Escolha sua cozinha"]	= "Choose your kitchen";
		#endregion
		
		#region Menu de descrição do móvel
		hashWords[1]["Lin"]	= "Lin";
		hashWords[1]["Ref"]	= "Ref";
		hashWords[1]["Cat"]	= "Cat";
		hashWords[1]["Alt"]	= "Hei";
		hashWords[1]["Lar"]	= "Wid";
		hashWords[1]["Pro"]	= "Dep";
		#endregion
		
		#region Popup exclusão
		hashWords[1]["Confirma exclusão?"]	= "Do you want to delete it?";
		#endregion
		
		#region tooltip
		hashWords[1]["tip-rotacao-objeto"]	= "Rotation\nClick here to rotate the mobile.";
		hashWords[1]["tip-focar-objeto"]	= "Focus\nClick here to focus the selected mobile.";
		hashWords[1]["tip-excluir-objeto"]	= "Delete\nClick here to delete the selected mobile.";
		#endregion
		
		#region GUI catálogo
		//Troca de textura chão/parede
		hashWords[1]["Parede"] 					= "Wall";
		hashWords[1]["Piso"]   					= "Floor";
		hashWords[1]["Luz Principal"]			= "Main Light";
		hashWords[1]["Posicionamento Solar"]	= "Solar Positioning";
		#endregion
		
		#region GUI Editor Área
		hashWords[1]["preencher área"]			= "fill area";
		hashWords[1]["colocar parede"]			= "put wall";
		hashWords[1]["reiniciar"]				= "restart";
		hashWords[1]["Lar"]						= "Wid";
		hashWords[1]["Alt"]						= "Hei";
		#endregion
	}
	static private void InitializeSpanish(){
		#region Introdução
		hashWords[2]["Bem-vindo à Cozinha Virtual"]	= "Bien Venido a la Cocina Virtual";
		hashWords[2]["Nova Cozinha"]				= "Nueva Cocina";
		hashWords[2]["Modelo"]						= "Modelo";
		hashWords[2]["Tutorial"]					= "Tutorial";
		hashWords[2]["Catalogo"]					= "Catalogo";
		hashWords[2]["Construir cozinha"]			= "Construir cocina";
		hashWords[2]["Cozinha padrão"]				= "Cocina Stándar";
		hashWords[2]["Escolha uma marca"]			= "Elige una marca";
		#endregion
		
		#region tutorial
		hashWords[2]["Construtor"] = "Constructor";
		hashWords[2]["Construtor-texto-1"] = "Defina el área de construción deseada agregando los valores de anchura y profundidad de la cocina.";
		hashWords[2]["Construtor-texto-2"] = "Haz clic en los ladrillos para editar el formato del área.";
		hashWords[2]["Construtor-texto-3"] = "Cada ladrillo se equivale a un metro cuadrado(1m).";
		#endregion
		
		#region Escolha das paredes
		hashWords[2]["Colocacão de paredes"]= "Rellenar el área";
		hashWords[2]["Montar paredes"]		= "Poner pared";
		hashWords[2]["Retângulo"]			= "Rectángulo";
		hashWords[2]["Parede em L"]			= "Pared en \"L\"";
		hashWords[2]["Quadrado"]			= "Cuadrado";
		#endregion
		
		#region Escolha da linha da cozinha
		hashWords[2]["Escolha sua cozinha"]	= "Elige tu cocina";
		#endregion
		
		#region Menu de descrição do móvel
		hashWords[2]["Lin"]	= "Lin";
		hashWords[2]["Ref"]	= "Ref";
		hashWords[2]["Cat"]	= "Cat";
		hashWords[2]["Alt"]	= "Alt";
		hashWords[2]["Lar"]	= "Anc";
		hashWords[2]["Pro"]	= "Pro";
		#endregion
		
		#region Popup exclusão
		hashWords[2]["Confirma exclusão?"]	= "Lo quieres apagar?";
		#endregion
		
		#region tooltip
		hashWords[2]["tip-rotacao-objeto"]	= "Rotación\nClick aqui para rotacionar el móvil.";
		hashWords[2]["tip-focar-objeto"]	= "Enfoque\nClick aqui para enfocar el móvil selecionado.";
		hashWords[2]["tip-excluir-objeto"]	= "Apagar\nClick aqui para apagar el móvil selecionado.";
		#endregion
		
		#region GUI catálogo
		//Troca de textura chão/parede
		hashWords[2]["Parede"] 					= "Pared";
		hashWords[2]["Piso"]   					= "Suelo";
		hashWords[2]["Luz Principal"]			= "Luz Principal";
		hashWords[2]["Posicionamento Solar"]	= "Posicionamiento del Sol";
		#endregion
		
		#region GUI Editor Área
		hashWords[2]["preencher área"]			= "Rellenar el área";
		hashWords[2]["colocar parede"]			= "Poner en la pared";
		hashWords[2]["reiniciar"]				= "Reiniciar";
		hashWords[2]["Lar"]						= "Anc";
		hashWords[2]["Alt"]						= "Alt";
		#endregion
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
