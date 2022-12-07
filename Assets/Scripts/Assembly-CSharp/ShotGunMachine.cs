using System.Collections.Generic;
using UnityEngine;

public class ShotGunMachine : BaseMachine
{
	private List<GameObject> bullets;

	private Vector3 bulletPosOffset = new Vector3(0f, 0.06259137f, 0.7195923f);

	public override MachineType GetMachineType()
	{
		return MachineType.E_SHOTGUN;
	}

	public override void Init()
	{
		base.Init();
		m_fSkillCDTime = 5f;
		bullets = new List<GameObject>();
		for (int i = 0; i < 18; i++)
		{
			GameObject item = null;
			bullets.Add(item);
		}
	}

	public override void CatchingMode()
	{
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(new Ray(m_goMachineBulletObj.transform.position, m_goMachineBulletObj.transform.forward), out hitInfo, m_fMaxDistance, 10496) && hitInfo.collider.GetComponent<Collider>().gameObject.layer == 8)
		{
			m_eMachineState = MachineState.E_FIRING;
		}
	}

	public override void FiringMode()
	{
		for (int i = 0; i < bullets.Count; i++)
		{
			if (bullets[i] == null)
			{
				GameObject original = Resources.Load("Zombie3D/Misc/ShotGunBullet") as GameObject;
				Vector3 position = m_goMachineBulletObj.transform.TransformPoint(bulletPosOffset);
				float num = -30f;
				for (int j = 0; j < 3; j++)
				{
					GameObject gameObject = Object.Instantiate(original, position, Quaternion.Euler(270f, m_goMachineBulletObj.transform.rotation.eulerAngles.y + num, 0f)) as GameObject;
					float y = m_goMachineBulletObj.transform.localEulerAngles.y;
					gameObject.transform.Rotate(Vector3.forward, y);
					CommonEnemyBulletScript commonEnemyBulletScript = gameObject.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
					commonEnemyBulletScript.Speed = 3.8f;
					commonEnemyBulletScript.Damage = 20f;
					commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.down;
					num += 30f;
				}
				m_goMachineObj.GetComponent<Animation>()["fire"].wrapMode = WrapMode.Once;
				m_goMachineObj.GetComponent<Animation>().Play("fire");
				break;
			}
		}
		ResetCD();
	}
}
