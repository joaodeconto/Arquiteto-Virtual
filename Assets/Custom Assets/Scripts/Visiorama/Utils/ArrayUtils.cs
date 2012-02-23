using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Visiorama
{
	namespace Utils
	{
		public class ArrayUtils
		{
			public static void Swap(IList<Object> objects, int a, int b) {
			    Object tmp = objects[a];
			    objects[a] = objects[b];
			    objects[b] = tmp;
			}
		}
	}
}
