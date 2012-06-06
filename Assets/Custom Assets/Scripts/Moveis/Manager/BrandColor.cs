using UnityEngine;
using System.Collections;
	
public enum BrandColorEnum
{
	BLACK,
	GRAY,
	GREEN,
	VIOLET,
	WHITE,
}

public class BrandColor : MonoBehaviour {
	
	public BrandColorEnum[] colors;

	public static BrandColorEnum GetEnumColor (Color color)
	{
		if( color == new Color (0, 0, 0, 1))
			return BrandColorEnum.BLACK;
		if( color == new Color (0.64f, 0.64f, 0.64f, 1))
			return BrandColorEnum.GRAY;
		if( color == new Color (0, 0.65f, 0, 1))
			return BrandColorEnum.GREEN;
		if( color == new Color (0.36f, 0.11f, 0.29f, 1))
			return BrandColorEnum.VIOLET;
		
		return BrandColorEnum.WHITE;
	}

	public static Color GetRealColor (BrandColorEnum color)
	{
		switch (color) {
			case BrandColorEnum.BLACK:
				return new Color (0, 0, 0, 1);
				break;
			case BrandColorEnum.GRAY:
				return new Color (0.64f, 0.64f, 0.64f, 1);
				break;
			case BrandColorEnum.GREEN:
				return new Color (0, 0.65f, 0, 1);
				break;
			case BrandColorEnum.VIOLET:
				return new Color (0.36f, 0.11f, 0.29f, 1);
				break;
			case BrandColorEnum.WHITE:
				return new Color (1f, 1f, 1f, 1);
				break;
			default:
				return new Color (1f, 1f, 1f, 1);
				break;
		}
	}

	public static string GetColorName (BrandColorEnum color)
	{
		switch (color) {
			case BrandColorEnum.BLACK:
				return "Black";
				break;
			case BrandColorEnum.GRAY:
				return "Gray";
				break;
			case BrandColorEnum.GREEN:
				return "Green";
				break;
			case BrandColorEnum.VIOLET:
				return "Violet";
				break;
			case BrandColorEnum.WHITE:
				return "White";
				break;
			default:
				return "Black";
				break;
		}
	}
}
