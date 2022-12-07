using UnityEngine;

namespace Zombie3D
{
	public class PlayerForceIdleState : PlayerState
	{
		public override void NextState(Player player, float deltaTime)
		{
			if (!player.IsPlayingAnimation("Idle01" + player.WeaponNameEnd))
			{
				player.Animate("Idle01" + player.WeaponNameEnd, WrapMode.Loop);
			}
		}
	}
}
