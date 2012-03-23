using UnityEngine;
using System.Collections;

public class ColliderControl : MonoBehaviour {
	
	private CameraController cameraController;
	
	private GameObject lastSelectedMobile;
	public bool IsPanelFloor {get; set;}

	void Start () {
		cameraController = GameObject.FindWithTag("GameController").GetComponentInChildren<CameraController>();
	}
	
	public void Enable () {
		
		Screen.lockCursor = true;
		
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
		
		GameObject teto = GameObject.Find("ParentTeto");
		if  (teto != null && teto.renderer != null)
			teto.renderer.enabled = teto.collider.enabled = true;
		
		int i = 0;
		foreach (Transform parede in cameraController.wallParents.GetWalls()) {
			if (parede != null) {
				parede.renderer.material = cameraController.wallMaterial;
				parede.renderer.material.color = cameraController.wallParents.GetWallColor(i);
				parede.collider.enabled = true;
				parede.collider.isTrigger = false;
				++i;
			}
		}
		
		cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (true);
	}
	
	// Update is called once per frame
	public void Disable () {
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
		
		if (cameraController.wallParents != null) {
			foreach (Transform parede in cameraController.wallParents.GetWalls()) {
				parede.collider.isTrigger = true;
			}
		}
		GameObject teto = GameObject.Find("ParentTeto");
		if  (teto != null && teto.renderer != null)
			teto.renderer.enabled = teto.collider.enabled = true;
					
	}
	
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			SnapBehaviour.ActivateAll();
			cameraController.mainCamera.gameObject.SetActiveRecursively(true);
			cameraController.setFirstPerson = false;
			cameraController.interfaceGUI.main.SetActiveRecursively(true);
			cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (false);
			
			Disable();
			if (!IsPanelFloor) {
				cameraController.interfaceGUI.viewUIPiso.SetActiveRecursively(false);
				cameraController.interfaceGUI.panelFloor.SetActiveRecursively(false);
			}
			if (lastSelectedMobile == null) {
				if (cameraController.interfaceGUI.panelInfo.active)
					cameraController.interfaceGUI.panelInfo.SetActiveRecursively(false);
			}
			
			gameObject.SetActiveRecursively(false);
		}
	}
}
