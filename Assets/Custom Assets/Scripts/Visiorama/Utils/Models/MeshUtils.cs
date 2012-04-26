using UnityEngine;
using System.Collections;

public class MeshUtils {
	
	public static void CombineMesh (Transform target, bool createCollider)
	{
		CombineMesh(target, createCollider, false);
	}
	
	public static void CombineMesh (Transform target, bool createCollider, bool destroyChildren)
	{
		MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter> ();
		
		if(meshFilters.Length == 0)
		{
			Debug.LogError("The target doesn't have mesh filters!");
			return;
		}
		
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i != meshFilters.Length; ++i) {
			combine [i].mesh = meshFilters [i].sharedMesh;
			combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
		}
			
		target.GetComponent<MeshFilter> ().mesh = new Mesh ();
		target.GetComponent<MeshFilter> ().mesh.CombineMeshes (combine);
		if (createCollider) {
			MeshCollider mc = target.gameObject.AddComponent ("MeshCollider") as MeshCollider;
		}
		
		if (destroyChildren) {
			for(int i = target.transform.childCount - 1; i != -1; --i)
			{
				GameObject.Destroy(target.transform.GetChild(i).gameObject);
			}
		}
	}

	public static Vector3 FixZYFromBlender (Vector3 size)
	{
		float t	= size.y;
		size.y	= size.z;
		size.z 	= t;
		return size;
	}
}