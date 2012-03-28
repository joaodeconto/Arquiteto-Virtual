using UnityEngine;
using System.Collections;

public class Camera3d : MonoBehaviour
{
	static public Camera sCamera;
	static public float SpeedZoom { get; private set; }
	static public float StepZoom  { get; private set; }
	
	private GameObject movelSelecionado;
	private FurnitureManager furnitureManager;
	private InfoController infoController;
	private CameraGUIController cameraGUIController;
	private bool CanMoveCamera;
	
	void Start () {
		
		CanMoveCamera = true;
		
		furnitureManager = GetComponent<FurnitureManager> ();
	
		sCamera = GetComponent<Camera>();
		SpeedZoom = 1.5f;
		StepZoom  = 8f;
		
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		cameraGUIController = GameObject.FindWithTag("GameController").GetComponentInChildren<CameraGUIController>();
	}

	// Update is called once per frame
	void Update () {
		if (CanMoveCamera)
		{
			MoveCamera();
			MouseMoveCamera();
			SelectMobile ();
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
			
			transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, 955f, 1045f), 
			                                      Mathf.Clamp(transform.localPosition.y, -10f, 10f), 
			                                      Mathf.Clamp(transform.localPosition.z, 955f, 1045f));
			
			// Mouse wheel moving forward
			if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f && transform.position.y > -10f) {
//				if (!MouseUtils.MouseClickedInArea(guiCatalogo.wndAccordMain) &&
//				    !MouseUtils.MouseClickedInArea(guiCamera.wndOpenMenu) &&
//				    !MouseUtils.MouseClickedInArea(guiDescription.window)) {
					Ray rayMouse = camera.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(rayMouse) || !Physics.Raycast(rayMouse)) {
						Vector3 cameraPosition = (rayMouse.origin - transform.position) * SpeedZoom;
						StartCoroutine(LerpCamera(cameraPosition, true));
					}
//				}
			}
			
			// Mouse wheel moving backward
			if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f && transform.position.y < 10f) {
//				if (!MouseUtils.MouseClickedInArea(guiCatalogo.wndAccordMain) &&
//				    !MouseUtils.MouseClickedInArea(guiCamera.wndOpenMenu) &&
//				    !MouseUtils.MouseClickedInArea(guiDescription.window)) {
					Ray rayMouse = camera.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(rayMouse) || !Physics.Raycast(rayMouse)) {
						Vector3 cameraPosition = (rayMouse.origin - transform.position) * SpeedZoom;
						StartCoroutine(LerpCamera(cameraPosition, false));
					}
//				}
			}
		}
	}
	
	private void SelectMobile () {
		if (Input.GetMouseButtonDown(0) && 
		    !furnitureManager.isNewFurnitureActive()) 
		{
			//Se clicar em cima de uma GUI ele ignora e volta
			if (cameraGUIController.ClickInGUI ()) return;
			CheckActiveFurniture();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			//Se clicar em cima de uma GUI ele ignora e volta
			if (cameraGUIController.ClickInGUI ()) return;
			MoveActiveNewFurniture ();
		}		
	}
	
	private void CheckActiveFurniture (){
		
		movelSelecionado = GameObject.FindGameObjectWithTag("MovelSelecionado");
		
		//Botão esquerdo
		if (Input.GetMouseButtonDown(0)) {
			
			//Deselecionar móvel selecionado se clicar com o botão esquerdo
			if (movelSelecionado != null) {
				movelSelecionado.GetComponentInChildren<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
				movelSelecionado.tag = "Movel";
				movelSelecionado.GetComponentInChildren<SnapBehaviour>().Select = false;					
				GetComponent<RenderBounds>().Display = false;
				infoController.Close();
			}
			
			RaycastHit hit = new RaycastHit ();
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			GameObject[] moveis = GameObject.FindGameObjectsWithTag ("Movel");
			
			//Só continua se pegar algum móvel no ray cast do mouse
			//Se houverem moveis na cena
			if (moveis.Length > 0) {
				foreach (GameObject movel in moveis) {
					if (Physics.Raycast (ray, out hit, Mathf.Infinity) && hit.transform.tag != "Movel")
						continue;
					if (hit.transform == movel.transform) {
						movel.tag = "MovelSelecionado";
						movel.GetComponentInChildren<SnapBehaviour>().Select = true;
						movel.GetComponentInChildren<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
						GetComponent<RenderBounds>().Display = true;
						GetComponent<RenderBounds>().SetBox(movel);
						infoController.Open(movel.GetComponent<InformacoesMovel>());
					}
				}
			} else {
				print ("Não há mais objetos deselecionados.");
			}
		}
	}

	private void MoveActiveNewFurniture ()
	{
		
		if (!furnitureManager.isNewFurnitureActive ())
			return;
		
		GameObject activeFurniture = GetComponent<FurnitureManager> ().GetActiveNewFurniture ();
		
		if (activeFurniture.tag != "Movel") {
			if (!Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = camera.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Chao")) {
						activeFurniture.transform.position = hit.point;
					}
				}
			} else {
				furnitureManager.FreeActiveNewFurniture ();
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