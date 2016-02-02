using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	[AddComponentMenu("Augmenta/Timer Event Action")]
	public class TimerEventAction : GenericEventAction
	{
		[SerializeField]
		private float[] times;

		private List<Hourglass> hourglasses = new List<Hourglass>();

		[SerializeField]
		private float[] repeatTimes;

		private List<Hourglass> repeatHourglasses = new List<Hourglass>();

		[SerializeField]
		private bool preWarmedRepeat;

		protected override void OnEventActionReset()
		{
			ReinitialiseTimers();
		}

		protected override void OnEventActionPause()
		{
			hourglasses.ForEach(x => x.Paused = true);
			repeatHourglasses.ForEach(x => x.Paused = true);
		}

		protected override void OnEventActionResume()
		{
			hourglasses.ForEach(x => x.Paused = false);
			repeatHourglasses.ForEach(x => x.Paused = false);
		}

		protected override void Update()
		{
			base.Update();
			if ((settingsFlags & EventActionSettingFlags.TriggerOnlyOnce) == 0 || !triggered)
			{
				ManageRepeatTimers();
				ManageRegularTimers();
			}
		}

		private void Start()
		{
			ReinitialiseTimers();
		}

		private void ReinitialiseTimers()
		{
			hourglasses.Clear();
			repeatHourglasses.Clear();

			for (int i = 0; i < times.Length; ++i)
			{
				Hourglass newHourglass = new Hourglass(times[i], Hourglass.CountdownType.ScaledTime);
				newHourglass.Paused = !isActiveAndEnabled;
				hourglasses.Add(newHourglass);
			}
			for (int i = 0; i < repeatTimes.Length; ++i)
			{
				Hourglass newHourglass;
				newHourglass = new Hourglass(0, Hourglass.CountdownType.ScaledTime);
				if (!preWarmedRepeat)
					Hourglass.Change(newHourglass, repeatTimes[i], Hourglass.ChangeType.WillExpireAt);
				newHourglass.Paused = !isActiveAndEnabled;
				repeatHourglasses.Add(newHourglass);
			}
		}

		private void ManageRegularTimers()
		{
			hourglasses.ForEach(x => { if (x.Expired) Trigger(); });
			hourglasses.RemoveAll(x => x.Expired);
		}

		private void ManageRepeatTimers()
		{
			int i = 0;
			float clampValue = Time.deltaTime * 0.01f; //Maximum of 100 triggers per frame.
			repeatHourglasses.ForEach(x => { while (x.Expired) { Trigger(); x.Change(System.Math.Max(clampValue, repeatTimes[i]), Hourglass.ChangeType.AddTime); } ++i; });
		}
	}
}