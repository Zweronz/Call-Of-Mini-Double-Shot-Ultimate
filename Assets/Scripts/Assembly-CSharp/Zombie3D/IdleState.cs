using UnityEngine;

namespace Zombie3D
{
	public class IdleState : EnemyState
	{
		public override void NextState(Enemy enemy, float deltaTime, Player player)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE);
				return;
			}
			enemy.Animate("Idle01", WrapMode.Loop);
			if (enemy.CouldEnterAttackState())
			{
				enemy.SetState(Enemy.ATTACK_STATE);
				return;
			}
			float sqrMagnitude = (enemy.GetTransform().position - player.GetTransform().position).sqrMagnitude;
			if (sqrMagnitude < enemy.DetectionRange * enemy.DetectionRange)
			{
				enemy.SetState(Enemy.CATCHING_STATE);
			}
		}
	}
}
