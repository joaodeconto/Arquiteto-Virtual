using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Visiorama
{
	namespace Utils
	{
		public class ArrayUtils
		{
			public static void Add(Object[] array, Object item)
			{
				List<Object> objArray = new List<Object>(array);
				objArray.Add (item);
//				System.Array.Copy(objArray.ToArray (), (System.Array) array, objArray.Count);
				array = objArray.ToArray ();
				Debug.Log (array.Length);
			}
			
			public static void Swap(IList<Object> objects, int a, int b) {
			    Object tmp = objects[a];
			    objects[a] = objects[b];
			    objects[b] = tmp;
			}
		}
	}
}
