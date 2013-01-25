using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TextureAtlasInfo
{
	public int anisoLevel;
	public bool compressTexturesInMemory;
	public FilterMode filterMode;
	public bool ignoreAlpha;
	public TextureWrapMode wrapMode;
	public List<ShaderProperties> shaderPropertiesToLookFor;
	
	public TextureAtlasInfo()
	{
		anisoLevel = 1;
		compressTexturesInMemory = true;
		filterMode = FilterMode.Trilinear;
		ignoreAlpha = true;
		wrapMode = TextureWrapMode.Clamp;	
	
		shaderPropertiesToLookFor = new List<ShaderProperties> ();
		
		shaderPropertiesToLookFor.Add(new ShaderProperties(false, "_MainTex")); 
		shaderPropertiesToLookFor.Add(new ShaderProperties(true, "_BumpMap")); 
		shaderPropertiesToLookFor.Add(new ShaderProperties(false, "_Cube"));
		shaderPropertiesToLookFor.Add(new ShaderProperties(false, "_DecalTex"));
		shaderPropertiesToLookFor.Add(new ShaderProperties(false, "_Detail"));
		shaderPropertiesToLookFor.Add(new ShaderProperties(false, "_ParallaxMap"));
	}
}
