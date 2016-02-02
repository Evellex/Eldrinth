using System.Collections.Generic;
using UnityEngine;

namespace Augmenta.Combat
{
	[AddComponentMenu("")]
	public class DamageReceiver : MonoBehaviour
	{
		private List<Health> connectedHealthScripts = new List<Health>();

		public void ConnectHealth(Health scriptToConnect)
		{
			if (!connectedHealthScripts.Contains(scriptToConnect))
				connectedHealthScripts.Add(scriptToConnect);
		}

		public void DisconnectHealth(Health scriptToDisconnect)
		{
			connectedHealthScripts.Remove(scriptToDisconnect);
		}

		public void RequestHandshake(Damage source)
		{
			foreach (Health h in connectedHealthScripts)
			{
				h.ReceiveHandshakeRequest(source, this);
			}
		}
	}
}