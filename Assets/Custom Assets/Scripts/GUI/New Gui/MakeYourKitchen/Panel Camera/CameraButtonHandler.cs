using UnityEngine;
using System.Collections;

public class CameraButtonHandler : MonoBehaviour {
	
	public enum CameraButtonHandlerEnum
	{
		CameraMoveUp,
		CameraMoveDown,
		CameraMoveLeft,
		CameraMoveRight,
		CameraRotateUp,
		CameraRotateDown,
		CameraRotateLeft,
		CameraRotateRight,
		CameraPlay,
		CameraScreenshot,
		CameraReport,
		CameraHideShowWalls,
		CameraZoomMinus,
		CameraZoomPlus,	
	}
	
	public CameraButtonHandlerEnum cameraButtonHandler;
	public bool IsRepeatButton;
	
	private CameraController cameraController;
	
	#region NGUI monkey patch vars :P
	private float repeatInterval = 0.01f;
	private float mNextUpdate = 0f;
	private bool mIsPressed = false;
	private GameObject mainCamera;
	
	#endregion
		
	void Start()
	{
		cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
		mainCamera = GameObject.FindWithTag("MainCamera");
	}
 
	void OnPress (bool val)
	{
		mIsPressed = val;
	}
 
	void Update ()
	{
		if (IsRepeatButton && mIsPressed && mNextUpdate < Time.realtimeSinceStartup) 
		{
			mNextUpdate = Time.realtimeSinceStartup + repeatInterval;
			SendMessage ("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnClick()
	{
		if (!mainCamera.GetComponent<Camera3d>().CanMoveCamera) return;
		
		switch (cameraButtonHandler) {
		//para mover a câmera uso coordenadas x,y
			#region move
			case CameraButtonHandlerEnum.CameraMoveUp:
				cameraController.Move (0, 1);
				break;
			case CameraButtonHandlerEnum.CameraMoveDown:
				cameraController.Move (0, -1);
				break;
			case CameraButtonHandlerEnum.CameraMoveLeft:
				cameraController.Move (-1, 0);
				break;
			case CameraButtonHandlerEnum.CameraMoveRight:
				cameraController.Move (1, 0);
				break;
			#endregion
			#region rotate
			case CameraButtonHandlerEnum.CameraRotateUp:
				cameraController.Rotate (0, -1);
				break;
			case CameraButtonHandlerEnum.CameraRotateDown:
				cameraController.Rotate (0, 1);
				break;
			case CameraButtonHandlerEnum.CameraRotateLeft:
				cameraController.Rotate (-1, 0);
				break;
			case CameraButtonHandlerEnum.CameraRotateRight:
				cameraController.Rotate (1, 0);
				break;
			#endregion
			#region Zoom
			case CameraButtonHandlerEnum.CameraZoomMinus:
				cameraController.Zoom (-1);
				break;
			case CameraButtonHandlerEnum.CameraZoomPlus:
//				gameObject.AddComponent<Configuration>().SaveCurrentState("scene-template-" + string.Format ("{0:yyyy-MM-dd-HH-mm-ss}", System.DateTime.Now) + ".xml", false);
				cameraController.Zoom (1);
				break;
			#endregion
			#region Extras
			case CameraButtonHandlerEnum.CameraPlay:
				cameraController.Play();
				break;
			case CameraButtonHandlerEnum.CameraReport:
				cameraController.Report();
				break;
			case CameraButtonHandlerEnum.CameraScreenshot:
				cameraController.Screenshot();
				break;
			case CameraButtonHandlerEnum.CameraHideShowWalls:
				cameraController.ShowHideWalls();
				break;
			#endregion
		/*
			case CameraButtonHandlerEnum.CameraMoveUp:
				break;
			case CameraButtonHandlerEnum.CameraMoveUp:
				break;
			case CameraButtonHandlerEnum.CameraMoveUp:
				break;
			case CameraButtonHandlerEnum.CameraMoveUp:
				break;
			case CameraButtonHandlerEnum.CameraMoveUp:
				break;
			case CameraButtonHandlerEnum.CameraMoveUp:
				break;*/
		}
	}
}
