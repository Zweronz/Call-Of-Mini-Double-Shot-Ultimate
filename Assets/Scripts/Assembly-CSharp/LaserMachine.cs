using UnityEngine;
using Zombie3D;

public class LaserMachine : BaseMachine
{
	private GameObject _GOCatchingLine;

	private GameObject _GOReadyLine;

	private GameObject _GOFireLine;

	private float m_LaserDamage = 10f;

	private float m_LineLength = 20f;

	private float _SkillReadyFireTimer = -1f;

	private float _SkillReadyFireTime = 2f;

	private float _SkillFiringTimer = -1f;

	private float _SkillFiringTime = 3f;

	private float hitTriggerLastTime;

	private float _CatchingLineOffset = 4.980061f;

	private float _ReadyLineOffset = 4.980061f;

	private float _FireLineOffset = 3.932f;

	public override MachineType GetMachineType()
	{
		return MachineType.E_LASER;
	}

	public override void Init()
	{
		base.Init();
		m_fSkillCDTime = 0f;
		_GOCatchingLine = m_goMachineBulletObj.transform.Find("CatchLine").gameObject;
		_GOReadyLine = m_goMachineBulletObj.transform.Find("ReadyLine").gameObject;
		_GOFireLine = m_goMachineBulletObj.transform.Find("FireLine").gameObject;
		_CatchingLineOffset = Vector3.Distance(_GOCatchingLine.transform.Find("laser gun_infrared_02").position, _GOCatchingLine.transform.Find("Plane01").position);
		_ReadyLineOffset = Vector3.Distance(_GOReadyLine.transform.Find("laser gun_infrared_02").position, _GOReadyLine.transform.Find("Plane01").position);
		_FireLineOffset = Vector3.Distance(_GOFireLine.transform.Find("laser gun_open_01").position, _GOFireLine.transform.Find("laser gun_open_02").position);
		m_LaserDamage = GameApp.GetInstance().GetGameState().CalcMaxHp() / 10f;
		_GOCatchingLine.SetActiveRecursively(false);
		_GOReadyLine.SetActiveRecursively(false);
		_GOFireLine.SetActiveRecursively(false);
	}

	public override void BeginAnimationEnd()
	{
		_GOCatchingLine.SetActiveRecursively(true);
		_GOReadyLine.SetActiveRecursively(false);
		_GOFireLine.SetActiveRecursively(false);
	}

	public override void CatchingMode()
	{
		if (!_GOCatchingLine.active)
		{
			_GOCatchingLine.SetActiveRecursively(true);
			_GOReadyLine.SetActiveRecursively(false);
			_GOFireLine.SetActiveRecursively(false);
		}
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(new Ray(_GOCatchingLine.transform.position, -_GOCatchingLine.transform.right), out hitInfo, m_fMaxDistance, 10496))
		{
			_GOCatchingLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(true);
			if (hitInfo.collider.GetComponent<Collider>().gameObject.layer != 8)
			{
				m_LineLength = Vector3.Distance(hitInfo.point, _GOCatchingLine.transform.position);
			}
			else
			{
				m_LineLength = m_fMaxDistance;
			}
			float x = m_LineLength / _CatchingLineOffset * 1f;
			_GOCatchingLine.transform.localScale = new Vector3(x, 1f, 1f);
			if (hitInfo.collider.GetComponent<Collider>().gameObject.layer == 8)
			{
				m_eMachineState = MachineState.E_FIRING;
				_SkillReadyFireTimer = 0f;
				float x2 = m_fMaxDistance / _FireLineOffset;
				_GOFireLine.transform.localScale = new Vector3(x2, 1f, 1f);
			}
		}
		else
		{
			_GOCatchingLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(false);
			m_LineLength = m_fMaxDistance;
			float x3 = m_LineLength / _CatchingLineOffset * 1f;
			_GOCatchingLine.transform.localScale = new Vector3(x3, 1f, 1f);
		}
	}

	public override void ReadyMode()
	{
		if (_SkillReadyFireTimer >= 0f)
		{
			_SkillReadyFireTimer += Time.deltaTime;
			if (!_GOReadyLine.active)
			{
				_GOCatchingLine.SetActiveRecursively(false);
				_GOReadyLine.SetActiveRecursively(true);
				_GOFireLine.SetActiveRecursively(false);
			}
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(new Ray(_GOReadyLine.transform.position, -_GOReadyLine.transform.right), out hitInfo, m_fMaxDistance, 10496))
			{
				_GOReadyLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(true);
				m_LineLength = m_fMaxDistance;
				float x = m_LineLength / _ReadyLineOffset * 1f;
				_GOReadyLine.transform.localScale = new Vector3(x, 1f, 1f);
			}
			else
			{
				_GOReadyLine.transform.Find("laser gun_infrared_02").gameObject.SetActiveRecursively(false);
				m_LineLength = m_fMaxDistance;
				float x2 = m_LineLength / _CatchingLineOffset * 1f;
				_GOReadyLine.transform.localScale = new Vector3(x2, 1f, 1f);
			}
			_GOReadyLine.transform.Find("ReadyLineEffect").GetComponent<Animation>()["laser gun_ready"].wrapMode = WrapMode.Loop;
			_GOReadyLine.transform.Find("ReadyLineEffect").GetComponent<Animation>().Play("laser gun_ready");
			if (_SkillReadyFireTimer >= _SkillReadyFireTime)
			{
				_SkillReadyFireTimer = -1f;
				_SkillFiringTimer = 0f;
				_GOReadyLine.transform.Find("ReadyLineEffect").GetComponent<Animation>().Stop("laser gun_ready");
				_GOCatchingLine.SetActiveRecursively(false);
				_GOReadyLine.SetActiveRecursively(false);
				_GOFireLine.SetActiveRecursively(true);
			}
		}
	}

	public override void FiringMode()
	{
		if (_GOFireLine.active)
		{
			Debug.DrawLine(_GOFireLine.transform.position, -_GOFireLine.transform.right * m_fMaxDistance, Color.yellow);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(new Ray(_GOFireLine.transform.position, -_GOFireLine.transform.right), out hitInfo, m_fMaxDistance, 10496))
			{
				Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
				Player friendPlayer = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
				Player player2 = player;
				if (hitInfo.collider.GetComponent<Collider>().gameObject.layer == 8)
				{
					if (hitInfo.transform.name == player.PlayerObject.name)
					{
						player2 = player;
					}
					if (friendPlayer != null && hitInfo.transform.name == friendPlayer.PlayerObject.name)
					{
						player2 = friendPlayer;
					}
					if (Time.time - hitTriggerLastTime >= 0.33f)
					{
						float laserDamage = m_LaserDamage;
						player2.OnHit(laserDamage);
						player2.OnHitBack(0.4f, 1f, _GOFireLine.transform.up);
						hitTriggerLastTime = Time.time;
					}
				}
			}
			GameObject gameObject = _GOFireLine.transform.Find("laser gun_open_04").gameObject;
			if (gameObject != null)
			{
				float t = Mathf.Abs(Mathf.Sin(Time.time * 50f));
				float y = Mathf.Lerp(0.8f, 1f, t);
				float x = gameObject.transform.localScale.x;
				gameObject.transform.localScale = new Vector3(x, y, 1f);
			}
		}
		if (_SkillFiringTimer >= 0f)
		{
			_SkillFiringTimer += Time.deltaTime;
			if (_SkillFiringTimer >= _SkillFiringTime)
			{
				_SkillReadyFireTimer = -1f;
				_SkillFiringTimer = -1f;
				ResetCD();
			}
		}
	}
}
