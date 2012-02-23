using UnityEngine;
using System.Collections.Generic;

public class Line {
	
	#region array singleton
	static public List<Line> Lines { get; private set; }
	static public Line CurrentLine { get; private set; }
	static public bool WasInitialized { get; private set; }
	static public void Initialize(List<Line> lines){
		
		Lines = lines;
		
		WasInitialized = true;
		
		ChooseLine(0);
		
//		Color[] standardColors = new Color[4]{new Color(0.6F,0.6F,0.0F),new Color(0.0F,1.0F,0.0F),new Color(1.0F,1.0F,1.0F),new Color(1.0F,1.0F,1.0F)};
//		Color[] maximaColors   = new Color[3]{new Color(0.5F,0.5F,0.5F),new Color(0.0F,0.0F,0.0F),new Color(1.0F,1.0F,1.0F)};
		
//		lines = new Hashtable();
		//lines.Add("Maxima", new Line(maximaColors));
		//lines.Add("Clean", 	new Line(standardColors));
		//lines.Add("Novita", new Line(standardColors));
		//lines.Add("America Slim",  new Line(standardColors));
		//lines.Add("Diamante Slim", new Line(standardColors));
		//lines.Add("Star", new Line(standardColors));
	}
	
	public static Line ChooseLine(uint index){
		return CurrentLine = Lines[(int)index];
	}
	#endregion
	
	public uint GlobalDetailColorIndex { get; set; }
	public uint GlobalBaseColorIndex { get; set; }
	public Color[] colors { get; private set; }
	public Texture[] colorsImg { get; private set; }
//	public List<FurnitureData> furniture { get; private set; }
//	public List<InformacoesMovel> furniture { get; private set; }
	public List<Category> categories { get; private set; }
	public string Name { get; private set; }
	
	public Line(Color[] colors, Texture[] colorsImg, List<Category> categories, string name){
		this.colors    	= colors;
		this.colorsImg 	= colorsImg;
		this.categories = categories;
		this.Name 	   	= name;
	}
}