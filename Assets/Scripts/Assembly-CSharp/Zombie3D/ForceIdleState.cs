using UnityEngine;

namespace Zombie3D
{
	public class ForceIdleState : EnemyState
	{
		public override void NextState(Enemy enemy, float deltaTime, Player player)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE);
			}
			else if (!enemy.IsPlayingAnimation("Idle01"))
			{
				enemy.Animate("Idle01", WrapMode.Loop);
			}
		}
	}
}
