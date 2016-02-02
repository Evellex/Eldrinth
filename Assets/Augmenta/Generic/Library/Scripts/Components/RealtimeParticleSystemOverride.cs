using UnityEngine;

namespace Augmenta
{
	public class RealtimeParticleSystemOverride : MonoBehaviour
	{
		private new ParticleSystem particleSystem;

		private void Start()
		{
			particleSystem = GetComponent<ParticleSystem>();
		}

		private void Update()
		{
			if (particleSystem != null)
				particleSystem.Simulate(Time.unscaledDeltaTime, false, false);
		}
	}
}