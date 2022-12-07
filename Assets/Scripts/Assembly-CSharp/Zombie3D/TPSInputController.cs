using UnityEngine;

namespace Zombie3D
{
	public class TPSInputController : InputController
	{
		public override void ProcessInput(float deltaTime, InputInfo inputInfo)
		{
			Weapon weapon = player.GetWeapon();
			GameObject playerObject = player.PlayerObject;
			Transform transform = player.GetTransform();
			Vector3 getHitFlySpeed = player.GetHitFlySpeed;
			if (!Application.isMobilePlatform)
			{
				if (base.EnableShootingInput)
				{
					if (Input.GetButton("Fire1"))
					{
						inputInfo.fire = true;
					}
					else
					{
						inputInfo.stopFire = true;
					}
				}
				if (base.EnableMoveInput)
				{
					moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
				}
				if (base.EnableTurningAround)
				{
					cameraRotation.x = player.InputController.CameraRotation.x;
					cameraRotation.y = player.InputController.CameraRotation.y;
				}
			}
			else
			{
				touchX = 0f;
				touchY = 0f;
				cameraRotation.x = 0f;
				cameraRotation.y = 0f;
				bool flag = false;
				for (int i = 0; i < Input.touchCount && i != 2; i++)
				{
					int theMoment = 0;
					Touch touch = new Touch();
					if (Application.isMobilePlatform)
					{
						touch = Input.GetTouch(i);
					}
					else
					{
						foreach (Touch the in InputHelper.GetTouches())
						{
							if(theMoment==1)
							continue;
							touch = the;
							theMoment++;
						}
					}
					if (touch.tapCount == 0)
					{
						thumbTouchFingerId = -1;
						shootingTouchFingerId = -1;
					}
					phaseStr = touch.phase.ToString() + touch.fingerId + " p:" + touch.position.x + "," + touch.position.y;
					Vector2 vector = touch.position - thumbCenterToScreen;
					bool flag2 = vector.sqrMagnitude < thumbRadius * thumbRadius;
					bool flag3 = touch.fingerId == thumbTouchFingerId;
					if (touch.phase == TouchPhase.Stationary)
					{
						if (flag2 || flag3)
						{
							if (flag2)
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
							}
							else
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
								if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
								{
									touchY /= Mathf.Abs(touchX);
									touchX = ((touchX > 0f) ? 1 : (-1));
								}
								else if (touchY != 0f)
								{
									touchX /= Mathf.Abs(touchY);
									touchY = ((touchY > 0f) ? 1 : (-1));
								}
								else
								{
									touchX = 0f;
									touchY = 0f;
								}
							}
							thumbTouchFingerId = touch.fingerId;
						}
						else if (base.EnableShootingInput)
						{
							Vector2 vector2 = touch.position - shootThumbCenterToScreen;
							bool flag4 = vector2.sqrMagnitude < thumbRadius * thumbRadius;
							if (flag4 || shootingTouchFingerId == touch.fingerId)
							{
								if (flag4)
								{
									cameraRotation.x = Mathf.Clamp(vector2.x, 0f - thumbRadius, thumbRadius) * 0.005f;
									lastShootTouch = touch.position;
								}
								else
								{
									cameraRotation.x = Mathf.Sign(vector2.x) * thumbRadius * 0.01f;
									Vector2 normalized = (touch.position - shootThumbCenterToScreen).normalized;
									lastShootTouch = shootThumbCenterToScreen + normalized * thumbRadius;
								}
								inputInfo.fire = true;
								shootingTouchFingerId = touch.fingerId;
								flag = true;
							}
						}
					}
					else if (touch.phase == TouchPhase.Moved)
					{
						if (flag2 || flag3)
						{
							if (flag2)
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
							}
							else
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
								if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
								{
									touchY /= Mathf.Abs(touchX);
									touchX = ((touchX > 0f) ? 1 : (-1));
								}
								else if (touchY != 0f)
								{
									touchX /= Mathf.Abs(touchY);
									touchY = ((touchY > 0f) ? 1 : (-1));
								}
								else
								{
									touchX = 0f;
									touchY = 0f;
								}
							}
							thumbTouchFingerId = touch.fingerId;
						}
						else
						{
							if (base.EnableTurningAround)
							{
								cameraRotation.x = touch.deltaPosition.x * 0.4f;
								cameraRotation.y = touch.deltaPosition.y * 0.4f;
							}
							Vector2 vector3 = touch.position - shootThumbCenterToScreen;
							bool flag5 = vector3.sqrMagnitude < thumbRadius * thumbRadius;
							if (base.EnableShootingInput && (shootingTouchFingerId == touch.fingerId || flag5))
							{
								inputInfo.fire = true;
								flag = true;
								if (flag5)
								{
									cameraRotation.x += Mathf.Clamp(vector3.x, 0f - thumbRadius, thumbRadius) * 0.005f;
									lastShootTouch = touch.position;
								}
								else
								{
									Vector2 normalized2 = (touch.position - shootThumbCenterToScreen).normalized;
									lastShootTouch = shootThumbCenterToScreen + normalized2 * thumbRadius;
									cameraRotation.x = Mathf.Sign(vector3.x) * thumbRadius * 0.01f;
								}
								shootingTouchFingerId = touch.fingerId;
							}
						}
					}
					else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						if (touch.fingerId == thumbTouchFingerId)
						{
							thumbTouchFingerId = -1;
						}
						if (touch.fingerId == shootingTouchFingerId)
						{
							shootingTouchFingerId = -1;
							lastShootTouch = shootThumbCenterToScreen;
						}
					}
					lastTouch[i] = touch;
				}
				if (!flag)
				{
					inputInfo.stopFire = true;
				}
				touchX = Mathf.Clamp(touchX, -1f, 1f);
				touchY = Mathf.Clamp(touchY, -1f, 1f);
				moveDirection = new Vector3(touchX, 0f, touchY);
			}
			moveDirection = transform.TransformDirection(moveDirection);
			if (!base.EnableMoveInput)
			{
				moveDirection = Vector3.zero;
			}
			if (!base.EnableShootingInput)
			{
				inputInfo.fire = false;
			}
			moveDirection += Physics.gravity * deltaTime;
			inputInfo.moveDirection = moveDirection;
			if (Input.GetButton("K"))
			{
				player.OnHit(player.GetMaxHp());
			}
			if (Input.GetButton("H"))
			{
				player.GetHealed((int)player.GetMaxHp());
			}
			if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f || touchX != 0f || touchY != 0f)
			{
				player.Run();
			}
			else
			{
				player.StopRun();
			}
		}
	}
}
