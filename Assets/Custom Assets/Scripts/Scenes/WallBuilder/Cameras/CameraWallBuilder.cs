using UnityEngine;
using System.Collections;

public class CameraWallBuilder : MonoBehaviour {
	private Vector3		mov;
	private float 		speedCam = 5.0f;
	private float 		zoom;
	private int 		zoomGrau = 40;
	private Light		light;
	
	void Start () {
		mov = transform.position;
		zoom = camera.orthographicSize;
		light = GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		MovCamera ();
	}
	
	void MovCamera ()
	{
		mov += new Vector3 ((Input.GetAxis ("Horizontal") * Time.deltaTime) * speedCam, 0, (Input.GetAxis ("Vertical") * Time.deltaTime) * speedCam);
		mov.x = Mathf.Clamp(mov.x, 979.5f, 1020.5f);
		mov.z = Mathf.Clamp(mov.z, 984.5f, 1015.5f);		
		transform.position = mov;
		zoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomGrau * Mathf.Abs(zoom);
		zoom = Mathf.Clamp(zoom, 2.5f, 15f);
		camera.orthographicSize = zoom;
		light.range = zoom * 4;
		light.transform.position = new Vector3(light.transform.position.x, zoom - 2f, light.transform.position.z);
	}
}
