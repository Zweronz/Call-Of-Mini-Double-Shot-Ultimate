using System.Collections.Generic;
using UnityEngine;

public class BulletMachine : BaseMachine
{
	private List<GameObject> bullets;

	private Vector3 bulletPosOffset = new Vector3(0f, 0.08410245f, 0.7859242f);

	private float m_fInterval = 0.8f;

	private float m_lastFireTime;

	private float m_fFiredNum;

	public override MachineType GetMachineType()
	{
		return MachineType.E_BULLET;
	}

	public override void Init()
	{
		base.Init();
		m_fSkillCDTime = 5f;
		bullets = new List<GameObject>();
		for (int i = 0; i < 30; i++)
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
		if (m_fFiredNum >= 5f)
		{
			ResetCD();
			m_fFiredNum = 0f;
		}
		if (Time.time - m_lastFireTime >= m_fInterval)
		{
			int num = 0;
			if (num < bullets.Count / 5 && bullets[num] == null)
			{
				GameObject original = Resources.Load("Zombie3D/Misc/MachineBullet") as GameObject;
				Vector3 position = m_goMachineBulletObj.transform.TransformPoint(bulletPosOffset);
				GameObject gameObject = Object.Instantiate(original, position, Quaternion.Euler(270f, m_goMachineBulletObj.transform.rotation.eulerAngles.y + 180f, 0f)) as GameObject;
				float y = m_goMachineBulletObj.transform.localEulerAngles.y;
				gameObject.transform.Rotate(Vector3.forward, y);
				CommonEnemyBulletScript commonEnemyBulletScript = gameObject.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
				commonEnemyBulletScript.Speed = 3.8f;
				commonEnemyBulletScript.Damage = 10f;
				commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.up;
				m_goMachineObj.GetComponent<Animation>()["fire"].wrapMode = WrapMode.Once;
				m_goMachineObj.GetComponent<Animation>().Play("fire");
				m_lastFireTime = Time.time;
				m_fFiredNum += 1f;
			}
		}
	}
}
