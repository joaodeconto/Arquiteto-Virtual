using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderBounds : MonoBehaviour {

	private float alpha;
	
	static Material lineMaterial;
	
	private GameObject vobj;
	
	private List<Vector3> boxCorners;
	
	private GameObject obj;
		
	public bool Display { get; set; }
	
	void Awake(){
		Display = false;
	}
	
	private GameObject hLabel;
	private GameObject wLabel;
	private GameObject dLabel;	
	
	static void  CreateLineMaterial (){
	    if( !lineMaterial ) {
	        lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
	            "SubShader { Pass { " +
	            "    Blend SrcAlpha OneMinusSrcAlpha " +
	            "    ZWrite Off Cull Off Fog { Mode Off } " +
	            "    BindChannels {" +
	            "      Bind \"vertex\", vertex Bind \"color\", color }" +
	            "} } }" );
	        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
	        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
	    }
	}
	
	void  OnPostRender (){
//		Debug.LogError("Chegou");
		
		if(!Display || obj == null)
			return;
		
		UpdateObj();
		
		if(boxCorners == null || 
		   boxCorners.Count == 0) 
			return;
	    
		CreateLineMaterial();
		
		lineMaterial.SetPass(0);
				
		GL.Color(Color.cyan);//new Color(0,1,0,0.5f));
		GL.Begin( GL.LINES );
			int j1;
			for (int j=0; j<4; j++) {
//				GL.Color(Color.blue);
				GL.Vertex3(boxCorners[j].x,boxCorners[j].y,boxCorners[j].z);//top lines
				j1 = (j+1)%4;
				GL.Vertex3(boxCorners[j1].x,boxCorners[j1].y,boxCorners[j1].z);
				j1 = j + 4;
//				GL.Color(Color.red);
				GL.Vertex3(boxCorners[j].x,boxCorners[j].y,boxCorners[j].z);//vertical lines
				GL.Vertex3(boxCorners[j1].x,boxCorners[j1].y,boxCorners[j1].z);
//				GL.Color(Color.yellow);
				GL.Vertex3(boxCorners[j1].x,boxCorners[j1].y,boxCorners[j1].z);//bottom rectangle
				j1 = 4 + (j+1)%4;
				GL.Vertex3(boxCorners[j1].x,boxCorners[j1].y,boxCorners[j1].z);
			}
		GL.End();
	}
	
	public void SetBox ( GameObject scr  ){
		obj = scr;
		UpdateObj();
	}
	
	public void UpdateObj(){
		alpha 		= obj.GetComponent<CalculateBounds>().Alpha;
		boxCorners	= obj.GetComponent<CalculateBounds>().Corners;
	}
}