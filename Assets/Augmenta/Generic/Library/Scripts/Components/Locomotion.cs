using System.Collections;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Locomotion")]
	public class Locomotion : MonoBehaviour
	{
		[SerializeField]
		private new Rigidbody rigidbody;

		[SerializeField]
		private CapsuleCollider capCollider;

		[SerializeField]
		private float acceleration = 100.0f;

		[SerializeField]
		private float maxSpeed = 10.0f;

		[SerializeField]
		private float maxStepSize = 0.5f;

		[SerializeField]
		private float maxWalkingAngle = 125.0f;

		private Vector3 capColliderPosOffset = Vector3.zero;
		private bool grounded = false;

		private Vector3 movementDirection = Vector3.zero;
		private Vector3 facingDirection = Vector3.zero;

		private float colliderRadius = 0;
		private float colliderHeight = 0;
		private Vector3 scale = Vector3.zero;

		private float maxWalkingNormal = 0.707f;
		private float maxWalkingAngleInternal = 125.0f;

		private RaycastHit[] raycastHits;
		private RaycastHit hitInfoMin;
		private RaycastHit hitInfoMinWalk;

		private float minWalkDist = 100.0f;
		private float minDist = 100.0f;

		private Vector3 worldMovementDirection;
		private Vector3 worldFacingDirection;

		private Vector3 originalVelocity;
		private Vector3 addedVelocity;
		private Vector3 newVelocity;

		public void SetMovementDirection(Vector3 newMovementDirection)
		{
			movementDirection = newMovementDirection;
		}

		public void SetFacingDirection(Vector3 newFacingDirection)
		{
			facingDirection = newFacingDirection;
		}

		private void OnValidate()
		{
			acceleration = Mathf.Max(0, acceleration);
			maxSpeed = Mathf.Max(0, maxSpeed);
			maxStepSize = Mathf.Max(0, maxStepSize);
			maxWalkingAngle = Mathf.Clamp(maxWalkingAngle, 0.0f, 250.0f);
		}

		private void FixedUpdate()
		{
			PerformLocomotionUpdate();
		}

		private void PerformLocomotionUpdate()
		{
			UpdateMaxWalkingNormal();
			VerifyUniformScale();
			UpdateCapColliderValues();
			DoCapsuleCast();
			GetRelevantGroundPoints();
			CheckForGrounding();
			ProjectControlVectors();
			CalculateForces();
			ControlRigidbodyRotation();
		}

		private void UpdateCapColliderValues()
		{
			if (capCollider && (
				!Vector3Ext.Approximately(scale, transform.localScale) ||
				!Mathf.Approximately(colliderRadius, capCollider.radius) ||
				!Mathf.Approximately(colliderHeight, capCollider.height)))
			{
				scale = transform.lossyScale;
				colliderRadius = capCollider.radius;
				colliderHeight = capCollider.height;
				float offsetValue = (Mathf.Max(capCollider.radius, capCollider.height * 0.5f) - capCollider.radius) * transform.localScale.x;
				if (capCollider.direction == 0)
					capColliderPosOffset = (rigidbody.rotation * Vector3.right) * offsetValue;
				else if (capCollider.direction == 1)
					capColliderPosOffset = (rigidbody.rotation * Vector3.up) * offsetValue;
				else if (capCollider.direction == 2)
					capColliderPosOffset = (rigidbody.rotation * Vector3.forward) * offsetValue;
			}
		}

		private void UpdateMaxWalkingNormal()
		{
			if (maxWalkingAngleInternal != maxWalkingAngle)
			{
				maxWalkingAngleInternal = maxWalkingAngle;
				maxWalkingNormal = Mathf.Sin((float)(maxWalkingAngle * MathfExt.mτToRad));
			}
		}

		private void VerifyUniformScale()
		{
			if (!Vector3Ext.IsUniform(transform.lossyScale))
			{
				Debug.LogError("\"" + gameObject.name + "\" or one of its ancestors does not have a uniform scale! Locomotion does not support non-uniform scales!");
			}
		}

		private void DoCapsuleCast()
		{
			Vector3 centerPosition = capCollider.transform.position + ((capCollider.transform.rotation * capCollider.center) * transform.localScale.x) + (Vector3.up * 0.01f);
			Vector3 point1 = centerPosition + (capColliderPosOffset);
			Vector3 point2 = centerPosition + (-capColliderPosOffset);
			raycastHits = Physics.CapsuleCastAll(point1, point2, capCollider.radius, Vector3.down, 10.0f, PhysicsExt.GetCollisionMask(capCollider.gameObject.layer));
		}

		private void GetRelevantGroundPoints()
		{
			minWalkDist = 100.0f;
			minDist = 100.0f;
			for (int i = 0; i < raycastHits.Length; ++i)
			{
				RaycastHit currentHit = raycastHits[i];
				if (currentHit.point != Vector3.zero)
				{
					if (currentHit.normal.y > maxWalkingNormal && currentHit.distance < minWalkDist)
					{
						hitInfoMinWalk = currentHit;
						minWalkDist = currentHit.distance;
					}
					if (currentHit.distance < minDist)
					{
						hitInfoMin = currentHit;
						minDist = currentHit.distance;
					}
				}
			}
		}

		private void CheckForGrounding()
		{
			float offset = capCollider.contactOffset + hitInfoMin.collider.contactOffset;
			bool minDistCheck = (minDist < offset);
			bool minWalkDistCheck = (minWalkDist < maxStepSize);
			rigidbody.useGravity = !(grounded = (minDistCheck && minWalkDistCheck));
		}

		private void ProjectControlVectors()
		{
			worldMovementDirection = Vector3.ProjectOnPlane(movementDirection, hitInfoMinWalk.normal).normalized * movementDirection.magnitude;
			worldFacingDirection = Vector3.ProjectOnPlane(facingDirection, Vector3.up).normalized * facingDirection.magnitude;
		}

		private void CalculateForces()
		{
			if (grounded)
			{
				originalVelocity = rigidbody.velocity;
				float accelAmount = Time.fixedDeltaTime * acceleration;
				if (worldMovementDirection != Vector3.zero)
					addedVelocity = worldMovementDirection * accelAmount;
				else if (originalVelocity.magnitude < accelAmount)
					addedVelocity = -originalVelocity;
				else
					addedVelocity = (-originalVelocity).normalized * accelAmount;
				newVelocity = originalVelocity + addedVelocity;
				newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
				addedVelocity = newVelocity - originalVelocity;
				rigidbody.AddForce(addedVelocity, ForceMode.VelocityChange);
			}
		}

		private void ControlRigidbodyRotation()
		{
			if (worldFacingDirection != Vector3.zero)
				rigidbody.MoveRotation(Quaternion.LookRotation(worldFacingDirection, Vector3.up));
			else if (worldMovementDirection != Vector3.zero)
				rigidbody.MoveRotation(Quaternion.LookRotation(worldMovementDirection, Vector3.up));
			else
			{
				Vector3 flatVelocity = Vector3.ProjectOnPlane(rigidbody.velocity, Vector3.up);
				if (flatVelocity.sqrMagnitude > 0.01f)
					rigidbody.MoveRotation(Quaternion.LookRotation(flatVelocity, Vector3.up));
				else
					rigidbody.MoveRotation(Quaternion.LookRotation(Vector3.ProjectOnPlane(rigidbody.rotation * Vector3.forward, Vector3.up), Vector3.up));
			}
		}
	}
}