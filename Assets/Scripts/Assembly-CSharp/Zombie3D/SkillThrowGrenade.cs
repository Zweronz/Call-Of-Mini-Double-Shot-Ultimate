using UnityEngine;

namespace Zombie3D
{
	public class SkillThrowGrenade : SkillImpl
	{
		private GameObject m_GrenadeObj;

		public float m_StaminaSpend = 27f;

		public override void Init(Player player, Skill skill)
		{
			base.Init(player, skill);
			m_StaminaSpend = 27f;
			m_Player.Stamina -= m_StaminaSpend;
			float num = 35f;
			switch (skill.Level)
			{
			case 1u:
				num = 35f;
				break;
			case 2u:
				num = 70f;
				break;
			case 3u:
				num = 110f;
				break;
			case 4u:
				num = 155f;
				break;
			case 5u:
				num = 205f;
				break;
			case 6u:
				num = 260f;
				break;
			case 7u:
				num = 320f;
				break;
			case 8u:
				num = 385f;
				break;
			case 9u:
				num = 455f;
				break;
			case 10u:
				num = 530f;
				break;
			default:
				num = 35f;
				break;
			}
			num *= 2f;
			Vector3 vector = new Vector3(m_Player.PlayerObject.transform.position.x, 10000.1f, m_Player.PlayerObject.transform.position.z);
			float num2 = -3f;
			float num3 = 3f;
			Vector3 vector2 = m_Player.PlayerObject.transform.forward * num3;
			float num4 = 10f;
			float num5 = num3 / num4;
			float num6 = (num2 - 0.5f * Physics.gravity.y * num5 * num5) / num5;
			Vector3 vector3 = Vector3.up * num6 + vector2.normalized * num4;
			GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillFragGrenade") as GameObject, m_Player.PlayerObject.transform.position + Vector3.up * (0f - num2), Quaternion.LookRotation(-vector3)) as GameObject;
			GrenadeItem component = gameObject.GetComponent<GrenadeItem>();
			component.explodeTime = 2f;
			component.radius = 4.5f;
			component.damage = num;
			component.explodeObj = Resources.Load("Zombie3D/Misc/SkillFragGrenadeExplode") as GameObject;
			Vector3 force = Vector3.up * 1f + m_Player.PlayerObject.transform.forward * 3f;
			gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
			string text = "GrenadesColorAnim1";
			AnimationState animationState = gameObject.GetComponent<Animation>()[text];
			if (animationState != null)
			{
				animationState.wrapMode = WrapMode.Loop;
				gameObject.GetComponent<Animation>().Play(text);
			}
		}

		public override void Update(float deltaTime)
		{
		}

		public override void Stop()
		{
		}
	}
}
