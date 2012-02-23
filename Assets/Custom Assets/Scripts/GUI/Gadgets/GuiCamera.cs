using UnityEngine;

using System;
using System.Collections;

public class GuiCamera : MonoBehaviour, GuiBase {
	
	#region Gui
	public GUIStyle[] moveStyles;
	public GUIStyle[] rotationStyles;
	public GUIStyle[] zoomStyles;
	public GUIStyle[] extrasStyles;
	public GUIStyle[] lockStyles;
	
	private Rect[] btnsMove;
	private Rect[] btnsRotation;
	private Rect[] btnsZoom;
	private Rect[] btnsExtras;
	private Rect[] btnsLock;
	
	public Texture BgTexture;
	#endregion
			 
	#region cam variables
	private Camera cameraMain;
	internal float STEP;
	internal float ANGLE;
	internal float maxHeight;
	internal float minHeight;
	internal float playerHeight;
	#endregion
	
	private Rect wnd;
	internal  Rect wndOpenMenu;
	
	#region Gui Movement
	private float lerpStep;
	private float lerpTime;
	private float cLerp;
	private int minLeftPosition;
	private int maxLeftPosition;
	private int leftBorderPosition;
	private bool isLocked;
	#endregion
	
	public GameObject fpsCam;
	
	#region Unity methods
	void Awake(){
				
		cameraMain = transform.parent.GetComponent<Camera>();
			
		STEP  = 10.0F;
		ANGLE = 2F;
		maxHeight = 20F;
		minHeight = 1F;
		playerHeight = 1F;
		
		lerpStep 	= 0.1F;
		lerpTime 	= 3.0F;
		cLerp 		= 0.1F;
		
		
		wnd = ScreenUtils.ScaledRect(1024f -  11f - GuiScript.BORDER_MARGIN, 
	                                 640f  - 380f - GuiScript.BORDER_MARGIN, 
	                                 128,
		                             380);
		                             
		wndOpenMenu   = new Rect(wnd);
		wndOpenMenu.x = ScreenUtils.ScaleWidth(11f + GuiScript.BORDER_MARGIN);
		
		minLeftPosition    = (int)ScreenUtils.ScaleWidth(1024f - 128f);//Largura da tela - Largura da imagem
		maxLeftPosition    = (int)ScreenUtils.ScaleWidth(1024f - 11f - GuiScript.BORDER_MARGIN);
		leftBorderPosition = (int)ScreenUtils.ScaleWidth(1024f - 11f - GuiScript.BORDER_MARGIN);
		
		btnsZoom = new Rect[2];
		btnsZoom[0] = ScreenUtils.ScaledRect(16, 17, 32, 32);
		btnsZoom[1] = ScreenUtils.ScaledRect(80, 17, 32, 32);
		
		btnsMove = new Rect[4];
		btnsMove[0] = ScreenUtils.ScaledRect( 50, 50, 32, 32);
		btnsMove[1] = ScreenUtils.ScaledRect( 50, 115, 32, 32);
		btnsMove[2] = ScreenUtils.ScaledRect( 16, 82, 32, 32);
		btnsMove[3] = ScreenUtils.ScaledRect( 80, 82, 32, 32);
		
		btnsRotation = new Rect[4];
		btnsRotation[0] = ScreenUtils.ScaledRect( 50, 158, 32, 32);
		btnsRotation[1] = ScreenUtils.ScaledRect( 50, 223, 32, 32);
		btnsRotation[2] = ScreenUtils.ScaledRect( 16, 190, 32, 32);
		btnsRotation[3] = ScreenUtils.ScaledRect( 80, 190, 32, 32);
		
		btnsExtras = new Rect[4];
		btnsExtras[0] = ScreenUtils.ScaledRect(16, 284, 32, 32);
		btnsExtras[1] = ScreenUtils.ScaledRect(30, 320, 32, 32);
		btnsExtras[2] = ScreenUtils.ScaledRect(68, 320, 32, 32);
		btnsExtras[3] = ScreenUtils.ScaledRect(80, 284, 32, 32);
		
		btnsLock = new Rect[2];
		btnsLock[0] = ScreenUtils.ScaledRect(57, 353, 16, 16);
		btnsLock[1] = ScreenUtils.ScaledRect(57, 353, 16, 16);
	}
	 
	void Update(){
	
		if(isLocked)
			return;
			
		//se está fora da wnd
		if(!MouseUtils.MouseClickedInArea(wnd)){
			if(wnd.x != maxLeftPosition){
				CancelInvoke("Showing");
				Hide();
			}
		}
		if(!MouseUtils.MouseClickedInArea(wndOpenMenu)){
//			Debug.LogError("Chegou");
			CancelInvoke("Hiding");
			Show();
		}
	}
	#endregion
	
	#region animation
	public void Hide(){
		InvokeRepeating("Hiding",0.1F,lerpStep);
	}
	
	private void Hiding(){
		cLerp -= 0.1F / (lerpTime);
		
		if(cLerp < 0F){
			CancelInvoke("Hiding");
		}
		
		leftBorderPosition = (int)(Mathf.Lerp(maxLeftPosition, minLeftPosition ,cLerp));
	}
	
	private void Show(){
		InvokeRepeating("Showing",0.1F,lerpStep);
	}
	
	private void Showing(){
		cLerp += 1F / (lerpTime);
		
		if(cLerp > 1F){
			CancelInvoke("Showing");
		}
		
		leftBorderPosition = (int)(Mathf.Lerp(maxLeftPosition, minLeftPosition,cLerp));
		
	}
	#endregion
		
	#region camera movements
	void DrawZoomControl(){
		if(GUI.RepeatButton(btnsZoom[0],"",zoomStyles[0])){				
			Ray rayMouse = cameraMain.ScreenPointToRay(new Vector3(Screen.width / 2,Screen.height / 2,0));
			if (Physics.Raycast(rayMouse)) {
				Vector3 cameraPosition = (rayMouse.origin - transform.position) * Camera3d.SpeedZoom;
				cameraMain.transform.localPosition += cameraPosition * Time.deltaTime * Camera3d.StepZoom / 2;
			}
		}
		
		//Move backward button
		if(GUI.RepeatButton(btnsZoom[1],"",zoomStyles[1])){				
			Ray rayMouse = cameraMain.ScreenPointToRay(new Vector3(Screen.width / 2,Screen.height / 2,0));
			if (Physics.Raycast(rayMouse)) {
				Vector3 cameraPosition = (rayMouse.origin - transform.position) * Camera3d.SpeedZoom;
				cameraMain.transform.localPosition -= cameraPosition * Time.deltaTime * Camera3d.StepZoom / 2;
			}
		}
	}
	void DrawMoveControl(){
	
		Vector3 direcao = Vector3.zero;
		
		//Move forward button
		if(GUI.RepeatButton(btnsMove[0],"",moveStyles[0])){
			direcao = transform.up * Time.deltaTime * STEP;
		}
		
		//Move backward button
		if(GUI.RepeatButton(btnsMove[1],"",moveStyles[1])){
			direcao = transform.up * Time.deltaTime * - STEP;
		}
		
		//Move to left button
		if(GUI.RepeatButton(btnsMove[2],"",moveStyles[2])){
			direcao = transform.right * Time.deltaTime * - STEP;
		}
		
		//Move to right button
		if(GUI.RepeatButton(btnsMove[3],"",moveStyles[3])){
			direcao = transform.right * Time.deltaTime * STEP;
		}
		
		cameraMain.transform.position += direcao;
		
		//Setando a altura(y) do personagem sempre a mesma
//		if(transform.position.y != playerHeight )
//			transform.position -= new Vector3(0,direcao.y,0);
		
//		if(transform.position.y < minHeight || transform.position.y > maxHeight)
//			transform.position -= new Vector3(0,direcao.y,0);
		
	}
	void DrawLookControl(){
			
		//Look up button
		if(GUI.RepeatButton(btnsRotation[0],"",rotationStyles[0])){
			cameraMain.transform.RotateAroundLocal(transform.right, - ANGLE * Time.deltaTime);
		}
		
		//Look down button
		if(GUI.RepeatButton(btnsRotation[1],"",rotationStyles[1])){
			cameraMain.transform.RotateAroundLocal(transform.right, ANGLE * Time.deltaTime);
		}
		
		//Look left button
		if(GUI.RepeatButton(btnsRotation[2],"",rotationStyles[2])){
			cameraMain.transform.RotateAround(Vector3.up, - ANGLE * Time.deltaTime);
		}
		
		//Look right button
		if(GUI.RepeatButton(btnsRotation[3],"",rotationStyles[3])){
			cameraMain.transform.RotateAround(Vector3.up, ANGLE * Time.deltaTime);
		}
	}
	
	void DrawExtrasControl(){
		
		//Play btn
		if(GUI.Button(btnsExtras[0],"",extrasStyles[0])){
			SnapBehaviour.DeactivateAll();
			cameraMain.gameObject.SetActiveRecursively(false); 
			fpsCam.SetActiveRecursively(true);
			foreach (Transform child in GameObject.Find("RotacaoCubo").transform) {
				child.gameObject.SetActiveRecursively(false);
			}
		}
		//Screenshot btn
		if(GUI.Button(btnsExtras[1],"",extrasStyles[1])){
			
//			Debug.LogError("Screenshot");
			
			//TODO colocar isso no lugar certo
			//GameObject.Find("cfg").GetComponent<Configuration>().SaveCurrentState("lolol",true);
			//GameObject.Find("cfg").GetComponent<Configuration>().LoadState("teste/teste.xml",false);
			//GameObject.Find("cfg").GetComponent<Configuration>().RunPreset(0);
			StartCoroutine("SendScreenshotToForm","http://www.visiorama360.com.br/Telasul/uploadScreenshot.php");
		}
		//Report btn
		if(GUI.Button(btnsExtras[2],"",extrasStyles[2])){
			Debug.LogError("Report");
		}
		//Disable walls btn
		if(GUI.Button(btnsExtras[3],"",extrasStyles[3])){
			cameraMain.GetComponent<Camera3d>().AreWallsAlwaysVisible = !cameraMain.GetComponent<Camera3d>().AreWallsAlwaysVisible;
		}
		
		if(isLocked){
			if(GUI.Button(btnsLock[0],"",lockStyles[0])){
				isLocked = false;
			}
		} else {
			if(GUI.Button(btnsLock[1],"",lockStyles[1])){
				isLocked = true;
			}
		}
	}
	#endregion
		
	private IEnumerator SendScreenshotToForm(string screenShotURL){
	
		GuiScript.ShowGUI = false;
		
		yield return new WaitForSeconds(0.1f);
		yield return new WaitForEndOfFrame();
	    
	    // Create a texture the size of the screen, RGB24 format
	    int width = Screen.width;
	    int height = Screen.height;
	    Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
	    
	    // Read screen contents into the texture
	    tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
	    tex.Apply();
	    
		GuiScript.ShowGUI = true;
	
	    // Encode texture into PNG
	    byte[] bytes = tex.EncodeToPNG();
	    Destroy( tex );
	
	    // Create a Web Form
	    WWWForm form = new WWWForm();
//	    Debug.LogError( String.Format("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png");
	    
	    string filename = String.Format("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png";
	    
	    form.AddBinaryData("file", bytes, filename , "multipart/form-data");
		
	    WWW www = new WWW(screenShotURL, form);
	
	    yield return www;
	    
	    if (www.error != null)
	        print(www.error);  
	    else {
	        print("Finished Uploading Screenshot"); 
	        Application.ExternalCall("tryToDownload",filename);
	    }
	    

	}
	
	#region GuiBase implementation
	public void Draw(){
		
		wnd.x 		  = leftBorderPosition;
		wndOpenMenu.x = leftBorderPosition;
		
		GUI.DrawTexture(wnd,BgTexture);
		GUILayout.BeginArea(wnd);
			DrawZoomControl();
			DrawMoveControl();
			DrawLookControl();
			DrawExtrasControl();
		GUILayout.EndArea();
	}
	
	public Rect[] GetWindows (){
		return new Rect[1]{wnd};
	}
	#endregion
}