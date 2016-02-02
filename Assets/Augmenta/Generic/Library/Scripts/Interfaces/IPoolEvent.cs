namespace Augmenta
{
	public interface IPoolEvent
	{
		void OnDeposit();

		void OnWithdraw();
	}
}