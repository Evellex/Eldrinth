using UnityEngine;
using System.Collections.Generic;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Fast Mesh Particles")]
	public class FastMeshParticles : MonoBehaviour
	{
		[SerializeField]
		AnimationCurve sizeOverLifetime;

		ParticleSystem targetSystem = null;
		ParticleSystemRenderer targetRenderer = null;	
		ParticleSystem.Particle[] particleArray;

		List<GameObject> particleReplacements = new List<GameObject>();
		bool applicationQuitting = false;

		static GameObject template;
		static bool initialised = false;		

		void Awake()
		{
			if (!initialised)
				InitialisePool();
		}

		void Start()
		{
			targetSystem = GetComponent<ParticleSystem>();
			targetRenderer = targetSystem.GetComponent<ParticleSystemRenderer>();
			targetRenderer.enabled = false;
		}

		void Update()
		{
			UpdateParticles();
		}

		void OnDisable()
		{
			if (!applicationQuitting)
			{
				foreach (GameObject particle in particleReplacements)
				{
					ObjectPooler.Deposit(particle);
				}
			}
		}

		void OnApplicationQuit()
		{
			applicationQuitting = true;
		}		

		void UpdateParticles()
		{
			particleArray = new ParticleSystem.Particle[targetSystem.particleCount];
			targetSystem.GetParticles(particleArray);
			int i = 0;
			foreach (ParticleSystem.Particle particle in particleArray)
			{
				if (particleReplacements.Count <= i)
				{
					CreateNewParticleObject();
				}
				if (targetSystem.simulationSpace == ParticleSystemSimulationSpace.Local)
					particleReplacements[i].transform.position = targetSystem.transform.TransformPoint(particle.position);
				else if (targetSystem.simulationSpace == ParticleSystemSimulationSpace.World)
					particleReplacements[i].transform.position = particle.position;
				particleReplacements[i].transform.rotation = Quaternion.AngleAxis(particle.rotation, particle.axisOfRotation);
				particleReplacements[i].transform.localScale = Vector3Ext.Uniform(particle.size * sizeOverLifetime.Evaluate(1 - (particle.lifetime / particle.startLifetime)));
				++i;
			}
			int q = i;
			for (; i < particleReplacements.Count; ++i)
			{
				ObjectPooler.Deposit(particleReplacements[i]);
			}
			particleReplacements.RemoveRange(q, particleReplacements.Count - q);
		}

		void CreateNewParticleObject()
		{
			GameObject newParticleObject = ObjectPooler.Withdraw(template);
			newParticleObject.SetActive(true);
			newParticleObject.GetComponent<MeshFilter>().mesh = targetRenderer.mesh;
			newParticleObject.GetComponent<MeshRenderer>().material = targetRenderer.material;
			particleReplacements.Add(newParticleObject);
		}

		static void InitialisePool()
		{
			initialised = true;
			GameObject newParticleObject = new GameObject("MeshParticle");
			newParticleObject.SetActive(true);
			newParticleObject.AddComponent<MeshFilter>().mesh = null;
			MeshRenderer renderer = newParticleObject.AddComponent<MeshRenderer>();
			renderer.material = null;
			renderer.receiveShadows = false;
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			template = newParticleObject;
			ObjectPooler.InitialisePool(template, 5);
			template.SetActive(false);
			template.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		}
	}
}