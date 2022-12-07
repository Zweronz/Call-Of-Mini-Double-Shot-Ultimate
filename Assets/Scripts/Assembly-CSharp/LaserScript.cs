using UnityEngine;
using Zombie3D;

public class LaserScript : MonoBehaviour
{
	public enum MachineType
	{
		E_LASER = 0,
		E_BULLET = 1,
		E_SHOTGUN = 2
	}

	public MachineType m_eMachineType;

	public float m_fMoveSpeed = 1f;

	public float m_RoateAngle = 44f;

	public bool m_bWay = true;

	public bool m_bIsWork;

	public float m_MaxLineLength = 25f;

	public float m_LaserDamage = 10f;

	protected float lastUpdateTime;

	private GameObject m_GORayObj;

	private Renderer m_GORender;

	private Transform m_GORayTrans;

	private float m_fOneRound = 360f;

	private Vector3 m_Vec3BeginRotat = Vector3.zero;

	private Vector3 m_Vec3EndRotat = Vector3.zero;

	private bool m_bIsFindPlayer;

	private bool m_bAimAtTarget;

	private float m_fAnimReadyFireTime = 2f;

	private bool m_bIsAttack;

	private float m_fSkillCdTime = 12f;

	private bool m_bIsSpecial;

	private GameObject _GOCatchingLine;

	private GameObject _GOReadyLine;

	private GameObject _GOFireLine;

	private float m_LineLength = 20f;

	private float _SkillCDTimer = -1f;

	private float _SkillCDTime = 12f;

	private float _SkillReadyFireTimer = -1f;

	private float _SkillReadyFireTime = 2f;

	private float _SkillFiringTimer = -1f;

	private float _SkillFiringTime = 0.5f;

	private float hitTriggerLastTime;

	private float _CatchingLineOffset = 4.980061f;

	private float _FireLineOffset = 3.932f;

	private float _fIsFirstInitTimer = -1f;

	private float _fIsFirstInitTime = 1f;

	private void Start()
	{
		m_GORayObj = base.gameObject;
		m_GORayTrans = m_GORayObj.transform;
		m_GORender = m_GORayObj.transform.Find("Root_Bone").GetComponent<Renderer>();
		_fIsFirstInitTime = m_GORayObj.GetComponent<Animation>()["open"].length;
		if (m_GORayTrans.rotation.eulerAngles.y > m_fOneRound - m_RoateAngle && Mathf.Abs(360f - m_GORayTrans.rotation.eulerAngles.y) < m_RoateAngle)
		{
			m_fOneRound = 360f;
		}
		else if (m_GORayTrans.rotation.eulerAngles.y < m_RoateAngle * 2f && m_GORayTrans.rotation.eulerAngles.y >= 0f)
		{
			m_fOneRound = -360f;
		}
		m_Vec3BeginRotat = new Vector3(0f, m_GORayTrans.rotation.eulerAngles.y - m_RoateAngle, 0f);
		m_Vec3EndRotat = new Vector3(0f, m_GORayTrans.rotation.eulerAngles.y + m_RoateAngle, 0f);
		if (m_eMachineType == MachineType.E_LASER)
		{
			_GOCatchingLine = m_GORayObj.transform.Find("Bullet").Find("CatchLine").gameObject;
			_GOReadyLine = m_GORayObj.transform.Find("Bullet").Find("ReadyLine").gameObject;
			_GOFireLine = m_GORayObj.transform.Find("Bullet").Find("FireLine").gameObject;
			_FireLineOffset = Vector3.Distance(_GOFireLine.transform.Find("laser gun_open_01").position, _GOFireLine.transform.Find("laser gun_open_02").position);
			m_LaserDamage = GameApp.GetInstance().GetGameState().CalcMaxHp() / 10f;
			_GOCatchingLine.SetActiveRecursively(false);
			_GOReadyLine.SetActiveRecursively(false);
			_GOFireLine.SetActiveRecursively(false);
		}
		m_fMoveSpeed = 0.5f;
	}

	private void Update()
	{
		if (Time.time - lastUpdateTime < 0.001f)
		{
			return;
		}
		lastUpdateTime = Time.time;
		if (!m_bIsWork)
		{
			return;
		}
		if (_fIsFirstInitTimer < 0f && m_GORender.isVisible)
		{
			_fIsFirstInitTimer = 0f;
			m_GORayObj.GetComponent<Animation>()["open"].wrapMode = WrapMode.Once;
			m_GORayObj.GetComponent<Animation>().Play("open");
		}
		if (_fIsFirstInitTimer >= 0f)
		{
			_fIsFirstInitTimer += Time.deltaTime;
		}
		if (_fIsFirstInitTimer >= _fIsFirstInitTime + 0.2f && _fIsFirstInitTimer <= 10000f)
		{
			if (m_eMachineType == MachineType.E_LASER)
			{
				_GOCatchingLine.SetActiveRecursively(true);
				_GOReadyLine.SetActiveRecursively(false);
				_GOFireLine.SetActiveRecursively(false);
			}
			_fIsFirstInitTimer = 1111111f;
		}
		if (!m_bIsWork || _fIsFirstInitTimer != 1111111f)
		{
			return;
		}
		if (!IsCdIng())
		{
			if (m_eMachineType == MachineType.E_LASER)
			{
				OperativeMode();
			}
			MoveAround();
		}
		else if (m_eMachineType == MachineType.E_LASER)
		{
			SkillCD();
			_GOCatchingLine.SetActiveRecursively(false);
			_GOReadyLine.SetActiveRecursively(false);
			_GOFireLine.SetActiveRecursively(false);
		}
	}

	private void MoveAround()
	{
		if (!m_bIsFindPlayer)
		{
			Quaternion identity = Quaternion.identity;
			float num = m_GORayTrans.rotation.eulerAngles.y;
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
			m_GORayTrans.rotation = identity;
			if (Mathf.Abs(num - m_GORayTrans.rotation.eulerAngles.y) > 0.01f)
			{
				m_bIsSpecial = true;
			}
			else
			{
				m_bIsSpecial = false;
			}
		}
	}

	private void OperativeMode()
	{
		if (m_bIsFindPlayer)
		{
			FiringMode();
		}
		else
		{
			CatchingMode();
		}
	}

	private void CatchingMode()
	{
		_GOCatchingLine.SetActiveRecursively(true);
		_GOReadyLine.SetActiveRecursively(false);
		_GOFireLine.SetActiveRecursively(false);
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(new Ray(_GOCatchingLine.transform.position, -_GOCatchingLine.transform.right), out hitInfo, m_MaxLineLength, 10496))
		{
			_GOCatchingLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(true);
			m_LineLength = Vector3.Distance(hitInfo.point, _GOCatchingLine.transform.position);
			float x = m_LineLength / _CatchingLineOffset * 1f;
			_GOCatchingLine.transform.localScale = new Vector3(x, 1f, 1f);
			if (hitInfo.collider.GetComponent<Collider>().gameObject.layer == 8)
			{
				m_bIsFindPlayer = true;
				_SkillReadyFireTimer = 0f;
				float x2 = m_MaxLineLength / _FireLineOffset;
				_GOFireLine.transform.localScale = new Vector3(x2, 1f, 1f);
			}
		}
		else
		{
			_GOCatchingLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(false);
			m_LineLength = m_MaxLineLength;
			float x3 = m_LineLength / _CatchingLineOffset * 1f;
			_GOCatchingLine.transform.localScale = new Vector3(x3, 1f, 1f);
		}
	}

	private void FiringMode()
	{
		if (_GOFireLine.active)
		{
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(new Ray(_GOFireLine.transform.position, -_GOFireLine.transform.right), out hitInfo, m_MaxLineLength, 10496) && hitInfo.collider.GetComponent<Collider>().gameObject.layer == 8)
			{
				if (Time.time - hitTriggerLastTime >= 0.33f)
				{
					GameApp.GetInstance().GetGameScene().GetPlayer()
						.OnHit(m_LaserDamage);
				}
				hitTriggerLastTime = Time.time;
			}
			GameObject gameObject = _GOFireLine.transform.Find("laser gun_open_04").gameObject;
			if (gameObject != null)
			{
				float y = 0.95f + 1f * (1f * Mathf.Sin(Time.time * 50f));
				float x = gameObject.transform.localScale.x;
				gameObject.transform.localScale = new Vector3(x, y, 1f);
			}
		}
		if (_SkillReadyFireTimer >= 0f)
		{
			_SkillReadyFireTimer += Time.deltaTime;
			if (!_GOReadyLine.active)
			{
				_GOCatchingLine.SetActiveRecursively(false);
				_GOReadyLine.SetActiveRecursively(true);
				_GOFireLine.SetActiveRecursively(false);
			}
			RaycastHit hitInfo2 = default(RaycastHit);
			if (Physics.Raycast(new Ray(_GOFireLine.transform.position, -_GOFireLine.transform.right), out hitInfo2, m_MaxLineLength, 10496))
			{
				_GOReadyLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(true);
				m_LineLength = Vector3.Distance(hitInfo2.point, _GOCatchingLine.transform.position);
				float x2 = m_LineLength / _CatchingLineOffset * 1f;
				_GOReadyLine.transform.localScale = new Vector3(x2, 1f, 1f);
			}
			else
			{
				_GOReadyLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(false);
				m_LineLength = m_MaxLineLength;
				float x3 = m_LineLength / _CatchingLineOffset * 1f;
				_GOReadyLine.transform.localScale = new Vector3(x3, 1f, 1f);
			}
			if (_SkillReadyFireTimer >= _SkillReadyFireTime)
			{
				_SkillReadyFireTimer = -1f;
				_SkillFiringTimer = 0f;
				_GOCatchingLine.SetActiveRecursively(false);
				_GOReadyLine.SetActiveRecursively(false);
				_GOFireLine.SetActiveRecursively(true);
			}
		}
		if (_SkillFiringTimer >= 0f)
		{
			_SkillFiringTimer += Time.deltaTime;
			if (_SkillFiringTimer >= _SkillFiringTime)
			{
				_SkillReadyFireTimer = -1f;
				_SkillFiringTimer = -1f;
				_SkillCDTimer = _SkillCDTime;
				m_bIsFindPlayer = false;
				_GOCatchingLine.SetActiveRecursively(true);
				_GOReadyLine.SetActiveRecursively(false);
				_GOFireLine.SetActiveRecursively(false);
			}
		}
	}

	private bool IsCdIng()
	{
		if (_SkillCDTimer <= 0f)
		{
			return false;
		}
		return true;
	}

	private void SkillCD()
	{
		if (_SkillCDTimer > 0f)
		{
			_SkillCDTimer -= Time.deltaTime;
		}
	}

	private void ResetCD()
	{
		_SkillCDTimer = _SkillCDTime;
	}
}
