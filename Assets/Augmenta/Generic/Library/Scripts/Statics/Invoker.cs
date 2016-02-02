using System.Collections.Generic;
using UnityEngine;

namespace Augmenta
{
	public static class Invoker
	{
		private static List<Invocation> invocations = new List<Invocation>(8);

		static Invoker()
		{
			UnityEvents.On_Update += Update;
		}

		public static void Invoke(System.Action Func, float delay = 0, bool realtime = false)
		{
			if (delay <= 0)
				Func();
			else
				invocations.Add(new Invocation(Func, new Hourglass(delay, realtime ? Hourglass.CountdownType.UnscaledTime : Hourglass.CountdownType.ScaledTime)));
		}

		public static void Invoke(System.Action Func, Hourglass hourglassTrigger)
		{
			if (hourglassTrigger.Expired)
				Func();
			else
				invocations.Add(new Invocation(Func, hourglassTrigger));
		}

		private static void Update()
		{
			invocations.ForEach(x => { if (x.hourglassTrigger.Expired) x.methodToInvoke(); });
			invocations.RemoveAll(x => x.hourglassTrigger.Expired);
		}

		private struct Invocation
		{
			public System.Action methodToInvoke;
			public Hourglass hourglassTrigger;

			public Invocation(System.Action methodToInvoke, Hourglass hourglassTrigger)
			{
				this.methodToInvoke = methodToInvoke;
				this.hourglassTrigger = hourglassTrigger;
			}
		}
	}
}