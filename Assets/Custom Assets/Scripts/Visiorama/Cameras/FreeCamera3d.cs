using UnityEngine;
using System.Collections;
using Visiorama;

public class FreeCamera3d : MonoBehaviour
{
	public Camera ThisCamera {get; private set;}
	public float Step;
	public float StepTime;
	public bool CanMoveCamera {get; private set;}
	public bool ZoomFollowMousePosition;
	
	public FreeCamera3d Initialize (Camera camera, 
									float step, 
									float stepTime, 
									bool canMoveCamera, 
									bool zoomFollowMousePosition)
	{
		ThisCamera 				= camera;
		Step 					= step;
		StepTime				= stepTime;
		CanMoveCamera 			= canMoveCamera;
		ZoomFollowMousePosition	= zoomFollowMousePosition;
		return this;
	}

	// Update is called once per frame
	public void DoCamera () {
		if (CanMoveCamera)
		{
			MoveCamera();
			MouseMoveCamera();
		}
	}
	
	public void FreezeCamera ()
	{
		CanMoveCamera = false;
	}
	
	public void FreeCamera ()
	{
		CanMoveCamera = true;
	}

	void MoveCamera () {
		Vector3 direcao = 	(ThisCamera.transform.right * Input.GetAxis ("Horizontal") * Time.deltaTime * 5) + 
							(((ThisCamera.transform.up + ThisCamera.transform.forward) * Input.GetAxis ("Vertical") * Time.deltaTime * 5) / 2);
		direcao = new Vector3(direcao.x, 0, direcao.z);
		ThisCamera.transform.position += direcao;
		if (Input.GetKey (KeyCode.Q))
			ThisCamera.transform.eulerAngles -= new Vector3 (0, 1, 0);
		if (Input.GetKey (KeyCode.E))
			ThisCamera.transform.eulerAngles += new Vector3 (0, 1, 0);
	}
	
	void MouseMoveCamera () {
		if (Input.GetMouseButton(1)) {
			float x = Input.GetAxis("Mouse X") * 2;
			float y = Input.GetAxis("Mouse Y") * 2;
			
			ThisCamera.transform.localEulerAngles += new Vector3(-y, x, 0);
		}
		
		if (Input.GetMouseButton(2)) {
			float x = Input.GetAxis("Mouse X");
			float y = Input.GetAxis("Mouse Y");
			
			ThisCamera.transform.localPosition += ThisCamera.transform.TransformDirection(new Vector3(x, y, 0));
		}
		
		ThisCamera.transform.localPosition = new Vector3( Mathf.Clamp(ThisCamera.transform.localPosition.x, 955f, 1045f), 
					                                      Mathf.Clamp(ThisCamera.transform.localPosition.y, -10f, 10f), 
					                                      Mathf.Clamp(ThisCamera.transform.localPosition.z, 955f, 1045f));
		
		// Mouse wheel moving forward
		if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f && ThisCamera.transform.position.y > -10f)
		{
			CanMoveCamera = false;
			
			Vector3 positionToScroll;
			if (ZoomFollowMousePosition)
			{
				positionToScroll = Input.mousePosition;
			}
			else
			{
				positionToScroll = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
			}
			
			Ray rayMouse 		   = ThisCamera.ScreenPointToRay(positionToScroll);
			Vector3 cameraPosition = ThisCamera.transform.position + (rayMouse.direction.normalized * Step);
			
			iTween.MoveTo(ThisCamera.gameObject, iTween.Hash(iT.MoveTo.time, 		StepTime,
															 iT.MoveTo.position,	cameraPosition,
															 iT.MoveTo.easetype, 	iTween.EaseType.easeInOutSine,
															 iT.MoveTo.oncomplete,	"FreeCamera"));
		}
		
		// Mouse wheel moving backward
		else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f && ThisCamera.transform.position.y < 10f) {
			Vector3 positionToScroll;
			if (ZoomFollowMousePosition)
			{
				positionToScroll = Input.mousePosition;
			}
			else
			{
				positionToScroll = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
			}
			
			Ray rayMouse 		   = ThisCamera.ScreenPointToRay(positionToScroll);
			Vector3 cameraPosition = (ThisCamera.transform.position - (rayMouse.direction.normalized * Step));	
			iTween.MoveTo(ThisCamera.gameObject, iTween.Hash(iT.MoveTo.time, 		StepTime,
															 iT.MoveTo.position,	cameraPosition,
															 iT.MoveTo.easetype, 	iTween.EaseType.easeInOutSine,
															 iT.MoveTo.oncomplete,	"FreeCamera"));
		}
	}
}