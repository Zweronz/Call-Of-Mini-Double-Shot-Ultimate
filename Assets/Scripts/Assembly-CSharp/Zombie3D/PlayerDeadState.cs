namespace Zombie3D
{
	public class PlayerDeadState : PlayerState
	{
		public override void NextState(Player player, float deltaTime)
		{
			player.ZoomOut(deltaTime);
		}

		public override void OnHit(Player player, float damage)
		{
		}
	}
}
