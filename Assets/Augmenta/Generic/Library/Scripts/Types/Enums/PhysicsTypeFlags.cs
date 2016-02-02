using UnityEngine;
using System.Collections;

namespace Augmenta
{
	[System.Flags]
	public enum PhysicsTypeFlags
	{
		Collision   = 1 << 0,
		Trigger     = 1 << 1,
	}	
}
