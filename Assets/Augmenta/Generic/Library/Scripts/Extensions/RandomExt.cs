using UnityEngine;
using System.Collections;

namespace Augmenta
{ 
	public static class RandomExt
	{
		public static Vector3 Range(Vector3 min, Vector3 max)
		{
			return new Vector3(Random.Range(min.x,max.x),Random.Range(min.y,max.y),Random.Range(min.z,max.z));
		}
	}
}
