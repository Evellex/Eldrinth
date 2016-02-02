using UnityEngine;

namespace Augmenta
{
	[ExecuteInEditMode]
	[AddComponentMenu("Augmenta/Transform Motion")]
	public class TransformMotion : MonoBehaviour
	{
		[SerializeField]
		private UpdateType updateType = UpdateType.Rendering;

		[SerializeField]
		private bool translate = false;

		[SerializeField]
		private float translationSpeed = 0.5f;

		[SerializeField]
		private Vector3 translationAxis = Vector3.right;

		[SerializeField]
		private Space translationSpace = Space.Self;

		[SerializeField]
		private bool rotate = false;

		[SerializeField]
		private float rotationSpeed = 45.0f;

		[SerializeField]
		private Vector3 rotationAxis = Vector3.up;

		[SerializeField]
		private Space rotationSpace = Space.Self;

		[SerializeField]
		private RotationUnit rotationUnit = RotationUnit.Degrees;

		[HideInInspector]
		[SerializeField]
		private RotationUnit activeRotationUnit = RotationUnit.Degrees;

		[SerializeField]
		[HideInInspector]
		private bool activeRotationUnitInitialised = false;

		private float timePeriod = 0;

		public enum RotationUnit
		{
			MilliTau,
			Degrees,
		}

		public void SetRotationSpeed(float newRotationSpeed, RotationUnit newRotationUnit)
		{
			rotationSpeed = newRotationSpeed;
			rotationUnit = newRotationUnit;
		}

		public void SetRotationAxis(Vector3 newAxis)
		{
			rotationAxis = newAxis;
		}

		public void SetRotationSpace(Space newSpace)
		{
			rotationSpace = newSpace;
		}

		public void SetRotate(bool willRotate)
		{
			rotate = willRotate;
		}

		public void SetTranslation(bool willTranslate)
		{
			translate = willTranslate;
		}

		public void TranslationSpeed(float translateSpeed)
		{
			translationSpeed = translateSpeed;
		}

		private void OnValidate()
		{
			if (!activeRotationUnitInitialised)
			{
				activeRotationUnitInitialised = true;
				activeRotationUnit = rotationUnit;
			}

			if (activeRotationUnit != rotationUnit)
			{
				if (rotationUnit == RotationUnit.Degrees)
				{
					if (activeRotationUnit == RotationUnit.MilliTau)
						rotationSpeed = (float)(rotationSpeed * Mathd.mτToDeg);
				}
				if (rotationUnit == RotationUnit.MilliTau)
				{
					if (activeRotationUnit == RotationUnit.Degrees)
						rotationSpeed = (float)(rotationSpeed * Mathd.DegTomτ);
				}
				activeRotationUnit = rotationUnit;
			}
		}

		private void Update()
		{
			if (Application.isPlaying && updateType == UpdateType.Rendering)
			{
				timePeriod = Time.deltaTime;
				UpdateRotation();
				UpdateTranslation();
			}
		}

		private void FixedUpdate()
		{
			if (Application.isPlaying && updateType == UpdateType.Physics)
			{
				timePeriod = Time.fixedDeltaTime;
				UpdateRotation();
				UpdateTranslation();
			}
		}

		private void UpdateRotation()
		{
			if (rotate)
			{
				float rotationAmount = 0;
				if (rotationUnit == RotationUnit.MilliTau)
					rotationAmount = (float)(Mathd.mτToDeg * rotationSpeed * timePeriod);
				else if (rotationUnit == RotationUnit.Degrees)
					rotationAmount = rotationSpeed * timePeriod;
				transform.Rotate(rotationAxis, rotationAmount, rotationSpace);
			}
		}

		private void UpdateTranslation()
		{
			if (translate)
				transform.Translate(translationAxis.normalized * translationSpeed * timePeriod, translationSpace);
		}
	}
}