using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Jitter")]
	public class Jitter : MonoBehaviour
	{
		[SerializeField]
		private TargetType targetType = TargetType.Float;

		[SerializeField]
		private float speed;

		[SerializeField]
#pragma warning disable 0414
		private float smoothness = 1;

#pragma warning restore 0414

		[SerializeField]
		private LimitType overallLimitType = LimitType.Magnitude;

		[SerializeField]
		private LimitType movementLimitType = LimitType.Magnitude;

		[SerializeField]
		private float overallMagnitudeLimit = 1;

		[SerializeField]
		private float movementMagnitudeLimit = 0.5f;

		private float movementValue = 0;

		//Float
		[SerializeField]
		private FloatEvent onValueChangeFloat = new FloatEvent();

		[SerializeField]
		private float f_minimumOverall = -1;

		[SerializeField]
		private float f_maximumOverall = 1;

		[SerializeField]
		private float f_minimumChange = -0.5f;

		[SerializeField]
		private float f_maximumChange = 0.5f;

		private float f_value = 0;

		private List<float> f_TargetList = new List<float>();

		//Vector3
		[SerializeField]
		private Vector3Event onValueChangeVector3 = new Vector3Event();

		[SerializeField]
		private Vector3 v3_minimumOverall = -Vector3.one;

		[SerializeField]
		private Vector3 v3_maximumOverall = Vector3.one;

		[SerializeField]
		private Vector3 v3_minimumChange = Vector3.one * -0.5f;

		[SerializeField]
		private Vector3 v3_maximumChange = Vector3.one * 0.5f;

		private Vector3 v3_value = Vector3.zero;

		private List<Vector3> v3_TargetList = new List<Vector3>();

		public enum TargetType
		{
			Float,
			Vector3,
		}

		public enum LimitType
		{
			Magnitude,
			Range,
		}

		private void Start()
		{
			f_value = Random.Range(f_minimumOverall, f_maximumOverall);
		}

#if UNITY_EDITOR

		private void OnValidate()
		{
			ValidateValues();
		}

#endif

		private void ValidateValues()
		{
			float buffer = 0.0f;
			overallMagnitudeLimit = Mathf.Max(buffer, overallMagnitudeLimit);
			movementMagnitudeLimit = Mathf.Max(buffer, movementMagnitudeLimit);
		}

		private void Update()
		{
			ValidateValues();
			int loopCount = 0;
			movementValue = Time.deltaTime * speed;
			float distance;
			if (targetType == TargetType.Float)
			{
				if (f_TargetList.Count == 0)
					AddFloatTarget();
				distance = Mathf.Abs(f_TargetList[0] - f_value);
				while (movementValue > distance && loopCount < 100)
				{
					f_value = Mathf.MoveTowards(f_value, f_TargetList[0], distance);
					movementValue -= distance;
					AddFloatTarget();
					f_TargetList.RemoveAt(0);
					distance = Mathf.Abs(f_TargetList[0] - f_value);
					loopCount += 1;
				}
				f_value = Mathf.MoveTowards(f_value, f_TargetList[0], movementValue);
				onValueChangeFloat.Invoke(f_value);
			}
			else if (targetType == TargetType.Vector3)
			{
				if (v3_TargetList.Count == 0)
					AddVector3Target();
				distance = Vector3.Distance(v3_TargetList[0], v3_value);
				while (movementValue > distance && loopCount < 100)
				{
					v3_value = Vector3.MoveTowards(v3_value, v3_TargetList[0], distance);
					movementValue -= distance;
					AddVector3Target();
					v3_TargetList.RemoveAt(0);
					distance = Vector3.Distance(v3_TargetList[0], v3_value);
					loopCount += 1;
				}
				v3_value = Vector3.MoveTowards(v3_value, v3_TargetList[0], movementValue);
				onValueChangeVector3.Invoke(v3_value);
			}
		}

		private void AddFloatTarget()
		{
			float change = 0;
			if (movementLimitType == LimitType.Range)
			{
				change = Random.Range(f_minimumChange, f_maximumChange);
			}
			else if (movementLimitType == LimitType.Magnitude)
			{
				change = Random.Range(-movementMagnitudeLimit, movementMagnitudeLimit);
			}
			float original = f_TargetList.Count > 0 ? f_TargetList[f_TargetList.Count - 1] : 0;
			float newtarget = 0;
			if (overallLimitType == LimitType.Range)
			{
				newtarget = Mathf.Clamp(original + change, f_minimumOverall, f_maximumOverall);
			}
			else if (overallLimitType == LimitType.Magnitude)
			{
				newtarget = Mathf.Clamp(original + change, -overallMagnitudeLimit, overallMagnitudeLimit);
			}
			f_TargetList.Add(newtarget);
		}

		private void AddVector3Target()
		{
			Vector3 change = Vector3.zero;
			if (movementLimitType == LimitType.Range)
			{
				change = RandomExt.Range(v3_minimumChange, v3_maximumChange);
			}
			else if (movementLimitType == LimitType.Magnitude)
			{
				change = Random.onUnitSphere * Random.Range(0, movementMagnitudeLimit);
			}
			Vector3 original = v3_TargetList.Count > 0 ? v3_TargetList[v3_TargetList.Count - 1] : Vector3.zero;
			Vector3 newtarget = Vector3.zero;
			if (overallLimitType == LimitType.Range)
			{
				newtarget = Vector3Ext.ClampResult(original + change, v3_minimumOverall, v3_maximumOverall);
			}
			else if (overallLimitType == LimitType.Magnitude)
			{
				newtarget = Vector3.ClampMagnitude(original + change, overallMagnitudeLimit);
			}
			v3_TargetList.Add(newtarget);
		}
	}
}