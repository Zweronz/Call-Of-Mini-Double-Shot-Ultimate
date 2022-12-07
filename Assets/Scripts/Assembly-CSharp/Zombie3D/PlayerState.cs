namespace Zombie3D
{
	public abstract class PlayerState
	{
		public virtual void OnEnter(Player player)
		{
		}

		public virtual void NextState(Player player, float deltaTime)
		{
		}

		public virtual void OnExit(Player player)
		{
		}

		public virtual void OnHit(Player player, float damage)
		{
			if (player.HP <= 0f)
			{
				player.StopFire();
				player.OnDead();
				player.SetState(Player.DEAD_STATE);
			}
			else if (player.CouldGetAnotherHit())
			{
				player.CreateScreenBlood(damage);
			}
		}
	}
}
