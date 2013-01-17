using UnityEngine;
using System.Collections.Generic;

public class WallController : MonoBehaviour {

	public List<Transform> walls { get; private set; }

	// Use this for initialization
	void Start ()
	{
		walls = new List<Transform>();
		foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Parede"))
		{
			walls.Add (wall.transform);
		}
		WallController.wc = GameObject.Find ("WallController").GetComponent<WallController> ();

	}

	private static WallController wc;
	public static void Add (GameObject wall)
	{
		if (!wc.walls.Contains (wall.transform))
		{
			wc.walls.Add (wall.transform);
		}
		else
		{
			Debug.LogError ("Whata? wc already contains this wall.");
		}
	}

	public static Transform GetNearestWall (Transform module)
	{
		float minDistance = float.MaxValue;
		float cDistance = 0;
		Transform nearestTransform = null;
		foreach (Transform wall in wc.walls)
		{
			cDistance = Vector3.Distance (wall.position, module.position);

			if (minDistance > cDistance)
			{
				minDistance = cDistance;
				nearestTransform = wall;
			}
		}

		return nearestTransform;
	}

	public static float GetNearestWallDistance (Transform module)
	{
		float minDistance = float.MaxValue;
		float cDistance = 0;
		Transform nearestTransform = null;

		foreach (Transform wall in wc.walls) {
			cDistance = Vector3.Distance (wall.position, module.position);
			if (minDistance > cDistance) {
				minDistance = cDistance;
				nearestTransform = wall;
			}
		}

		return minDistance;
	}
}
