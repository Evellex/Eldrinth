namespace Augmenta.Combat
{
	public interface IHealthEvent
	{
		void OnDamageReceived(DamageInfo info);

		void OnDeath(DamageInfo info);

		void OnHealReceived(DamageInfo info);
	}
}