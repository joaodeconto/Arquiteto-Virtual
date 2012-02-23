using UnityEngine;
using System.Collections;

public class TrocarMesh : MonoBehaviour {
	
	public Mesh puxador1;
	public Mesh puxador2;
	public Mesh puxador3;
	public Mesh puxador4;
	
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(5f);
		if (puxador1 != null)
			GetComponent<MeshFilter>().mesh = puxador1;
//		if (puxador2 != null)
//			GetComponent<MeshFilter>().mesh = puxador2;
//		if (puxador3 != null)
//			GetComponent<MeshFilter>().mesh = puxador3;
//		if (puxador4 != null)
//			GetComponent<MeshFilter>().mesh = puxador4;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
