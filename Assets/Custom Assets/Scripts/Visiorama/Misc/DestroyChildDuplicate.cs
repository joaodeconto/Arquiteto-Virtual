using UnityEngine;
using System.Collections;

public class DestroyChildDuplicate : MonoBehaviour {
	
	public string childName;
	public bool destroyInOrder;
	
	void OnEnable ()
	{
		int contains = 0;
		int lastIndex = -1;
		
		for (int i = 0; i != transform.GetChildCount(); i++)
		{
			if (transform.GetChild(i).ToString().Contains(childName))
			{
				if (!destroyInOrder)
				{
					if (contains != 0)
					{
						Destroy (transform.GetChild(i).gameObject);
					}
				}
				else
				{
					if (lastIndex != -1)
					{
						Destroy (transform.GetChild(lastIndex).gameObject);
					}
					lastIndex = i;
				}
				contains++;
			}
		}
	}
}
