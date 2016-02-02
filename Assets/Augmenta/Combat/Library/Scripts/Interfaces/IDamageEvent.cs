namespace Augmenta.Combat
{
	public interface IDamageEvent
	{
		void OnDamageSent(DamageInfo info);

		void OnHealSent(DamageInfo info);
	}
}