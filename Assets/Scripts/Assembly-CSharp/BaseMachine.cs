using UnityEngine;

public abstract class BaseMachine : MonoBehaviour
{
	protected MachineType m_eMachineType;

	protected MachineState m_eMachineState = MachineState.E_CATCHING;

	public GameObject m_goMachineObj;

	public Renderer m_rMachineRender;

	public Transform m_goMachineTrans;

	public GameObject m_goMachineBulletObj;

	public bool m_bIsWork;

	public float m_fMoveSpeed = 1f;

	public float m_RoateAngle = 44f;

	private float m_fSkillCDTimer = -1f;

	protected float m_fSkillCDTime = 12f;

	protected string m_strBeginAniamtionName = "open";

	protected float m_fMaxDistance = 25f;

	private float m_fBeginAnimationTimer = -1f;

	private float m_fBeginAnimationTime = 1f;

	private float lastUpdateTime;

	private float m_fOneRound = 360f;

	private bool m_bIsSpecial;

	private bool m_bWay = true;

	private Vector3 m_Vec3BeginRotat = Vector3.zero;

	private Vector3 m_Vec3EndRotat = Vector3.zero;

	public abstract MachineType GetMachineType();

	public virtual void Init()
	{
		m_goMachineObj = base.gameObject;
		m_goMachineTrans = m_goMachineObj.transform;
		m_rMachineRender = m_goMachineObj.transform.Find("Root_Bone").GetComponent<Renderer>();
		m_goMachineBulletObj = m_goMachineObj.transform.Find("Bullet").gameObject;
		m_fBeginAnimationTime = m_goMachineObj.GetComponent<Animation>()[m_strBeginAniamtionName].length;
		if (m_goMachineTrans.rotation.eulerAngles.y > m_fOneRound - m_RoateAngle && Mathf.Abs(360f - m_goMachineTrans.rotation.eulerAngles.y) < m_RoateAngle)
		{
			m_fOneRound = 360f;
		}
		else if (m_goMachineTrans.rotation.eulerAngles.y < m_RoateAngle * 2f && m_goMachineTrans.rotation.eulerAngles.y >= 0f)
		{
			m_fOneRound = -360f;
		}
		m_Vec3BeginRotat = new Vector3(0f, m_goMachineTrans.rotation.eulerAngles.y - m_RoateAngle, 0f);
		m_Vec3EndRotat = new Vector3(0f, m_goMachineTrans.rotation.eulerAngles.y + m_RoateAngle, 0f);
		m_fMoveSpeed = 0.5f;
		m_goMachineBulletObj.SetActiveRecursively(false);
	}

	private void Update()
	{
		if (Time.time - lastUpdateTime < 0.001f)
		{
			return;
		}
		lastUpdateTime = Time.time;
		if (m_bIsWork && PlayBeginAnimation())
		{
			switch (m_eMachineState)
			{
			case MachineState.E_CD:
				SkillCD();
				break;
			case MachineState.E_CATCHING:
				MoveAround();
				CatchingMode();
				break;
			case MachineState.E_FIRING:
				ReadyMode();
				FiringMode();
				break;
			}
		}
	}

	private void MoveAround()
	{
		Quaternion identity = Quaternion.identity;
		float num = m_goMachineTrans.rotation.eulerAngles.y;
		if (m_bIsSpecial)
		{
			num += m_fOneRound;
		}
		if (num < m_Vec3BeginRotat.y)
		{
			m_bWay = true;
			num = m_Vec3BeginRotat.y;
		}
		else if (num > m_Vec3EndRotat.y)
		{
			m_bWay = false;
			num = m_Vec3EndRotat.y;
		}
		num = ((!m_bWay) ? (num - m_fMoveSpeed) : (num + m_fMoveSpeed));
		identity.eulerAngles = new Vector3(0f, num, 0f);
		m_goMachineTrans.rotation = identity;
		if (Mathf.Abs(num - m_goMachineTrans.rotation.eulerAngles.y) > 0.01f)
		{
			m_bIsSpecial = true;
		}
		else
		{
			m_bIsSpecial = false;
		}
	}

	public virtual void CatchingMode()
	{
	}

	public virtual void ReadyMode()
	{
	}

	public virtual void FiringMode()
	{
	}

	private void SkillCD()
	{
		if (m_fSkillCDTimer > 0f)
		{
			m_fSkillCDTimer -= Time.deltaTime;
		}
		if (m_fSkillCDTimer <= 0f)
		{
			m_eMachineState = MachineState.E_CATCHING;
		}
	}

	public void ResetCD()
	{
		m_fSkillCDTimer = m_fSkillCDTime;
		m_eMachineState = MachineState.E_CD;
		m_goMachineBulletObj.SetActiveRecursively(false);
	}

	private bool PlayBeginAnimation()
	{
		if (m_fBeginAnimationTimer >= 9999f)
		{
			return true;
		}
		if (m_fBeginAnimationTimer < 0f && m_rMachineRender.isVisible)
		{
			m_fBeginAnimationTimer = 0f;
			m_goMachineObj.GetComponent<Animation>()[m_strBeginAniamtionName].wrapMode = WrapMode.Once;
			m_goMachineObj.GetComponent<Animation>().Play(m_strBeginAniamtionName);
		}
		if (m_fBeginAnimationTimer >= 0f)
		{
			m_fBeginAnimationTimer += Time.deltaTime;
		}
		if (m_fBeginAnimationTimer >= m_fBeginAnimationTime)
		{
			m_fBeginAnimationTimer = 9999f;
			m_goMachineBulletObj.SetActiveRecursively(true);
			BeginAnimationEnd();
			return true;
		}
		return false;
	}

	public virtual void BeginAnimationEnd()
	{
	}
}
