using UnityEngine;

namespace Zombie3D
{
	public class VampireDogCatchingState : EnemyState
	{
		public void Init()
		{
		}

		public override void NextState(Enemy enemy, float deltaTime, Player player)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE);
				return;
			}
			enemy.FindPath();
			enemy.DoMove(deltaTime);
			if (Mathf.Abs(enemy.SqrDistanceFromPlayer - ((VampireDog)enemy).GetBeginJumpOnDir() * ((VampireDog)enemy).GetBeginJumpOnDir()) <= 0.2f)
			{
				enemy.DoMove(deltaTime * 3f);
				enemy.Animate(enemy.RunAnimationName, WrapMode.Loop);
				if (enemy.CouldEnterAttackState())
				{
					enemy.SetState(Enemy.ATTACK_STATE);
				}
			}
			else
			{
				enemy.Animate(enemy.RunAnimationName, WrapMode.Loop);
			}
		}
	}
}
