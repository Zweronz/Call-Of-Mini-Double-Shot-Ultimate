using UnityEngine;
using Zombie3D;

public abstract class BaseCameraScript : MonoBehaviour
{
	protected float angelH;

	protected float angelV;

	protected float lastUpdateTime;

	protected float deltaTime;

	public GameScene gameScene;

	protected Transform m_LookTargetTransform;

	public Vector3 cameraDistanceFromPlayerWhenIdle;

	public Vector3 cameraDistanceFromPlayerWhenAimed;

	public float cameraSwingSpeed;

	public float minAngelV;

	public float maxAngelV;

	public float fixedAngelV;

	public bool isAngelVFixed;

	public bool limitReticle;

	public bool allowReticleMove;

	public float reticleLogoRange = 0.15f;

	public float reticleMoveSpeed = 20f;

	public float mutipleSizeReticle;

	protected GameObject[] lastTransparentObjList = new GameObject[5];

	protected Vector3 moveTo;

	protected bool behindWall;

	public Vector3 cameraDistanceFromPlayer;

	public bool lastInWall;

	protected ScreenBloodScript bs;

	protected bool started;

	public float CAMERA_AIM_FOV = 22f;

	public float CAMERA_NORMAL_FOV = 38f;

	protected Vector2 reticlePosition;

	protected Transform cameraTransform;

	protected CameraType cameraType;

	public AudioSource loseAudio;

	public Transform CameraTransform
	{
		get
		{
			return cameraTransform;
		}
	}

	public Vector2 ReticlePosition
	{
		get
		{
			return reticlePosition;
		}
		set
		{
			reticlePosition = value;
		}
	}

	public abstract CameraType GetCameraType();

	public virtual void SetCameraTarget(Transform trans)
	{
		m_LookTargetTransform = trans;
	}

	public virtual void Init(Transform targetTransform)
	{
		gameScene = GameApp.GetInstance().GetGameScene();
		m_LookTargetTransform = targetTransform;
		angelH = m_LookTargetTransform.rotation.eulerAngles.y;
		cameraDistanceFromPlayer = cameraDistanceFromPlayerWhenIdle;
		base.transform.position = m_LookTargetTransform.TransformPoint(cameraDistanceFromPlayer);
		base.transform.rotation = Quaternion.Euler(0f - angelV, angelH, 0f);
		reticlePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			cameraSwingSpeed *= 20f;
		}
		else if (Screen.width == 960)
		{
			cameraSwingSpeed *= 0.4f;
		}
		float[] array = new float[32];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 100f;
		}
		array[17] = 1000f;
		base.GetComponent<Camera>().layerCullDistances = array;
		GameObject gameObject = base.transform.Find("Screen_Blood").gameObject;
		bs = gameObject.GetComponent<ScreenBloodScript>();
		started = true;
	}

	public virtual void CreateScreenBlood(float damage)
	{
	}

	public virtual void ZoomIn(float deltaTime)
	{
		cameraDistanceFromPlayer.x = Mathf.Lerp(cameraDistanceFromPlayer.x, cameraDistanceFromPlayerWhenAimed.x, deltaTime * 10f);
		cameraDistanceFromPlayer.y = Mathf.Lerp(cameraDistanceFromPlayer.y, cameraDistanceFromPlayerWhenAimed.y, deltaTime * 10f);
		cameraDistanceFromPlayer.z = Mathf.Lerp(cameraDistanceFromPlayer.z, cameraDistanceFromPlayerWhenAimed.z, deltaTime * 10f);
		base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_AIM_FOV, deltaTime * 10f);
	}

	public virtual void ZoomOut(float deltaTime)
	{
		cameraDistanceFromPlayer.x = Mathf.Lerp(cameraDistanceFromPlayer.x, cameraDistanceFromPlayerWhenIdle.x, deltaTime * 10f);
		cameraDistanceFromPlayer.y = Mathf.Lerp(cameraDistanceFromPlayer.y, cameraDistanceFromPlayerWhenIdle.y, deltaTime * 10f);
		cameraDistanceFromPlayer.z = Mathf.Lerp(cameraDistanceFromPlayer.z, cameraDistanceFromPlayerWhenIdle.z, deltaTime * 10f);
		base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_NORMAL_FOV, deltaTime * 10f);
	}
}
