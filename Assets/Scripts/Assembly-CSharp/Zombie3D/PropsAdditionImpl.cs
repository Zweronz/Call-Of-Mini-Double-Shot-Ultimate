namespace Zombie3D
{
	public class PropsAdditionImpl
	{
		protected PropsAddition m_propsAddition;

		protected long m_lMaxSecSustainTime = 86400000L;

		protected long m_lBeginSustainTime;

		public PropsAddition GetPropsAddition()
		{
			return m_propsAddition;
		}

		public bool CheckAgeing(long nowTime = -1)
		{
			if (nowTime == -1)
			{
				nowTime = UtilsEx.getNowDateSeconds();
			}
			if (nowTime - m_lBeginSustainTime < m_lMaxSecSustainTime && nowTime - m_lBeginSustainTime >= 0)
			{
				return true;
			}
			return false;
		}

		public new string ToString()
		{
			string empty = string.Empty;
			return (int)m_propsAddition.PropsType + "," + (int)m_propsAddition.PropsPart + "," + m_propsAddition.Level + "," + m_lBeginSustainTime + "," + m_lMaxSecSustainTime;
		}

		public virtual void Init(PropsAddition props, long beginTime = 0, long maxSustainTime = 0)
		{
			m_propsAddition = props;
			if (maxSustainTime != 0L)
			{
				m_lMaxSecSustainTime = maxSustainTime;
			}
			else
			{
				m_lMaxSecSustainTime = 86400000L;
			}
			if (beginTime != 0L)
			{
				m_lBeginSustainTime = beginTime;
			}
			else
			{
				m_lBeginSustainTime = UtilsEx.getNowDateSeconds();
			}
		}

		public virtual float GetEffect(float des)
		{
			if (m_propsAddition.PropsType == enPropsAdditionType.E_Damage)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Weapon)
				{
					return 1300f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_AttackAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.2f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.25f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_DefenceAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.25f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.35f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_SpeedAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.22f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.25f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_HpAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.25f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.35f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_AttackSpeedAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.12f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.15f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_StaminaAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.17f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.2f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_ExpAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.27f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.3f;
				}
				return des;
			}
			if (m_propsAddition.PropsType == enPropsAdditionType.E_CashAdditive)
			{
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Head)
				{
					return 0.27f;
				}
				if (m_propsAddition.PropsPart == enPropsAdditionPart.E_Avatar_Body)
				{
					return 0.3f;
				}
				return des;
			}
			return des;
		}
	}
}
