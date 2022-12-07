using UnityEngine;

namespace Zombie3D
{
	public class PlayerShootState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.m_bFirstShoot = true;
			player.m_LastShootBeginTime = Time.time;
			player.m_TwoHandGunAnimSpecialCounter = 0;
			player.m_bShooting = false;
		}

		public override void OnExit(Player player)
		{
			player.m_bFirstShoot = true;
			player.m_LastShootBeginTime = 0f;
			player.m_TwoHandGunAnimSpecialCounter = 0;
			player.m_bShooting = false;
		}

		public override void NextState(Player player, float deltaTime)
		{
			if (!player.FriendPlayer)
			{
				InputController inputController = player.InputController;
				InputInfo inputInfo = new InputInfo();
				inputController.ProcessInput(deltaTime, inputInfo);
				Weapon weapon = player.GetWeapon();
				if (weapon != null)
				{
					if (!weapon.HaveBullets())
					{
						player.SetState(Player.IDLE_STATE);
					}
					else if (!weapon.CouldMakeNextShoot())
					{
						weapon.FireUpdate(deltaTime);
					}
					else if (inputInfo.fire && !inputInfo.IsMoving())
					{
						weapon.FireUpdate(deltaTime);
						player.Fire(deltaTime);
						if (player.GetWeapon().GetWeaponType() == WeaponType.Hellfire)
						{
							player.Animate("Idle01_RPG", WrapMode.Loop);
							return;
						}
						if (player.WeaponNameEnd == "_Two")
						{
							player.m_TwoHandGunAnimSpecialCounter++;
							if (player.m_TwoHandGunAnimSpecialCounter % 2 == 1)
							{
								float length = player.PlayerObject.GetComponent<Animation>()["Shoot01" + player.WeaponNameEnd].length;
								float num = length / (player.GetWeapon().AttackFrequency * 2f);
								player.PlayerObject.GetComponent<Animation>()["Shoot01" + player.WeaponNameEnd].speed = num;
								player.m_LastShootBeginTime = Time.time;
								player.PlayerObject.GetComponent<Animation>().Stop("Shoot01" + player.WeaponNameEnd);
								if (player.m_bFirstShoot)
								{
									player.m_bFirstShoot = false;
									player.PlayAnim("Shoot01" + player.WeaponNameEnd, WrapMode.Once, num);
								}
								else
								{
									player.Animate("Shoot01" + player.WeaponNameEnd, WrapMode.Loop, 0f, num);
								}
							}
							return;
						}
						string text = "Shoot01" + player.WeaponNameEnd;
						if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
						{
							text = "Shoot01_Shotgun";
						}
						float length2 = player.PlayerObject.GetComponent<Animation>()[text].length;
						if (player.GetWeapon().AttackFrequency > length2)
						{
							player.PlayerObject.GetComponent<Animation>()[text].speed = 1f;
							float num2 = player.GetWeapon().AttackFrequency - length2;
							float fadeTime = ((!(num2 > 0.3f)) ? num2 : 0.3f);
							player.PlayerObject.GetComponent<Animation>().Stop(text);
							player.Animate(text, WrapMode.Once, fadeTime);
						}
						else
						{
							float num3 = length2 / player.GetWeapon().AttackFrequency;
							if (num3 > 3f)
							{
								num3 = 3f;
							}
							player.PlayerObject.GetComponent<Animation>()[text].speed = num3;
							player.PlayerObject.GetComponent<Animation>().Stop(text);
							if (player.m_bFirstShoot)
							{
								player.m_bFirstShoot = false;
								player.PlayAnim(text, WrapMode.Once, num3);
							}
							else
							{
								player.Animate(text, WrapMode.Once);
							}
						}
						player.m_LastShootBeginTime = Time.time;
						return;
					}
				}
				string name = "Shoot01" + player.WeaponNameEnd;
				if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
				{
					name = "Shoot01_Shotgun";
				}
				bool flag = player.IsPlayingAnimation(name);
				bool flag2 = player.AnimationEnds(name);
				float attackFrequency = player.GetWeapon().AttackFrequency;
				if (player.WeaponNameEnd == "_Two")
				{
					attackFrequency = player.GetWeapon().AttackFrequency * 2f;
				}
				float num4 = player.PlayerObject.GetComponent<Animation>()[name].length / player.PlayerObject.GetComponent<Animation>()[name].speed;
				if (!inputInfo.fire && !inputInfo.IsMoving())
				{
					player.SetState(Player.IDLE_STATE);
					player.StopFire();
				}
				else if (inputInfo.fire && inputInfo.IsMoving())
				{
					player.SetState(Player.RUNSHOOT_STATE);
				}
				else if (!inputInfo.fire && inputInfo.IsMoving())
				{
					player.SetState(Player.RUN_STATE);
					player.StopFire();
				}
				else if (!(player.WeaponNameEnd == "_Two") && !(player.GetWeapon().AttackFrequency <= num4) && player.GetWeapon().AttackFrequency > num4)
				{
					bool flag3 = player.IsPlayingAnimation("Idle01" + player.WeaponNameEnd);
					bool flag4 = player.AnimationEnds("Idle01" + player.WeaponNameEnd);
					if ((flag3 && flag4) || !flag3)
					{
						player.Animate("Idle01" + player.WeaponNameEnd, WrapMode.Loop);
					}
				}
				return;
			}
			Player player2 = GameApp.GetInstance().GetGameScene().GetPlayer();
			bool flag5 = false;
			bool flag6 = false;
			if (player2.ActiveSkillImpl != null)
			{
				switch (player2.ActiveSkillImpl.GetSkill().SkillType)
				{
				case enSkillType.CoverMe:
					flag5 = true;
					break;
				case enSkillType.DoubleTeam:
					flag6 = true;
					break;
				}
			}
			if (flag5 || flag6)
			{
				InputController inputController2 = player2.InputController;
				bool bFire = ((TopWatchingInputController)inputController2).bFire;
				bool isRunning = player2.IsRunning;
				if (bFire && !isRunning)
				{
					Vector3 eulerAngles = player.GetTransform().eulerAngles;
					if (flag5)
					{
						Vector3 eulerAngles2 = new Vector3(eulerAngles.x, player2.GetTransform().eulerAngles.y + 180f, eulerAngles.z);
						player.GetTransform().eulerAngles = eulerAngles2;
					}
					else if (flag6)
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
						player.SetState(Player.IDLE_STATE);
						return;
					}
					if (!bFire && isRunning)
					{
						player.SetState(Player.RUN_STATE);
						return;
					}
				}
				bool flag7 = player.IsPlayingAnimation("RunShoot01" + player.WeaponNameEnd);
				bool flag8 = player.AnimationEnds("RunShoot01" + player.WeaponNameEnd);
				if ((flag7 && flag8) || !flag7)
				{
					float num5 = 0f;
					if (player.WeaponNameEnd == "_Two")
					{
						float length3 = player.PlayerObject.GetComponent<Animation>()["Shoot01" + player.WeaponNameEnd].length;
						num5 = length3 / (player.GetWeapon().AttackFrequency * 2f);
						player.PlayerObject.GetComponent<Animation>()["Shoot01" + player.WeaponNameEnd].speed = num5;
					}
					player.Animate("Shoot01" + player.WeaponNameEnd, WrapMode.Loop, 0.3f, num5);
				}
				Weapon weapon2 = player.GetWeapon();
				weapon2.FireUpdate(deltaTime);
				if (weapon2.CouldMakeNextShoot())
				{
					player.Fire(deltaTime);
				}
				return;
			}
			if (player.friendTargetEnemy == null)
			{
				player.SetState(Player.IDLE_STATE);
				return;
			}
			if (player.friendTargetEnemy.HP <= 0f)
			{
				player.friendTargetEnemy = null;
				player.SetState(Player.IDLE_STATE);
				return;
			}
			if (Vector3.Distance(player2.GetTransform().position, player.GetTransform().position) > GameApp.GetInstance().GetGameScene().GetGameParameters()
				.PlayersDistance)
			{
				player.SetState(Player.RUNSHOOT_STATE);
				return;
			}
			Enemy friendTargetEnemy = player.friendTargetEnemy;
			if (friendTargetEnemy.enemyObject != null)
			{
				player.GetTransform().LookAt(friendTargetEnemy.GetPosition());
			}
			bool flag9 = player.IsPlayingAnimation("RunShoot01" + player.WeaponNameEnd);
			bool flag10 = player.AnimationEnds("RunShoot01" + player.WeaponNameEnd);
			if ((flag9 && flag10) || !flag9)
			{
				float num6 = 1f;
				if (player.WeaponNameEnd == "_Two")
				{
					float length4 = player.PlayerObject.GetComponent<Animation>()["Shoot01" + player.WeaponNameEnd].length;
					num6 = length4 / (player.GetWeapon().AttackFrequency * 2f);
					player.PlayerObject.GetComponent<Animation>()["Shoot01" + player.WeaponNameEnd].speed = num6;
				}
				player.Animate("Shoot01" + player.WeaponNameEnd, WrapMode.Loop, 0.3f, num6);
			}
			Weapon weapon3 = player.GetWeapon();
			weapon3.FireUpdate(deltaTime);
			if (weapon3.CouldMakeNextShoot())
			{
				player.Fire(deltaTime);
			}
		}
	}
}
