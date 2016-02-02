using System;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/GameObject Event Action")]
	public class GameObjectEventAction : GenericEventAction
	{
		[SerializeField]
		[EnumMask]
		private GameObjectEventFlags triggerOn = (GameObjectEventFlags)0;

		private bool quitting = false;

		protected override void Update()
		{
			base.Update();
			if ((triggerOn & GameObjectEventFlags.Update) != 0)
				Trigger();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if ((triggerOn & GameObjectEventFlags.OnEnable) != 0)
				Trigger();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (!quitting && (triggerOn & GameObjectEventFlags.OnDisable) != 0)
				Trigger();
		}

		protected override void OnEventActionReset()
		{
		}

		protected override void OnEventActionPause()
		{
		}

		protected override void OnEventActionResume()
		{
		}

		private void Awake()
		{
			if ((triggerOn & GameObjectEventFlags.Awake) != 0)
				Trigger();
		}

		private void Start()
		{
			if ((triggerOn & GameObjectEventFlags.Start) != 0)
				Trigger();
		}

		private void FixedUpdate()
		{
			if ((triggerOn & GameObjectEventFlags.FixedUpdate) != 0)
				Trigger();
		}

		private void OnDestroy()
		{
			if (!quitting && (triggerOn & GameObjectEventFlags.OnDestroy) != 0)
				Trigger();
		}

		private void OnApplicationQuit()
		{
			quitting = true;
		}
	}
}