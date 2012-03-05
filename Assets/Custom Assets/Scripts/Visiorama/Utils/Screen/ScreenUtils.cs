using UnityEngine;
using System.Collections;

public class ScreenUtils {

	private ScreenUtils(){}
	//Eles são floats para que não seja necessário convertem de int para float em cada chamada
	private static float screenWidth;
	private static float screenHeight;
	public static float RealWidth { get; private set; }
	public static float RealHeight { get; private set; }
	private static bool wasInitialized = false;
	
	static public void Initialize(int gameTabWidth, int gameTabHeight){
		
		if(wasInitialized)
			Debug.LogWarning("ScreenUtils já foi inicializada.");
		
		RealWidth  = gameTabWidth;
		RealHeight = gameTabHeight;
		
		wasInitialized = true;
	}
	
	static public Rect ScaledRect(Texture texture, float x, float y){
		return ScaledRect(x,y,texture.width,texture.height);
	}
	
	static public Rect ScaledRect(Rect rect){
	    return ScaledRect(rect.x, rect.y, rect.width, rect.height);
	}
		
	static public Rect ScaledRect(float x, float y, float width, float height){
		
		if(!wasInitialized)
			Debug.LogError("Esta classe não foi inicializada!");
		
		screenWidth  = (float)Screen.width;
		screenHeight = (float)Screen.height;
		
		if((int)x != 0)
	    	x = (x*screenWidth)/RealWidth;
		
		if((int)y != 0)
	    	y = (y*screenHeight)/RealHeight;
		
		if((int)width != 0)
	    	width  = (width*screenWidth)/RealWidth;
		
		if((int)height != 0)
	    	height = (height*screenHeight)/RealHeight;
	    
	    return new Rect(x,y,width,height);
	}
	
	static public Rect OldScaledRect(float x, float y, float width, float height) {
					
		float screenWidth = (float)(Screen.width);
		float screenHeight = (float)(Screen.height);
		
		x *= screenHeight;
		y *= screenWidth;
		width  *= screenWidth;
	    height *= screenHeight;
	    
	    return new Rect(x,y,width,height);
	}
	
	static public float ScaleWidth(float width) {
		screenWidth  = (float)Screen.width;
		return (width*screenWidth)/RealWidth;
	}
	
	static public float ScaleHeight(float height) {
		screenHeight = (float)Screen.height;
		return (height*screenHeight)/RealHeight;
	}
	
	static public Vector2 ScaledVector2(Vector2 position) {
		screenWidth  = (float)Screen.width;
		screenHeight = (float)Screen.height;
		return new Vector2((position.x*screenWidth)/RealWidth, (position.y*screenHeight)/RealHeight);
	}
	
	static public Vector2 ScaledVector2(float width, float height) {
		screenWidth  = (float)Screen.width;
		screenHeight = (float)Screen.height;
		return new Vector2((width*screenWidth)/RealWidth, (height*screenHeight)/RealHeight);
	}
	
	static public int ScaledInt(int number) {
		
		int scaledNumber = (int)(( ScaleHeight(number) + ScaleWidth(number) ) / 2) ;
		
		return (int)Mathf.Ceil(scaledNumber);
		
//		float scaledWidth = Screen.width - RealWidth;
//		float scaledHeigth = Screen.height - RealHeight;
//		
//		scaledWidth = (number * scaledWidth) / 100;
//		scaledHeigth = (number * scaledHeigth) / 100;
//		
//		float totalScaled = (scaledWidth + scaledHeigth) / number;
//		int intTotalScaled = totalScaled - (int)totalScaled > 0.5 ? (int)totalScaled + 1 : (int)totalScaled;
//		
//		Debug.LogError(number + intTotalScaled);
//		
//		return (number + intTotalScaled);
		
	}
	
	static public float ScaledFloat(float number) {
		
		float scaledNumber = (ScaleHeight(number) + ScaleWidth(number)) / 2;
		
		return Mathf.Ceil(scaledNumber);
	}
	
	static public Rect ScaledRectInFloat(Rect rect) {
		return new Rect(ScaledFloat(rect.x), ScaledFloat(rect.y), 
						ScaledFloat(rect.width), ScaledFloat(rect.height));
	}
	
	static public bool ScreenSizeChange () {
		if (screenWidth != Screen.width ||
			screenHeight != Screen.height) {
			Initialize(Screen.width, Screen.height);
			return true;
		}
		return false;
	}
}
