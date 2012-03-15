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
	
	public static Line ChooseLine(uint index)
	{
		return CurrentLine = Lines[(int)index];
	}
	#endregion
	
	public uint GlobalDetailColorIndex { get; set; }
	public uint GlobalBaseColorIndex { get; set; }
	public BrandColorEnum[] colors { get; private set; }
	public Texture[] colorsImg { get; private set; }
	public List<Category> categories { get; private set; }
	public string Name { get; private set; }
	
	public Line(BrandColorEnum[] colors, List<Category> categories, string name)
	{
		this.colors    	= colors;
		this.colorsImg 	= colorsImg;
		this.categories = categories;
		this.Name 	   	= name;
	}
}