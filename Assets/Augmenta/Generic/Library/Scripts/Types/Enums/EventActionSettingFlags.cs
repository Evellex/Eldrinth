namespace Augmenta
{
	[System.Flags]
	public enum EventActionSettingFlags
	{
		TriggerOnlyOnce = 1 << 0,
		ActionOnlyOnce = 1 << 1,
	}
}
