using UnityEngine;
using System.Collections;

public class Camera3d : MonoBehaviour
{
	public Material paredeMaterial;
	public Material paredeTransparent;
	
	public WallsParents paredesParents;
	public GameObject tetoParent, chaoParent;
	
	static public Camera sCamera;
	static public float SpeedZoom { get; private set; }
	static public float StepZoom  { get; private set; }
	
	public bool AreWallsAlwaysVisible {
		get { return areWallsVisible; }
		set { 
				if(value == true){
					foreach (Transform parede in paredesParents.GetWalls()) {
						parede.renderer.material = paredeMaterial;
					}
				}

				//tetoParent.renderer.enabled = value;
				
				areWallsVisible = value;
			}
	}
	private bool areWallsVisible;
	
	private GuiCatalogo guiCatalogo;
	private GuiCamera guiCamera;
	private GuiDescription guiDescription;
	
	void Awake () {
	
		sCamera = GetComponent<Camera>();
		SpeedZoom = 1.5f;
		StepZoom  = 8f;
		
		paredesParents.colorWallLeft = Color.white;
		paredesParents.colorWallRight = Color.white;
		paredesParents.colorWallBack = Color.white;
		paredesParents.colorWallFront = Color.white;

		AreWallsAlwaysVisible = false;
		
		GameObject.Find("RotacaoCubo").GetComponentInChildren<Camera>().enabled = true;
		
		guiCatalogo = GetComponentInChildren<GuiCatalogo>();
		guiCamera = GetComponentInChildren<GuiCamera>();
		guiDescription = GetComponentInChildren<GuiDescription>();
	}

	// Update is called once per frame
	void Update () {
		MoveCamera();
		MouseMoveCamera();
		RenderWalls();
		RenderCeiling();
	}
		
	void OnGUI(){
		//GUI.Label(new Rect(200,200,200,200),(transform.localEulerAngles).ToString());
	}

	void RenderCeiling () {
		//if (AreWallsAlwaysVisible){
				tetoParent.collider.enabled = 
					tetoParent.renderer.enabled = this.transform.position.y < tetoParent.transform.position.y + 2.45f;
				chaoParent.collider.enabled = 
					chaoParent.renderer.enabled = this.transform.position.y > chaoParent.transform.position.y;
		//}
	}
	
	void RenderWalls() {
	
		if (!AreWallsAlwaysVisible) {
			//paredes da esquerda
			if (transform.localEulerAngles.y > 20 && transform.localEulerAngles.y < 160) {
				if (paredesParents.parentWallLeft.renderer.material.name != paredeTransparent.name+" (Instance)") {
					paredesParents.parentWallLeft.renderer.material = paredeTransparent;
					paredesParents.parentWallLeft.renderer.material.color = paredesParents.colorWallLeft;
					paredesParents.parentWallLeft.collider.enabled = false;
				}
				paredesParents.colorWallLeft = paredesParents.parentWallLeft.renderer.material.color;
			}
			else {
				if (paredesParents.parentWallLeft.renderer.material.name != paredeMaterial.name+" (Instance)") {
					paredesParents.parentWallLeft.renderer.material = paredeMaterial;
					paredesParents.parentWallLeft.renderer.material.color = paredesParents.colorWallLeft;
					paredesParents.parentWallLeft.collider.enabled = true;
				}
				paredesParents.colorWallLeft = paredesParents.parentWallLeft.renderer.material.color;
			}
			
			//paredes da direita
			if (transform.localEulerAngles.y > 200 && transform.localEulerAngles.y < 340) {
				if (paredesParents.parentWallRight.renderer.material.name != paredeTransparent.name+" (Instance)") {
					paredesParents.parentWallRight.renderer.material = paredeTransparent;
					paredesParents.parentWallRight.renderer.material.color = paredesParents.colorWallRight;
					paredesParents.parentWallRight.collider.enabled = false;
				}
				paredesParents.colorWallRight = paredesParents.parentWallRight.renderer.material.color;
			}
			else {
				if (paredesParents.parentWallRight.renderer.material.name != paredeMaterial.name+" (Instance)") {
					paredesParents.parentWallRight.renderer.material = paredeMaterial;
					paredesParents.parentWallRight.renderer.material.color = paredesParents.colorWallRight;
					paredesParents.parentWallRight.collider.enabled = true;
				}
				paredesParents.colorWallRight = paredesParents.parentWallRight.renderer.material.color;
			}

			//paredes de atrás
			if (transform.localEulerAngles.y > 290 || transform.localEulerAngles.y < 70) {
				if (paredesParents.parentWallBack.renderer.material.name != paredeTransparent.name+" (Instance)") {
					paredesParents.parentWallBack.renderer.material = paredeTransparent;
					paredesParents.parentWallBack.renderer.material.color = paredesParents.colorWallBack;
					paredesParents.parentWallBack.collider.enabled = false;
				}
				paredesParents.colorWallBack = paredesParents.parentWallBack.renderer.material.color;
			}
			else {
				if (paredesParents.parentWallBack.renderer.material.name != paredeMaterial.name+" (Instance)") {
					paredesParents.parentWallBack.renderer.material = paredeMaterial;
					paredesParents.parentWallBack.renderer.material.color = paredesParents.colorWallBack;
					paredesParents.parentWallBack.collider.enabled = true;
				}
				paredesParents.colorWallBack = paredesParents.parentWallBack.renderer.material.color;
			}
			
			//paredes de frente
			if (transform.localEulerAngles.y > 110 && transform.localEulerAngles.y < 270) {
				if (paredesParents.parentWallFront.renderer.material.name != paredeTransparent.name+" (Instance)") {
					paredesParents.parentWallFront.renderer.material = paredeTransparent;
					paredesParents.parentWallFront.renderer.material.color = paredesParents.colorWallFront;
					paredesParents.parentWallFront.collider.enabled = false;
				}
				paredesParents.colorWallFront = paredesParents.parentWallFront.renderer.material.color;
			}
			else {
				if (paredesParents.parentWallFront.renderer.material.name != paredeMaterial.name+" (Instance)") {
					paredesParents.parentWallFront.renderer.material = paredeMaterial;
					paredesParents.parentWallFront.renderer.material.color = paredesParents.colorWallFront;
					paredesParents.parentWallFront.collider.enabled = true;
				}
				paredesParents.colorWallFront = paredesParents.parentWallFront.renderer.material.color;
			}
		}
	}

	void MoveCamera () {
		Vector3 direcao = (transform.right * Input.GetAxis ("Horizontal") * Time.deltaTime * 5) + (transform.forward * Input.GetAxis ("Vertical") * Time.deltaTime * 5);
		direcao = new Vector3(direcao.x, 0, direcao.z);
		transform.position += direcao;
		if (Input.GetKey (KeyCode.Q))
			transform.eulerAngles -= new Vector3 (0, 1, 0);
		if (Input.GetKey (KeyCode.E))
			transform.eulerAngles += new Vector3 (0, 1, 0);
	}
	
	void MouseMoveCamera () {
		if (!isDoingLerp) {
			if (Input.GetMouseButton(1)) {
				float x = Input.GetAxis("Mouse X") * 2;
				float y = Input.GetAxis("Mouse Y") * 2;
				
				transform.localEulerAngles += new Vector3(-y, x, 0);	
			}
			
			if (Input.GetMouseButton(2)) {
				float x = Input.GetAxis("Mouse X");
				float y = Input.GetAxis("Mouse Y");
				
				transform.localPosition += transform.TransformDirection(new Vector3(x, y, 0));
			}
			
			transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -45f, 45f), 
			                                      Mathf.Clamp(transform.localPosition.y, -10f, 10f), 
			                                      Mathf.Clamp(transform.localPosition.z, -45f, 45f));
			
			// Mouse wheel moving forward
			if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f && transform.position.y > -10f) {
				if (!MouseUtils.MouseClickedInArea(guiCatalogo.wndAccordMain) &&
				    !MouseUtils.MouseClickedInArea(guiCamera.wndOpenMenu) &&
				    !MouseUtils.MouseClickedInArea(guiDescription.window)) {
					Ray rayMouse = camera.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(rayMouse) || !Physics.Raycast(rayMouse)) {
						Vector3 cameraPosition = (rayMouse.origin - transform.position) * SpeedZoom;
						StartCoroutine(LerpCamera(cameraPosition, true));
					}
				}
			}
			
			// Mouse wheel moving backward
			if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f && transform.position.y < 10f) {
				if (!MouseUtils.MouseClickedInArea(guiCatalogo.wndAccordMain) &&
				    !MouseUtils.MouseClickedInArea(guiCamera.wndOpenMenu) &&
				    !MouseUtils.MouseClickedInArea(guiDescription.window)) {
					Ray rayMouse = camera.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(rayMouse) || !Physics.Raycast(rayMouse)) {
						Vector3 cameraPosition = (rayMouse.origin - transform.position) * SpeedZoom;
						StartCoroutine(LerpCamera(cameraPosition, false));
					}
				}
			}
		}
	}
	
	public static bool isDoingLerp = false;
	public static IEnumerator LerpCamera (Vector3 position, bool positive){
	    float i = 0;
		isDoingLerp = true;
	    while(i < 1) {
			if (sCamera.transform.position.y > -10f) {
				if (positive)
					sCamera.transform.localPosition += position * Time.deltaTime * StepZoom;
				else
					sCamera.transform.localPosition -= position * Time.deltaTime * StepZoom;
		        i += Time.deltaTime * 2;
		        yield return null;
			}
			else break;
	    }
		isDoingLerp = false;
	}
}