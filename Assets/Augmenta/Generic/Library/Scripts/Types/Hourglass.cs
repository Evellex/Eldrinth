using UnityEngine;

namespace Augmenta
{
	public class Hourglass
	{
		private float expiryTime;
		private float timeRemaining;
		private CountdownType countdownType;
		private bool paused = false;

		public Hourglass(float duration, CountdownType countdownType)
		{
			this.countdownType = countdownType;
			if (countdownType == CountdownType.ScaledTime)
				expiryTime = Time.time + duration;
			else if (countdownType == CountdownType.UnscaledTime)
				expiryTime = Time.unscaledTime + duration;
		}

		public enum ChangeType
		{
			AddTime,
			WillExpireAt,
			WillExpireIn,
			MustExpireBefore,
			CantExpireBefore,
			MustExpireWithin,
			CantExpireWithin,
		}

		public enum CombineType
		{
			Latest,
			Soonest,
			Sum,
		}

		public enum CountdownType
		{
			ScaledTime,
			UnscaledTime,
		}

		public CountdownType CountdownTimerType
		{
			get { return countdownType; }
		}

		public float ExpiryTime
		{
			get { if (paused) expiryTime = TargetTime + timeRemaining; return expiryTime; }
			private set { expiryTime = value; timeRemaining = expiryTime - TargetTime; }
		}

		public float TimeRemaining
		{
			get { if (!paused) timeRemaining = expiryTime - TargetTime; return timeRemaining; }
			private set { timeRemaining = value; expiryTime = TargetTime + timeRemaining; }
		}

		public bool Expired
		{
			get { return ExpiryTime <= TargetTime; }
		}

		public bool Paused
		{
			get { return paused; }
			set { if (paused != value) { if (value) timeRemaining = expiryTime - TargetTime; else expiryTime = TargetTime + timeRemaining; paused = value; } }
		}

		private float TargetTime
		{
			get { if (countdownType == CountdownType.ScaledTime) return Time.time; else return Time.unscaledTime; }
		}

		/// <summary>
		/// Applies a change to the passed instance, as per the method of change.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="value"></param>
		/// <param name="changeType"></param>
		public static void Change(Hourglass target, float value, ChangeType changeType)
		{
			if (!target.paused)
			{
				if (changeType == ChangeType.AddTime)
					target.expiryTime += value;
				else if (changeType == ChangeType.CantExpireBefore)
					target.expiryTime = Mathf.Max(target.expiryTime, value);
				else if (changeType == ChangeType.MustExpireBefore)
					target.expiryTime = Mathf.Min(target.expiryTime, value);
				else if (changeType == ChangeType.WillExpireAt)
					target.expiryTime = value;
			}
			else
			{
				if (changeType == ChangeType.AddTime)
					target.timeRemaining += value;
				else if (changeType == ChangeType.CantExpireWithin)
					target.timeRemaining = Mathf.Max(target.timeRemaining, value);
				else if (changeType == ChangeType.MustExpireWithin)
					target.timeRemaining = Mathf.Min(target.timeRemaining, value);
				else if (changeType == ChangeType.WillExpireIn)
					target.timeRemaining = value;
			}
		}

		/// <summary>
		/// Applies a change to this Hourglass instance, as per the method of change.
		/// </summary>
		/// <param name="changeValue"></param>
		/// <param name="changeType"></param>
		public void Change(float changeValue, ChangeType changeType)
		{
			Change(this, changeValue, changeType);
		}
	}
}