namespace Augmenta
{
	[System.Flags]
	public enum GameObjectEventFlags
	{
		Awake =			1 << 0,
		Start =			1 << 1,
		Update =		1 << 2,
		FixedUpdate =	1 << 3,
		OnEnable =		1 << 4,
		OnDisable =		1 << 5,
		OnDestroy =		1 << 6,
	}
}