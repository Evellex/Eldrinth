using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Random Initial Transform")]
	public class RandomInitialTransform : MonoBehaviour, IPoolEvent
	{
		[SerializeField]
		private bool applyAtRuntime = true;

		[SerializeField]
		private bool randomRotation = false;

		[SerializeField]
		private Axes rotationType;

		[SerializeField]
		private Space rotationSpace;

		[SerializeField]
		private Vector3 rotationAxis;

		[SerializeField]
		private bool randomScale = false;

		[SerializeField]
		private AnimationCurve scaleDistribution;

		public enum Axes
		{
			AllAxes,
			XAxis,
			YAxis,
			ZAxis,
			CustomAxis,
		}

		public void ApplyRandomTransform()
		{
			if (randomRotation)
				ApplyRotation();
			if (randomScale)
				ApplyScale();
		}

		void IPoolEvent.OnDeposit()
		{
		}

		void IPoolEvent.OnWithdraw()
		{
			if (applyAtRuntime)
				ApplyRandomTransform();
		}

		private void Awake()
		{
			if (applyAtRuntime)
				ApplyRandomTransform();
		}

		private void ApplyScale()
		{
			float min = 9999;
			float max = -9999;
			if (scaleDistribution.keys.Length == 0)
			{
				Console.PrintError("Script \"RandomInitialTransform\" on GameObject \"" + gameObject.name + "\" has no curve keyframes defined! Scale has not been changed.");
			}
			else
			{
				foreach (Keyframe key in scaleDistribution.keys)
				{
					min = Mathf.Min(key.time, min);
					max = Mathf.Max(key.time, max);
				}
				transform.localScale = scaleDistribution.Evaluate(Random.Range(min, max)) * Vector3.one;
			}
		}

		private void ApplyRotation()
		{
			if (rotationType == Axes.AllAxes)
				transform.rotation = Random.rotationUniform;
			else
			{
				float randomAngle = Random.Range(0.0f, 360.0f);
				Quaternion newRotation = Quaternion.identity;

				switch (rotationType)
				{
					case Axes.XAxis:
						newRotation = Quaternion.AngleAxis(randomAngle, Vector3.right);
						break;

					case Axes.YAxis:
						newRotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
						break;

					case Axes.ZAxis:
						newRotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
						break;

					case Axes.CustomAxis:
						if (rotationAxis == Vector3.zero)
							Console.PrintText("Field \"Custom Rotation Axis\" of Script \"Random Initial Transform\" on GameObject \"" + gameObject.name + "\" is Vector3.zero. Rotation has not been changed.", true);
						else
							newRotation = Quaternion.AngleAxis(randomAngle, rotationAxis.normalized);
						break;

					default:
						break;
				}

				switch (rotationSpace)
				{
					case Space.World:
						transform.rotation = newRotation;
						break;

					case Space.Self:
						transform.localRotation = newRotation;
						break;

					default:
						break;
				}
			}
		}
	}
}