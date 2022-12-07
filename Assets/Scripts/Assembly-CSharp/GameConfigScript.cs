using UnityEngine;

[AddComponentMenu("TPS/PrefabObjectManager")]
public class GameConfigScript : MonoBehaviour
{
	public GameObject player;

	public GameObject playerUIShow;

	public GameObject mapData;

	public Texture reticle;

	public GameObject hitBlood;

	public GameObject deadBlood;

	public GameObject deadFoorblood;

	public GameObject deadFoorblood2;

	public GameObject hitParticles01;

	public GameObject hitParticles02;

	public GameObject hitParticles03;

	public GameObject hitParticles04;

	public GameObject projectile;

	public GameObject rocketExlposion;

	public GameObject Exlposion01;

	public GameObject Exlposion02;

	public GameObject Exlposion03;

	public GameObject rpgFloor;

	public GameObject laser;

	public GameObject laserHit;

	public GameObject BulletShell01;

	public GameObject BulletShell02;

	public GameObject BulletShell03;

	public GameObject AvatarGladiatorEffect;

	public GameObject woodBoxes;

	public GameObject woodExplode;

	public GameObject boomerExplosion;

	public GameObject Jerrican;

	public GameObject JerricanExplodeFloor;

	public GameObject JerricanExplodeAudio;

	public GameObject FragGrenade;

	public GameObject StormGrenade;

	public GameObject swatGunFire;

	public GameObject swatBullet;

	public GameObject boomerBurst;

	public GameObject hunterGunFire;

	public GameObject hunterBullet;

	public GameObject zombieLaserLine;

	public Texture zombieLaserLineTex1;

	public Texture zombieLaserLineTex2;

	public GameObject infecterBullet;

	public GameObject trackerGunFire;

	public GameObject trackerShells;

	public GameObject turreterGunFire;

	public GameObject turreterCannonShells;

	public GameObject SporeParticles01;

	public GameObject SporeParticles02;

	public GameObject SporeParticles03;

	public GameObject SporeParticles04;

	public GameObject SporeParticles05;

	public GameObject SporeParticles06;

	public GameObject SporeParticles07;

	public GameObject SporeParticles08;

	public GameObject SporeParticles09;

	public GameObject SporeParticles10;

	public GameObject SporeParticles11;

	public GameObject HunterIIAttackParticles;

	public GameObject SporeChild;

	public GameObject SporeThorn;

	public AudioSource UIClickAudio;

	public AudioSource UIPlayerSpeedUp;

	public AudioSource BulletShellHitFloorAudio01;

	public AudioSource BulletShellHitFloorAudio02;

	public GameObject SurvivalModeExitDoor_Parent;

	public GameObject[] machines = new GameObject[3];

	public GameObject[] maps = new GameObject[2];

	public GameObject[] levels = new GameObject[10];

	public GameObject[] weapons = new GameObject[10];

	public GameObject[] weaponBullets = new GameObject[22];

	public GameObject[] weaponBulletHitParticles = new GameObject[21];

	public GameObject[] enemy = new GameObject[10];

	public GameObject[] items = new GameObject[10];

	private void Start()
	{
	}

	private void Update()
	{
	}
}
