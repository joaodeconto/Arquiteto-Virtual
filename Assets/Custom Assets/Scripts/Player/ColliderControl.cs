using UnityEngine;
using System.Collections;

public class ColliderControl : MonoBehaviour {
	
	public Camera camera3d;
	
	private GameObject lastSelectedMobile;
	void OnEnable () {
		if (GameObject.FindGameObjectsWithTag("MovelSelecionado").Length == 1) {
			lastSelectedMobile = GameObject.FindWithTag("MovelSelecionado");
			lastSelectedMobile.tag = "Movel";
		}
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
		foreach (GameObject movel in moveis) {
			movel.collider.isTrigger = true;
		}
		
		foreach (Transform parede in camera3d.GetComponent<Camera3d>().paredesParents.GetWalls()) {
			parede.renderer.material = camera3d.GetComponent<Camera3d>().paredeMaterial;
			parede.collider.enabled = true;
			parede.collider.isTrigger = false;
		}
		GameObject teto = GameObject.FindWithTag("TetoParent");
		if (teto.renderer != null && !teto.renderer.enabled)
			teto.renderer.enabled = true;
	}
	
	// Update is called once per frame
	void OnDisable () {
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
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
		
		foreach (Transform parede in camera3d.GetComponent<Camera3d>().paredesParents.GetWalls()) {
			parede.collider.isTrigger = true;
		}
		foreach (Transform teto in GameObject.Find("Teto").transform) {
			if (teto.renderer != null)
				teto.renderer.enabled = camera3d.GetComponent<Camera3d>().AreWallsAlwaysVisible;
		}
	}
	
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			SnapBehaviour.ActivateAll();
			gameObject.SetActiveRecursively(false);
			camera3d.gameObject.SetActiveRecursively(true); 
			foreach (Transform child in GameObject.Find("RotacaoCubo").transform) {
				child.gameObject.SetActiveRecursively(true);
			}
		}
	}
}
