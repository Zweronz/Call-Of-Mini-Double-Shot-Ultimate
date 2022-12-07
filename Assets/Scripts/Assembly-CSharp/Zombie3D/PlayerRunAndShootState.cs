using UnityEngine;

namespace Zombie3D
{
	public class PlayerRunAndShootState : PlayerState
	{
		public override void OnEnter(Player player)
		{
			player.m_bFirstShoot = true;
			player.m_bRunAndShooting = false;
			player.m_LastShootBeginTime = 0f;
			if (player.WeaponNameEnd == "_Two")
			{
				player.m_TwoHandGunAnimSpecialCounter = 0;
			}
			else
			{
				player.m_TwoHandGunAnimSpecialCounter = 1;
			}
		}

		public override void OnExit(Player player)
		{
			player.m_bFirstShoot = true;
			player.m_bRunAndShooting = false;
			player.m_LastShootBeginTime = 0f;
			player.m_TwoHandGunAnimSpecialCounter = 0;
			player.PlayerObject.GetComponent<Animation>()["Shoot__Two"].layer = 0;
			player.PlayerObject.GetComponent<Animation>()["Shoot__Shotgun"].layer = 0;
			player.PlayerObject.GetComponent<Animation>()["Shoot__RPG"].layer = 0;
			player.PlayerObject.GetComponent<Animation>().Stop("Shoot__Two");
			player.PlayerObject.GetComponent<Animation>().Stop("Shoot__Shotgun");
			player.PlayerObject.GetComponent<Animation>().Stop("Shoot__RPG");
			player.PlayerObject.GetComponent<Animation>().Stop("Run__Two");
			player.PlayerObject.GetComponent<Animation>().Stop("Run__Shotgun");
			player.PlayerObject.GetComponent<Animation>().Stop("Run__RPG");
		}

		public override void NextState(Player player, float deltaTime)
		{
			if (!player.FriendPlayer)
			{
				InputController inputController = player.InputController;
				InputInfo inputInfo = new InputInfo();
				inputController.ProcessInput(deltaTime, inputInfo);
				if (player.m_TwoHandGunAnimSpecialCounter > 0)
				{
					string name = "Shoot__Two";
					if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
					{
						name = "Shoot__Shotgun";
					}
					else if (player.WeaponNameEnd == "_RPG")
					{
						name = "Shoot__RPG";
					}
					bool flag = player.IsPlayingAnimation(name);
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
				string text2 = "Shoot__Two";
				if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
				{
					text2 = "Shoot__Shotgun";
				}
				else if (player.WeaponNameEnd == "_RPG")
				{
					text2 = "Shoot__RPG";
				}
				Weapon weapon = player.GetWeapon();
				if (weapon != null)
				{
					weapon.FireUpdate(deltaTime);
					if (inputInfo.fire)
					{
						if (!weapon.HaveBullets())
						{
							player.SetState(Player.RUN_STATE);
						}
						else if (!weapon.CouldMakeNextShoot())
						{
							weapon.FireUpdate(deltaTime);
						}
						else if (inputInfo.fire && inputInfo.IsMoving())
						{
							player.Fire(deltaTime);
							if (player.GetWeapon().GetWeaponType() == WeaponType.Hellfire)
							{
								player.Animate("Run__RPG", WrapMode.Loop);
								return;
							}
							if (player.WeaponNameEnd == "_Two")
							{
								player.m_TwoHandGunAnimSpecialCounter++;
								if (player.m_TwoHandGunAnimSpecialCounter % 2 == 1)
								{
									player.PlayerObject.GetComponent<Animation>()[text].wrapMode = WrapMode.Loop;
									player.Animate(text, WrapMode.Loop);
									string name2 = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1";
									player.PlayerObject.GetComponent<Animation>()[text2].AddMixingTransform(player.GetTransform().Find(name2));
									float length = player.PlayerObject.GetComponent<Animation>()[text2].length;
									float speed = length / (player.GetWeapon().AttackFrequency * 2f);
									player.PlayerObject.GetComponent<Animation>()[text2].speed = speed;
									player.PlayerObject.GetComponent<Animation>().Stop(text2);
									player.PlayerObject.GetComponent<Animation>()[text2].wrapMode = WrapMode.Loop;
									player.PlayerObject.GetComponent<Animation>()[text2].layer = 1;
									player.PlayAnim(text2, WrapMode.Loop, speed);
									player.m_LastShootBeginTime = Time.time;
								}
								return;
							}
							float length2 = player.PlayerObject.GetComponent<Animation>()[text2].length;
							player.PlayerObject.GetComponent<Animation>()[text].wrapMode = WrapMode.Loop;
							player.PlayerObject.GetComponent<Animation>()[text].layer = 0;
							if (player.m_bFirstShoot)
							{
								player.m_bFirstShoot = false;
								player.PlayAnim(text, WrapMode.Loop);
							}
							else
							{
								player.Animate(text, WrapMode.Loop);
							}
							length2 = player.PlayerObject.GetComponent<Animation>()[text2].length;
							string name3 = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1";
							player.PlayerObject.GetComponent<Animation>()[text2].AddMixingTransform(player.GetTransform().Find(name3));
							player.PlayerObject.GetComponent<Animation>()[text2].wrapMode = WrapMode.Loop;
							float num = 0f;
							float num2 = 0.2f;
							float num3 = player.GetWeapon().AttackFrequency;
							if (num3 < num2)
							{
								num3 = num2;
							}
							num = length2 / num3;
							if (num < 1f)
							{
								if (num3 == num2)
								{
									if (num < 0.5f)
									{
										num = 0.5f;
									}
								}
								else
								{
									num = 1f;
								}
							}
							if (num > 3f)
							{
								num = 3f;
							}
							player.PlayerObject.GetComponent<Animation>()[text2].speed = num;
							player.PlayerObject.GetComponent<Animation>()[text2].layer = 1;
							player.m_LastShootBeginTime = Time.time;
							player.PlayAnim(text2, WrapMode.Loop, num);
							player.m_bRunAndShooting = true;
							return;
						}
					}
				}
				bool flag2 = player.IsPlayingAnimation(text2);
				bool flag3 = player.AnimationEnds(text2);
				float num4 = player.GetWeapon().AttackFrequency;
				if (player.WeaponNameEnd == "_Two")
				{
					num4 = player.GetWeapon().AttackFrequency * 2f;
				}
				float num5 = player.PlayerObject.GetComponent<Animation>()[text2].length / player.PlayerObject.GetComponent<Animation>()[text2].speed;
				if (!(Time.time - player.m_LastShootBeginTime > num4) && !(Time.time - player.m_LastShootBeginTime > num5))
				{
					return;
				}
				if (!inputInfo.fire && !inputInfo.IsMoving())
				{
					player.StopFire();
					player.SetState(Player.IDLE_STATE);
				}
				else if (!inputInfo.fire && inputInfo.IsMoving())
				{
					player.StopFire();
					player.SetState(Player.RUN_STATE);
				}
				else if (inputInfo.fire && !inputInfo.IsMoving())
				{
					player.SetState(Player.SHOOT_STATE);
				}
				else if (!(player.GetWeapon().AttackFrequency <= num5) && (!(player.GetWeapon().AttackFrequency > num5) || !(player.GetWeapon().AttackFrequency - num5 <= 0.1f)))
				{
					string text3 = "Run__Two";
					if (player.WeaponNameEnd == string.Empty || player.WeaponNameEnd == "_Shotgun")
					{
						text3 = "Run__Shotgun";
					}
					else if (player.WeaponNameEnd == "_RPG")
					{
						text3 = "Run__RPG";
					}
					bool flag4 = player.IsPlayingAnimation(text3);
					bool flag5 = player.AnimationEnds(text3);
					if ((flag4 && flag5) || !flag4)
					{
						player.PlayerObject.GetComponent<Animation>()["Shoot__Two"].layer = 0;
						player.PlayerObject.GetComponent<Animation>()["Shoot__Shotgun"].layer = 0;
						player.PlayerObject.GetComponent<Animation>()["Shoot__RPG"].layer = 0;
						player.PlayerObject.GetComponent<Animation>().Stop("Shoot__Two");
						player.PlayerObject.GetComponent<Animation>().Stop("Shoot__Shotgun");
						player.PlayerObject.GetComponent<Animation>().Stop("Shoot__RPG");
						player.PlayerObject.GetComponent<Animation>()[text3].wrapMode = WrapMode.Loop;
						player.PlayerObject.GetComponent<Animation>().CrossFade(text3);
					}
				}
				return;
			}
			Player player2 = GameApp.GetInstance().GetGameScene().GetPlayer();
			bool flag6 = false;
			bool flag7 = false;
			if (player2.ActiveSkillImpl != null)
			{
				switch (player2.ActiveSkillImpl.GetSkill().SkillType)
				{
				case enSkillType.CoverMe:
					flag6 = true;
					break;
				case enSkillType.DoubleTeam:
					flag7 = true;
					break;
				}
			}
			if (flag6 || flag7)
			{
				InputController inputController2 = player2.InputController;
				bool bFire = ((TopWatchingInputController)inputController2).bFire;
				bool isRunning = player2.IsRunning;
				if (bFire && isRunning)
				{
					Vector3 eulerAngles = player.GetTransform().eulerAngles;
					if (flag6)
					{
						Vector3 eulerAngles2 = new Vector3(eulerAngles.x, player2.GetTransform().eulerAngles.y + 180f, eulerAngles.z);
						player.GetTransform().eulerAngles = eulerAngles2;
					}
					else if (flag7)
					{
						Vector3 eulerAngles3 = new Vector3(eulerAngles.x, player2.GetTransform().eulerAngles.y, eulerAngles.z);
						player.GetTransform().eulerAngles = eulerAngles3;
					}
				}
				else
				{
					if (bFire && !isRunning)
					{
						player.SetState(Player.SHOOT_STATE);
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
				Vector3 vector = player.friendMoveTarget - player.GetTransform().position;
				if (vector.y != 0f)
				{
					vector = new Vector3(vector.x, 0f, vector.z);
				}
				vector.Normalize();
				player.Move((vector + Physics.gravity * deltaTime) * (deltaTime * player2.WalkSpeed));
				bool flag8 = player.IsPlayingAnimation("RunShoot01" + player.WeaponNameEnd);
				bool flag9 = player.AnimationEnds("RunShoot01" + player.WeaponNameEnd);
				float num6 = 0f;
				if ((flag8 && flag9) || !flag8)
				{
					if (player.WeaponNameEnd == "_Two")
					{
						float length3 = player.PlayerObject.GetComponent<Animation>()["RunShoot01" + player.WeaponNameEnd].length;
						num6 = length3 / (player.GetWeapon().AttackFrequency * 2f);
						player.PlayerObject.GetComponent<Animation>()["RunShoot01" + player.WeaponNameEnd].speed = num6;
					}
					player.Animate("RunShoot01" + player.WeaponNameEnd, WrapMode.Loop, 0.3f, num6);
				}
				Weapon weapon2 = player.GetWeapon();
				if (weapon2.CouldMakeNextShoot())
				{
					player.Fire(deltaTime);
				}
				return;
			}
			if (player.friendTargetEnemy == null)
			{
				if (Vector3.Distance(player2.GetTransform().position, player.GetTransform().position) > GameApp.GetInstance().GetGameScene().GetGameParameters()
					.PlayersDistance)
				{
					player.SetState(Player.RUN_STATE);
				}
				else
				{
					player.SetState(Player.IDLE_STATE);
				}
				return;
			}
			if (player2.IsRunning && Vector3.Distance(player2.GetTransform().position, player.GetTransform().position) <= GameApp.GetInstance().GetGameScene().GetGameParameters()
				.PlayersDistance)
			{
				player.SetState(Player.SHOOT_STATE);
				return;
			}
			float num7 = Vector2.Distance(new Vector2(player.friendMoveTarget.x, player.friendMoveTarget.z), new Vector2(player.GetTransform().position.x, player.GetTransform().position.z));
			if (num7 < 0.2f)
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
			Enemy friendTargetEnemy = player.friendTargetEnemy;
			if (friendTargetEnemy.enemyObject != null)
			{
				player.GetTransform().LookAt(new Vector3(friendTargetEnemy.GetPosition().x, player.GetTransform().position.y, friendTargetEnemy.GetPosition().z));
			}
			bool flag10 = player.IsPlayingAnimation("RunShoot01" + player.WeaponNameEnd);
			bool flag11 = player.AnimationEnds("RunShoot01" + player.WeaponNameEnd);
			float num8 = 0f;
			if ((flag10 && flag11) || !flag10)
			{
				if (player.WeaponNameEnd == "_Two")
				{
					float length4 = player.PlayerObject.GetComponent<Animation>()["RunShoot01" + player.WeaponNameEnd].length;
					num8 = length4 / (player.GetWeapon().AttackFrequency * 2f);
					player.PlayerObject.GetComponent<Animation>()["RunShoot01" + player.WeaponNameEnd].speed = num8;
				}
				player.Animate("RunShoot01" + player.WeaponNameEnd, WrapMode.Loop, 0.3f, num8);
			}
			Weapon weapon3 = player.GetWeapon();
			if (weapon3.CouldMakeNextShoot())
			{
				player.Fire(deltaTime);
			}
		}
	}
}
