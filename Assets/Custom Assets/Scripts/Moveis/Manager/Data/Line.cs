using UnityEngine;
using System.Collections.Generic;

public class Line {
	
	#region array singleton
	static public List<Line> Lines { get; private set; }
	static public Line CurrentLine { get; private set; }
	static public bool WasInitialized { get; private set; }
	static public void Initialize(List<Line> lines)
	{
		Lines = lines;
		
		WasInitialized = true;
		
		ChooseLine(0);
	}
	
	public static Line ChooseLine(int index)
	{
		return CurrentLine = Lines[index];
	}

	public static Color CurrentDetailColor
	{
		get
		{
			return BrandColor.GetRealColor( CurrentLine.colors[CurrentLine.GlobalDetailColorIndex]);
		}
	}
	#endregion
	
	public int GlobalDetailColorIndex { get; set; }
	public int GlobalBaseColorIndex { get; set; }
	public BrandColorEnum[] colors { get; private set; }
	public Texture[] colorsImg { get; private set; }
	public List<Category> categories { get; private set; }
	public string Name { get; private set; }
	public string cTopTexture;
	public string[] topTextureNames;
	
	public Line(BrandColorEnum[] colors, List<Category> categories, string name, string[] topTextureNames)
	{
		this.colors    		= colors;
		this.colorsImg 		= colorsImg;
		this.categories 	= categories;
		this.Name 	   		= name;
		this.topTextureNames= topTextureNames;
		this.cTopTexture	= topTextureNames[0];
	}
}