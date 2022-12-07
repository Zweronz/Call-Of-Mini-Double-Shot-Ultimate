namespace Zombie3D
{
	public class PlayerGotHitState : PlayerState
	{
		public override void NextState(Player player, float deltaTime)
		{
			if (!player.IsPlayingAnimation("Damage01" + player.WeaponNameEnd))
			{
				player.SetState(Player.IDLE_STATE);
			}
		}
	}
}
