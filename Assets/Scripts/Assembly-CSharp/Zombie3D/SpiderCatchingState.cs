using UnityEngine;

namespace Zombie3D
{
	public class SpiderCatchingState : EnemyState
	{
		public override void NextState(Enemy enemy, float deltaTime, Player player)
		{
			if (enemy.HP <= 0f)
			{
				enemy.OnDead();
				enemy.SetState(Enemy.DEAD_STATE);
				return;
			}
			if (((Spider)enemy).bStop)
			{
				enemy.Animate("Idle01", WrapMode.Loop);
			}
			else
			{
				enemy.FindPath();
				enemy.DoMove(deltaTime);
				enemy.Animate(enemy.RunAnimationName, WrapMode.Loop);
			}
			if (enemy.CouldEnterAttackState())
			{
				enemy.SetState(Enemy.ATTACK_STATE);
			}
		}
	}
}
