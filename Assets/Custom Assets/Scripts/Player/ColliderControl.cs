using UnityEngine;
using System.Collections;

public class ColliderControl : MonoBehaviour {
	
	private GUICameraController cameraController;
	
	private GameObject lastSelectedMobile;
	public bool IsPanelFloor {get; set;}

	void Start () {
		cameraController = GameObject.FindWithTag("GameController").GetComponentInChildren<GUICameraController>();
	}
	
	public void Enable () {
		
		Screen.lockCursor = true;
		
		cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (true);
		
		if (GameObject.FindGameObjectsWithTag("MovelSelecionado").Length == 1) {
			lastSelectedMobile = GameObject.FindWithTag("MovelSelecionado");
			lastSelectedMobile.tag = "Movel";
		}
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
		if (moveis.Length != 0) {
			foreach (GameObject movel in moveis) {
				movel.collider.isTrigger = true;
			}
		}
		
		GameObject teto = GameObject.FindWithTag("Teto");
		if  (teto != null && teto.renderer != null)
			teto.renderer.enabled = teto.collider.enabled = true;
		
		GameObject chao = GameObject.FindWithTag("Chao");
		if  (chao != null && chao.renderer != null)
			chao.renderer.enabled = chao.collider.enabled = true;
		
		int i = 0;
		foreach (Transform wallColor in cameraController.wallParent.transform) {
			if (wallColor != null)
			{
				wallColor.GetComponent<InfoWall>().wall.transform.GetChild(0).renderer.material = cameraController.wallMaterial;
				wallColor.GetComponent<InfoWall>().wall.transform.GetChild(0).renderer.material.color = wallColor.GetComponent<InfoWall>().color;
				wallColor.GetComponent<InfoWall>().wall.collider.enabled = true;
				wallColor.GetComponent<InfoWall>().wall.collider.isTrigger = false;
				++i;
			}
		}
	}
	
	// Update is called once per frame
	public void Disable () {
		
		cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (false);
		
		SnapBehaviour.ActivateAll();
		cameraController.mainCamera.gameObject.SetActiveRecursively(true);
		cameraController.setFirstPerson = false;
		cameraController.interfaceGUI.main.SetActiveRecursively(true);
		cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (false);
		
		Screen.lockCursor = false;
		
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
		if (moveis.Length != 0) {
			foreach (GameObject movel in moveis) {
				movel.collider.isTrigger = false;
				if (movel.GetComponent<InformacoesMovel>().portas != Portas.FECHADAS) {
					Animation[] animacoes = movel.GetComponentsInChildren<Animation>();
					foreach (Animation animacao in animacoes) {
						if (animacao.clip != null) {
							animacao[animacao.clip.name].speed = -1;
							animacao[animacao.clip.name].time = animacao[animacao.clip.name].length;
							animacao.Play();
						}
					}
					movel.GetComponent<InformacoesMovel>().portas = Portas.FECHADAS;
				}
			}
			if (lastSelectedMobile != null) {
				lastSelectedMobile.tag = "MovelSelecionado";
				lastSelectedMobile = null;
			}
		}
		
		if (cameraController.wallParent != null)
		{
			foreach (Transform wallColor in cameraController.wallParent.transform)
			{
				wallColor.GetComponent<InfoWall>().wall.collider.isTrigger = true;
			}
		}
		
		GameObject teto = GameObject.FindWithTag("Teto");
		if  (teto != null && teto.renderer != null)
			teto.renderer.enabled = teto.collider.enabled = true;
		
		GameObject chao = GameObject.FindWithTag("Chao");
		if  (chao != null && chao.renderer != null)
			chao.renderer.enabled = chao.collider.enabled = true;
		
		cameraController.EnableCeilFloor();
		
		if (!IsPanelFloor) {
			cameraController.interfaceGUI.viewUIPiso.SetActiveRecursively(false);
			cameraController.interfaceGUI.panelFloor.SetActiveRecursively(false);
		}
		
		//desativa sempre o panel info e o panel mobile
		cameraController.interfaceGUI.panelInfo.SetActiveRecursively(false);
		cameraController.interfaceGUI.panelMobile.SetActiveRecursively(false);
		
		#if UNITY_ANDROID || UNITY_IPHONE
		transform.parent.parent.gameObject.SetActiveRecursively(false);
		#else
		gameObject.SetActiveRecursively(false);
		#endif
	}
	
	#if !UNITY_ANDROID && !UNITY_IPHONE
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			Disable();
		}
	}
	#else
	void OnGUI () {
		if (MouseUtils.GUIMouseButtonDoubleClick(0)) {
			Disable ();
		}
	}
	#endif
}
