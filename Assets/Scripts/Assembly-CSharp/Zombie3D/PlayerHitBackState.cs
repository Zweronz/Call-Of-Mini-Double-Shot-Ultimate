using UnityEngine;

namespace Zombie3D
{
	public class PlayerHitBackState : PlayerState
	{
		private int m_HitBackAnimPlayedTimes;

		public override void OnEnter(Player player)
		{
			m_HitBackAnimPlayedTimes = 0;
		}

		public override void OnExit(Player player)
		{
			m_HitBackAnimPlayedTimes = 0;
		}

		public override void NextState(Player player, float deltaTime)
		{
			if (!player.FriendPlayer)
			{
				bool flag = player.IsPlayingAnimation("Damage01" + player.WeaponNameEnd);
				bool flag2 = player.AnimationEnds("Damage01" + player.WeaponNameEnd);
				if ((flag && flag2) || !flag)
				{
					if (m_HitBackAnimPlayedTimes < 1)
					{
						player.Animate("Damage01" + player.WeaponNameEnd, WrapMode.Once);
						m_HitBackAnimPlayedTimes++;
					}
					else
					{
						player.Animate("Idle01" + player.WeaponNameEnd, WrapMode.Loop);
					}
				}
				if (player.Faint)
				{
					Vector3 vector = player.GetRespawnTransform().TransformDirection(player.hitBackDir);
					vector += Physics.gravity * deltaTime;
					CharacterController component = player.PlayerObject.GetComponent<CharacterController>();
					if (component != null)
					{
						component.Move(vector * deltaTime * (player.hitBackDistance / player.faintTime));
					}
				}
				else
				{
					player.SetState(Player.IDLE_STATE);
				}
			}
			else
			{
				bool flag3 = player.IsPlayingAnimation("Damage01" + player.WeaponNameEnd);
				bool flag4 = player.AnimationEnds("Damage01" + player.WeaponNameEnd);
				if ((flag3 && flag4) || !flag3)
				{
					player.Animate("Damage01" + player.WeaponNameEnd, WrapMode.Once);
				}
				if (player.Faint)
				{
					Vector3 vector2 = player.GetRespawnTransform().TransformDirection(player.hitBackDir);
					vector2 += Physics.gravity * deltaTime;
					player.Move(vector2 * deltaTime * (player.hitBackDistance / player.faintTime));
				}
				else
				{
					player.SetState(Player.IDLE_STATE);
				}
			}
		}
	}
}
