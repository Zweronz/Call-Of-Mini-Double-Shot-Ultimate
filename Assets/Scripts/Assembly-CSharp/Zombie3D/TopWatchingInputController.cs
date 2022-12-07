using UnityEngine;

namespace Zombie3D
{
	public class TopWatchingInputController : InputController
	{
		public bool bFire;

		public override void ProcessInput(float deltaTime, InputInfo inputInfo)
		{
			Weapon weapon = player.GetWeapon();
			GameObject playerObject = player.PlayerObject;
			Vector3 getHitFlySpeed = player.GetHitFlySpeed;
			Transform respawnTransform = player.GetRespawnTransform();
			if (bFire)
			{
				inputInfo.fire = true;
			}
			inputInfo.moveDirection = moveDirection;
			player.GetTransform().localRotation = Quaternion.Lerp(player.GetTransform().localRotation, Quaternion.Euler(shootDirection), 0.5f);
			if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
			{
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			}
			float walkSpeed = player.WalkSpeed;
			moveDirection = respawnTransform.TransformDirection(moveDirection);
			getHitFlySpeed.x = Mathf.Lerp(getHitFlySpeed.x, 0f, 5f * Time.deltaTime);
			getHitFlySpeed.y = Mathf.Lerp(getHitFlySpeed.y, 0f, (0f - Physics.gravity.y) * Time.deltaTime);
			getHitFlySpeed.z = Mathf.Lerp(getHitFlySpeed.z, 0f, 5f * Time.deltaTime);
			if (!player.Faint)
			{
				player.Move((moveDirection + getHitFlySpeed) * (deltaTime * walkSpeed));
			}
			if (Input.GetAxis("Weapon2") != 0f && !Enemy.m_bShowDebugInfo)
			{
				Enemy.m_bShowDebugInfo = true;
			}
			if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f || touchX != 0f || touchY != 0f || moveDirection.x != 0f || moveDirection.z != 0f)
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
