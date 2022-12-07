using UnityEngine;
using Zombie3D;

[AddComponentMenu("TPS/TPSSimpleCamera")]
public class TPSSimpleCameraScript : BaseCameraScript
{
	public Texture reticle;

	private Player player;

	protected Shader transparentShader;

	protected Shader solidShader;

	private void Awake()
	{
		cameraTransform = Camera.main.transform;
	}

	public override CameraType GetCameraType()
	{
		return CameraType.TPSCamera;
	}

	private void Start()
	{
		solidShader = Shader.Find("iPhone/SolidTexture");
		transparentShader = Shader.Find("iPhone/AlphaBlend_Color");
		player = gameScene.GetPlayer();
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
	}

	private void LateUpdate()
	{
		if (!started)
		{
			return;
		}
		deltaTime = Time.deltaTime;
		float num = player.InputController.CameraRotation.x;
		float num2 = player.InputController.CameraRotation.y;
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			num = Input.GetAxis("Mouse X");
			num2 = Input.GetAxis("Mouse Y");
		}
		float num3 = reticlePosition.x - (float)(Screen.width / 2);
		if (allowReticleMove)
		{
			if (Mathf.Abs(num3) < reticleLogoRange * (float)Screen.width || num3 * num < 0f)
			{
				reticlePosition = new Vector2(reticlePosition.x + num * reticleMoveSpeed, reticlePosition.y);
				if (limitReticle)
				{
					if ((!(reticlePosition.y <= 40f) || !(num2 > 0f)) && (!(reticlePosition.y > 310f) || !(num2 < 0f)))
					{
						reticlePosition = new Vector2(reticlePosition.x, reticlePosition.y - num2 * reticleMoveSpeed);
					}
				}
				else
				{
					reticlePosition = new Vector2(reticlePosition.x, reticlePosition.y - num2 * reticleMoveSpeed);
				}
			}
			else
			{
				angelH += num * deltaTime * cameraSwingSpeed;
				reticlePosition = new Vector2(reticlePosition.x, reticlePosition.y - num2 * reticleMoveSpeed);
				angelV = fixedAngelV;
			}
		}
		else
		{
			angelH += num * deltaTime * cameraSwingSpeed;
			angelV += num2 * deltaTime * cameraSwingSpeed;
			angelV = Mathf.Clamp(angelV, minAngelV, maxAngelV);
		}
		float x = player.GetWeapon().Deflection.x;
		float y = player.GetWeapon().Deflection.y;
		cameraTransform.rotation = Quaternion.Euler(0f - (angelV + x), angelH + y, 0f);
		float num4 = 100f;
		if (gameScene.PlayingState == PlayingState.GamePlaying)
		{
			player.GetTransform().rotation = Quaternion.Euler(0f, angelH, 0f);
			moveTo = player.GetTransform().TransformPoint(cameraDistanceFromPlayer);
			Vector3 direction = moveTo - player.GetTransform().position;
			Ray ray = new Ray(player.GetTransform().position, direction);
			float magnitude = direction.magnitude;
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, magnitude, 67584))
			{
				GameObject gameObject = hitInfo.collider.gameObject;
				gameObject.layer = 16;
				gameObject.GetComponent<Renderer>().material.shader = transparentShader;
				Color gray = Color.gray;
				gray.a = 0.1f;
				gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", gray);
				for (int i = 0; i < 5 && !(lastTransparentObjList[i] == gameObject); i++)
				{
					if (lastTransparentObjList[i] == null)
					{
						lastTransparentObjList[i] = gameObject;
					}
				}
			}
			else
			{
				for (int j = 0; j < 5; j++)
				{
					if (lastTransparentObjList[j] != null)
					{
						lastTransparentObjList[j].GetComponent<Renderer>().material.shader = solidShader;
						lastTransparentObjList[j] = null;
					}
				}
			}
			cameraTransform.position = moveTo;
		}
		else
		{
			minAngelV = -70f;
			maxAngelV = 70f;
			cameraTransform.position = player.GetTransform().TransformPoint(3f * Mathf.Sin(Time.time * 0.3f), 4f, 3f * Mathf.Cos(Time.time * 0.3f));
			cameraTransform.LookAt(player.GetTransform());
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			deltaTime = 0f;
		}
	}
}
