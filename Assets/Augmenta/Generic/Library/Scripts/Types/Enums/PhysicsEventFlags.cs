namespace Augmenta
{
	[System.Flags]
	public enum PhysicsEventFlags
	{
		OnCollisionEnter = 1 << 0,
		OnCollisionStay = 1 << 1,
		OnCollisionExit = 1 << 2,
		OnTriggerEnter = 1 << 3,
		OnTriggerStay = 1 << 4,
		OnTriggerExit = 1 << 5,
		OnRigidbodySleep = 1 << 6,
	}
}