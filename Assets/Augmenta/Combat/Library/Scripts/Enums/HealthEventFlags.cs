namespace Augmenta.Combat
{
	public enum HealthEventFlags
	{
		OnDamageReceived = 1 << 0,
		OnHealReceived = 1 << 1,
		OnDeath = 1 << 2,
	}
}