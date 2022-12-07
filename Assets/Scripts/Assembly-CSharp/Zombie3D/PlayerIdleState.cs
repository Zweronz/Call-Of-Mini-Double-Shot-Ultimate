using UnityEngine;

namespace Zombie3D
{
	public class PlayerIdleState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.m_bIdle = false;
		}

		public override void OnExit(Player player)
		{
			player.m_bIdle = false;
		}

		public override void NextState(Player player, float deltaTime)
		{
			if (!player.FriendPlayer)
			{
				InputController inputController = player.InputController;
				InputInfo inputInfo = new InputInfo();
				inputController.ProcessInput(deltaTime, inputInfo);
				if (!inputInfo.fire)
				{
					player.ZoomOut(deltaTime);
				}
				player.Animate("Idle01" + player.WeaponNameEnd, WrapMode.Loop);
				player.m_bIdle = true;
				if (inputInfo.fire && !inputInfo.IsMoving())
				{
					player.SetState(Player.SHOOT_STATE);
				}
				else if (inputInfo.fire && inputInfo.IsMoving())
				{
					player.SetState(Player.RUNSHOOT_STATE);
				}
				else if (!inputInfo.fire && inputInfo.IsMoving())
				{
					player.SetState(Player.RUN_STATE);
				}
				return;
			}
			Player player2 = GameApp.GetInstance().GetGameScene().GetPlayer();
			bool flag = false;
			bool flag2 = false;
			if (player2.ActiveSkillImpl != null)
			{
				switch (player2.ActiveSkillImpl.GetSkill().SkillType)
				{
				case enSkillType.CoverMe:
					flag = true;
					break;
				case enSkillType.DoubleTeam:
					flag2 = true;
					break;
				}
			}
			if (flag || flag2)
			{
				InputController inputController2 = player2.InputController;
				bool bFire = ((TopWatchingInputController)inputController2).bFire;
				bool isRunning = player2.IsRunning;
				if (bFire || isRunning)
				{
					if (bFire && !isRunning)
					{
						player.SetState(Player.SHOOT_STATE);
						return;
					}
					if (bFire && isRunning)
					{
						player.SetState(Player.RUNSHOOT_STATE);
						return;
					}
					if (!bFire && isRunning)
					{
						player.SetState(Player.RUN_STATE);
						return;
					}
				}
				bool flag3 = player.IsPlayingAnimation("Idle01" + player.WeaponNameEnd);
				bool flag4 = player.AnimationEnds("Idle01" + player.WeaponNameEnd);
				if ((flag3 && flag4) || !flag3)
				{
					player.Animate("Idle01" + player.WeaponNameEnd, WrapMode.Loop);
				}
			}
			else if (player.friendTargetEnemy != null)
			{
				player.SetState(Player.SHOOT_STATE);
			}
			else if (Vector3.Distance(player2.GetTransform().position, player.GetTransform().position) > GameApp.GetInstance().GetGameScene().GetGameParameters()
				.PlayersDistance)
			{
				player.SetState(Player.RUN_STATE);
			}
			else
			{
				bool flag5 = player.IsPlayingAnimation("Idle01" + player.WeaponNameEnd);
				bool flag6 = player.AnimationEnds("Idle01" + player.WeaponNameEnd);
				if ((flag5 && flag6) || !flag5)
				{
					player.Animate("Idle01" + player.WeaponNameEnd, WrapMode.Loop);
				}
			}
		}
	}
}
