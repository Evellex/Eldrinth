namespace Augmenta.Combat
{
	[System.Flags]
	public enum DamageType
	{
		Generic = 1 << 0,
		Pierce = 1 << 1,
		Bludgeon = 1 << 2,
		Slash = 1 << 3,
		Fire = 1 << 4,
		Explosion = 1 << 5,
		Bullet = 1 << 6,
		Magic = 1 << 7,
	}
}