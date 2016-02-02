namespace Augmenta
{
	[System.Obsolete("IPoolable is deprecated. Please use IPoolEvent.",false)]
	public interface IPoolable
	{
		void Recycle();
	}
}