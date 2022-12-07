using UnityEngine;

namespace Zombie3D
{
	public class PlayerRunState : PlayerState
	{
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
				string text = "Run__Two";
				if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
				{
					text = "Run__Shotgun";
				}
				else if (player.WeaponNameEnd == "_RPG")
				{
					text = "Run__RPG";
				}
				bool flag = player.IsPlayingAnimation(text);
				player.Animate(text, WrapMode.Loop);
				Weapon weapon = player.GetWeapon();
				if (!inputInfo.fire && !inputInfo.IsMoving())
				{
					player.SetState(Player.IDLE_STATE);
				}
				else if (inputInfo.fire && inputInfo.IsMoving())
				{
					if (weapon.HaveBullets())
					{
						player.SetState(Player.RUNSHOOT_STATE);
					}
				}
				else if (inputInfo.fire && !inputInfo.IsMoving())
				{
					player.SetState(Player.SHOOT_STATE);
				}
				return;
			}
			Player player2 = GameApp.GetInstance().GetGameScene().GetPlayer();
			bool flag2 = false;
			bool flag3 = false;
			if (player2.ActiveSkillImpl != null)
			{
				switch (player2.ActiveSkillImpl.GetSkill().SkillType)
				{
				case enSkillType.CoverMe:
					flag2 = true;
					break;
				case enSkillType.DoubleTeam:
					flag3 = true;
					break;
				}
			}
			if (flag2 || flag3)
			{
				InputController inputController2 = player2.InputController;
				bool bFire = ((TopWatchingInputController)inputController2).bFire;
				bool isRunning = player2.IsRunning;
				if (!bFire && isRunning)
				{
					Vector3 eulerAngles = player.GetTransform().eulerAngles;
					if (flag2)
					{
						Vector3 eulerAngles2 = new Vector3(eulerAngles.x, player2.GetTransform().eulerAngles.y + 180f, eulerAngles.z);
						player.GetTransform().eulerAngles = eulerAngles2;
					}
					else if (flag3)
					{
						Vector3 eulerAngles3 = new Vector3(eulerAngles.x, player2.GetTransform().eulerAngles.y, eulerAngles.z);
						player.GetTransform().eulerAngles = eulerAngles3;
					}
				}
				else
				{
					if (bFire && isRunning)
					{
						player.SetState(Player.RUNSHOOT_STATE);
						return;
					}
					if (!bFire && !isRunning)
					{
						float num = Vector2.Distance(new Vector2(player.friendMoveTarget.x, player.friendMoveTarget.z), new Vector2(player.GetTransform().position.x, player.GetTransform().position.z));
						if (num < 0.2f)
						{
							player.SetState(Player.IDLE_STATE);
							return;
						}
					}
					if (bFire && !isRunning)
					{
						player.SetState(Player.SHOOT_STATE);
						return;
					}
				}
				Vector3 vector = player.friendMoveTarget - player.GetTransform().position;
				if (vector.y != 0f)
				{
					vector = new Vector3(vector.x, 0f, vector.z);
				}
				vector.Normalize();
				player.Move((vector + Physics.gravity * deltaTime) * (deltaTime * player2.WalkSpeed));
				string text2 = "Run__Two";
				if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
				{
					text2 = "Run__Shotgun";
				}
				else if (player.WeaponNameEnd == "_RPG")
				{
					text2 = "Run__RPG";
				}
				bool flag4 = player.IsPlayingAnimation(text2);
				bool flag5 = player.AnimationEnds(text2);
				if ((flag4 && flag5) || !flag4)
				{
					player.Animate(text2, WrapMode.Loop);
				}
				return;
			}
			if (player.friendTargetEnemy != null)
			{
				player.SetState(Player.RUNSHOOT_STATE);
				return;
			}
			if (player2.IsRunning && Vector3.Distance(player2.GetTransform().position, player.GetTransform().position) <= GameApp.GetInstance().GetGameScene().GetGameParameters()
				.PlayersDistance)
			{
				player.SetState(Player.IDLE_STATE);
				return;
			}
			float num2 = Vector2.Distance(new Vector2(player.friendMoveTarget.x, player.friendMoveTarget.z), new Vector2(player.GetTransform().position.x, player.GetTransform().position.z));
			if (num2 < 0.2f)
			{
				player.SetState(Player.IDLE_STATE);
				return;
			}
			Vector3 vector2 = player.friendMoveTarget - player.GetTransform().position;
			if (vector2.y != 0f)
			{
				vector2 = new Vector3(vector2.x, 0f, vector2.z);
			}
			vector2.Normalize();
			player.Move((vector2 + Physics.gravity * deltaTime) * (deltaTime * player.WalkSpeed * 0.75f));
			player.GetTransform().LookAt(new Vector3(player.friendMoveTarget.x, player.GetTransform().position.y, player.friendMoveTarget.z));
			string text3 = "Run__Two";
			if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
			{
				text3 = "Run__Shotgun";
			}
			else if (player.WeaponNameEnd == "_RPG")
			{
				text3 = "Run__RPG";
			}
			bool flag6 = player.IsPlayingAnimation(text3);
			bool flag7 = player.AnimationEnds(text3);
			if ((flag6 && flag7) || !flag6)
			{
				player.Animate(text3, WrapMode.Loop);
			}
		}
	}
}
