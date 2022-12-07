using UnityEngine;
using Zombie3D;

[AddComponentMenu("TPS/TopWatchingCamera")]
public class TopWatchingCameraScript : BaseCameraScript
{
	protected bool cameraset;

	protected Vector3 absoluteDistanceFromPlayer;

	protected bool bShowGameStartEffect;

	private float GameStartEffectCameraStartTime;

	protected bool bShowGameStartEffectEnd;

	private float GameStartEffectEndCameraStartTime;

	protected bool bShowPlayerDeadEffect;

	protected float PlayerDeadEffectCameraStartTime;

	protected bool bShowPlayerReliveEffect;

	protected float PlayerReliveEffectCameraStartTime;

	protected bool bExplodeShakeCameraEffect;

	protected float ExplodeShakeTimer = -1f;

	protected float ExplodeShakeTime = 0.5f;

	protected float ExplodeShakeCameraEffectOffset = 1.5f;

	protected bool ExplodeShakeCameraEffectFullOffset = true;

	private float m_ChangeLookTypeTime = -1f;

	public void SetCameraLookType(int type, bool needLerp)
	{
		switch (type)
		{
		case 2:
			absoluteDistanceFromPlayer = new Vector3(0f, 7f, -7f);
			break;
		case 1:
			absoluteDistanceFromPlayer = new Vector3(0f, 8f, -5f);
			break;
		case 3:
			absoluteDistanceFromPlayer = new Vector3(0f, 9f, 0f);
			break;
		default:
			absoluteDistanceFromPlayer = new Vector3(0f, 8f, -5f);
			break;
		}
		if (needLerp)
		{
			m_ChangeLookTypeTime = Time.time;
		}
	}

	public override CameraType GetCameraType()
	{
		return CameraType.TopWatchingCamera;
	}

	private void Awake()
	{
		cameraTransform = Camera.main.transform;
	}

	private void Start()
	{
	}

	public override void Init(Transform targetTransform)
	{
		base.Init(targetTransform);
		moveTo = m_LookTargetTransform.TransformPoint(cameraDistanceFromPlayer);
		absoluteDistanceFromPlayer = moveTo - m_LookTargetTransform.position;
		base.transform.LookAt(m_LookTargetTransform.position + new Vector3(0f, 1f, 0f));
		started = true;
	}

	public void ShowGameStartEffect()
	{
		if (!bShowGameStartEffect)
		{
			bShowGameStartEffect = true;
			started = false;
			GameStartEffectCameraStartTime = Time.time;
		}
	}

	public bool GetShowGameStartEffect()
	{
		return bShowGameStartEffect;
	}

	public void ShowGameStartEffectEnd()
	{
		if (!bShowGameStartEffectEnd)
		{
			GameStartEffectEndCameraStartTime = Time.time;
			bShowGameStartEffectEnd = true;
			started = false;
		}
	}

	public bool GetShowGameStartEndEffect()
	{
		return bShowGameStartEffectEnd;
	}

	public void ShowPlayerDeadEffect()
	{
		if (!bShowPlayerDeadEffect)
		{
			if (bExplodeShakeCameraEffect)
			{
				bExplodeShakeCameraEffect = false;
			}
			if (bShowGameStartEffect)
			{
				bShowGameStartEffect = false;
			}
			bShowPlayerDeadEffect = true;
			started = false;
			PlayerDeadEffectCameraStartTime = Time.time;
			Time.timeScale = 0.25f;
		}
	}

	public void ShowPlayerReliveEffect()
	{
		if (!bShowPlayerReliveEffect)
		{
			bShowPlayerReliveEffect = true;
			started = false;
			PlayerReliveEffectCameraStartTime = Time.time;
		}
	}

	public void ShowExplodeEffect(bool bFullOffset = true)
	{
		ExplodeShakeCameraEffectFullOffset = bFullOffset;
		if (!bShowPlayerDeadEffect && !bExplodeShakeCameraEffect)
		{
			bExplodeShakeCameraEffect = true;
			started = false;
			ExplodeShakeTimer = 0f;
		}
	}

	public override void CreateScreenBlood(float damage)
	{
		if (bs != null)
		{
			bs.NewBlood(damage);
		}
		else
		{
			Debug.Log("bs null");
		}
	}

	private void Update()
	{
		if (bShowGameStartEffect)
		{
			if (Time.time - GameStartEffectCameraStartTime <= 1f)
			{
				Vector3 position = base.transform.position;
				Vector3 to = m_LookTargetTransform.position + new Vector3(0f, 1f, -4f);
				base.transform.position = Vector3.Lerp(position, to, 0.8f);
				base.transform.LookAt(m_LookTargetTransform.position + new Vector3(0f, 1f, 0f));
			}
			else
			{
				bShowGameStartEffect = false;
			}
		}
		else if (bShowGameStartEffectEnd)
		{
			if (Time.time - GameStartEffectEndCameraStartTime <= 1f)
			{
				Vector3 position2 = base.transform.position;
				Vector3 to2 = m_LookTargetTransform.position + absoluteDistanceFromPlayer;
				base.transform.position = Vector3.Lerp(position2, to2, 0.1f);
				base.transform.LookAt(m_LookTargetTransform.position + new Vector3(0f, 1f, 0f));
			}
			else
			{
				bShowGameStartEffectEnd = false;
				started = true;
			}
		}
		else if (bShowPlayerDeadEffect)
		{
			if (Time.time - PlayerDeadEffectCameraStartTime <= 1f)
			{
				Vector3 position3 = base.transform.position;
				Vector3 to3 = m_LookTargetTransform.position + new Vector3(0f, 1f, -4f);
				base.transform.position = Vector3.Lerp(position3, to3, 0.1f);
				base.transform.LookAt(m_LookTargetTransform.position + new Vector3(0f, 1f, 0f));
			}
			else
			{
				bShowPlayerDeadEffect = false;
				Time.timeScale = 1f;
			}
		}
		else if (bShowPlayerReliveEffect)
		{
			if (Time.time - PlayerReliveEffectCameraStartTime <= 1f)
			{
				Vector3 position4 = base.transform.position;
				Vector3 to4 = m_LookTargetTransform.position + absoluteDistanceFromPlayer;
				base.transform.position = Vector3.Lerp(position4, to4, 0.1f);
				base.transform.LookAt(m_LookTargetTransform.position + new Vector3(0f, 1f, 0f));
			}
			else
			{
				bShowPlayerReliveEffect = false;
				started = true;
			}
		}
		else
		{
			if (!bExplodeShakeCameraEffect)
			{
				return;
			}
			Vector3 position5 = m_LookTargetTransform.position + absoluteDistanceFromPlayer;
			bool flag = true;
			if (gameScene.DDSTrigger != null)
			{
				if (gameScene.DDSTrigger.MapIndex == 1)
				{
					if (position5.x <= -22.8f)
					{
						position5 = new Vector3(-22.8f, position5.y, position5.z);
						flag = false;
					}
					if (position5.x >= 22.8f)
					{
						position5 = new Vector3(22.8f, position5.y, position5.z);
						flag = false;
					}
					if (position5.z <= -38.66f)
					{
						position5 = new Vector3(position5.x, position5.y, -38.66f);
						flag = false;
					}
					if (position5.z >= 26.53f)
					{
						position5 = new Vector3(position5.x, position5.y, 26.53f);
						flag = false;
					}
				}
				else if (gameScene.DDSTrigger.MapIndex != 2)
				{
				}
			}
			ExplodeShakeTimer += Time.deltaTime;
			if (ExplodeShakeTimer > 0.2f)
			{
				float num = ExplodeShakeCameraEffectOffset - ExplodeShakeTimer / ExplodeShakeTime * (ExplodeShakeCameraEffectOffset * 0.8f);
				if (!ExplodeShakeCameraEffectFullOffset)
				{
					num /= 2f;
				}
				float y = position5.y + num * Mathf.Cos(ExplodeShakeTimer * 100f);
				base.transform.position = new Vector3(position5.x, y, position5.z);
				if (ExplodeShakeTimer > ExplodeShakeTime + 0.2f)
				{
					bExplodeShakeCameraEffect = false;
					ExplodeShakeTimer = -1f;
					started = true;
				}
			}
			else
			{
				base.transform.position = position5;
				if (flag)
				{
					base.transform.LookAt(m_LookTargetTransform.position + new Vector3(0f, 1f, 0f));
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (!started)
		{
			return;
		}
		deltaTime = Time.deltaTime;
		float num = 0f;
		float num2 = 0f;
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			num = Input.GetAxis("Mouse X");
			num2 = Input.GetAxis("Mouse Y");
		}
		angelH += num * deltaTime * cameraSwingSpeed;
		angelV += num2 * deltaTime * cameraSwingSpeed;
		angelV = Mathf.Clamp(angelV, minAngelV, maxAngelV);
		float num3 = 100f;
		if (gameScene.PlayingState == PlayingState.GamePlaying)
		{
			if (m_ChangeLookTypeTime > 0f)
			{
				Vector3 b = m_LookTargetTransform.position + absoluteDistanceFromPlayer;
				if (Vector3.Distance(base.transform.position, b) > 0.01f)
				{
					Vector3 position = base.transform.position;
					Vector3 to = m_LookTargetTransform.position + absoluteDistanceFromPlayer;
					moveTo = Vector3.Lerp(position, to, 0.1f);
				}
				else
				{
					moveTo = m_LookTargetTransform.position + absoluteDistanceFromPlayer;
					m_ChangeLookTypeTime = -1f;
				}
			}
			else if (m_LookTargetTransform != null)
			{
				moveTo = m_LookTargetTransform.position + absoluteDistanceFromPlayer;
			}
			if (gameScene.DDSTrigger != null)
			{
				if (gameScene.DDSTrigger.MapIndex == 1)
				{
					if (moveTo.x <= -22.8f)
					{
						moveTo = new Vector3(-22.8f, moveTo.y, moveTo.z);
					}
					if (moveTo.x >= 22.8f)
					{
						moveTo = new Vector3(22.8f, moveTo.y, moveTo.z);
					}
					if (moveTo.z <= -38.66f)
					{
						moveTo = new Vector3(moveTo.x, moveTo.y, -38.66f);
					}
					if (moveTo.z >= 26.53f)
					{
						moveTo = new Vector3(moveTo.x, moveTo.y, 26.53f);
					}
				}
				else if (gameScene.DDSTrigger.MapIndex != 2)
				{
				}
			}
			cameraTransform.position = moveTo;
			if (m_ChangeLookTypeTime > 0f)
			{
				base.transform.LookAt(m_LookTargetTransform.position + new Vector3(0f, 1f, 0f));
			}
		}
		lastUpdateTime = Time.time;
	}
}
