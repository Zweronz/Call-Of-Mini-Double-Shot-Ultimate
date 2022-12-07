using UnityEngine;

public class GameLoadingUI : UIHandler, UIContainer
{
	public uiGroup m_uiGroup;

	public void DrawSprite(UISprite sprite)
	{
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public void SetupLoadingUI(bool bShow, UIManager ui_manager, Material matCommonBg, bool bNeedToShift = false)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		m_uiGroup = new uiGroup(ui_manager);
		string file_name = string.Empty;
		string introduction_title = string.Empty;
		string introduction_content = string.Empty;
		GetRandomEnemyIconMaterialFile(ref file_name, ref introduction_title, ref introduction_content);
		Material material = Resources.Load("Zombie3D/UI/Materials/EnemyIcons/" + file_name) as Material;
		if (material == null)
		{
			Debug.LogError("ERROR: Cannot find an Enemy Icon!!!");
		}
		Material mat = SceneUIManager.LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		Resources.UnloadUnusedAssets();
		float num = 0f;
		float num2 = 0f;
		if (bNeedToShift)
		{
			num = (float)Screen.width / 2f - 480f;
			num2 = (float)Screen.height / 2f - 320f;
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(num, num2, 960f, 640f), matCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		m_uiGroup.Add(control);
		if (AutoUIResolution.bIsIphoneResolution)
		{
			int num3 = 88;
			control = UIUtils.BuildImage(0, new Rect(0f, 0f, num3, 640f), mat, new Rect(600f, 1f, 1f, 1f), new Vector2(num3, 640f), 0);
			m_uiGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(num3 + 960, 0f, num3, 640f), mat, new Rect(600f, 1f, 1f, 1f), new Vector2(num3, 640f), 0);
			m_uiGroup.Add(control);
		}
		control = UIUtils.BuildImage(0, new Rect(num, num2 + 440f, 960f, 200f), mat, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 200f));
		m_uiGroup.Add(control);
		control = UIUtils.BuildImage(0, new Rect(num, num2, 960f, 150f), mat, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 150f));
		m_uiGroup.Add(control);
		control = UIUtils.BuildImage(0, new Rect(num, num2 + 392f, 960f, 74f), mat, new Rect(0f, 929f, 960f, 74f), new Vector2(960f, 74f));
		m_uiGroup.Add(control);
		control = UIUtils.BuildImage(0, new Rect(num, num2 + 137f, 960f, 19f), mat, new Rect(0f, 1005f, 960f, 19f), new Vector2(960f, 19f));
		m_uiGroup.Add(control);
		control = UIUtils.BuildImage(0, new Rect(num, num2 + 88f, 512f, 512f), material, new Rect(0f, 0f, 512f, 512f), new Vector2(512f, 512f));
		m_uiGroup.Add(control);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 610f, num2 + 413f, 350f, 30f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-22", introduction_title, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		m_uiGroup.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(num + 550f, num2 + 190f, 380f, 150f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", introduction_content, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		m_uiGroup.Add(uIText);
		control = UIUtils.BuildImage(0, new Rect(num + 395f, num2 + 87f, 190f, 16f), mat, new Rect(0f, 910f, 190f, 16f), new Vector2(190f, 16f));
		m_uiGroup.Add(control);
	}

	public void GetRandomEnemyIconMaterialFile(ref string file_name, ref string introduction_title, ref string introduction_content)
	{
		int num = Random.Range(1, 3);
		switch (Random.Range(1, 12))
		{
		case 1:
			switch (num)
			{
			case 1:
				file_name = "Zombie_Green";
				break;
			case 2:
				file_name = "Zombie_Purple";
				break;
			}
			introduction_title = "Shambler";
			introduction_content = "Once infected by the virus, an ordinary human brain decays rapidly. When almost nothing remains, a shambling beast is born.";
			break;
		case 2:
			file_name = "Zombie_Self_Destruction01";
			introduction_title = "Gasbomb";
			introduction_content = "The Gasbomb bulges with volatile gasses. It is fragile but very dangerous.";
			break;
		case 3:
			file_name = "Zombie_Swat_Green";
			introduction_title = "Security Guard";
			introduction_content = "A stereotypical rent-a-cop, mindless and trigger-happy even before he was infected.";
			break;
		case 4:
			file_name = "Zombie_Lava";
			introduction_title = "Pyro";
			introduction_content = "The Pyro is a bioengineered zombie whose creator remains unknown. Foul substances in its body burn unchecked.";
			break;
		case 5:
			file_name = "Zombie_Infecter";
			introduction_title = "Putridifier";
			introduction_content = "A bioengineered beast that produces noxious gas inside its body. This gas is spread by a high-tech contraption provided by an unknown source.";
			break;
		case 6:
			file_name = "Zombie_Spider";
			introduction_title = "Iron Dynamo";
			introduction_content = "Reports say that this monster can create a powerful shield around itself just by moving.";
			break;
		case 7:
			file_name = "Zombie_Hunter";
			introduction_title = "Hunter";
			introduction_content = "An infected mutant with powerful legs and a lust for chasing down prey.";
			break;
		case 8:
			file_name = "Zombie_Laser";
			introduction_title = "Atomizer";
			introduction_content = "A huge biological weapon upgraded from the Putridifier and equipped with a high-energy laser gun to atomize anything caught in its path.";
			break;
		case 9:
			file_name = "Zombie_Batcher";
			introduction_title = "Butcher";
			introduction_content = "A second-stage infected mutant with a monstrous body and devastating power. ";
			break;
		case 10:
			file_name = "Zombie_Tracker";
			introduction_title = "Destroyer";
			introduction_content = "A large mutant carrying a cannon. He shoots randomly with no target in particular.";
			break;
		case 11:
			file_name = "Zombie_Turreter";
			introduction_title = "Annihilator";
			introduction_content = "A big mutant equipped with a cannon that ruthlessly persecutes anything that moves.";
			break;
		case 12:
			file_name = "Zombie_Spore";
			introduction_title = "Infector";
			introduction_content = "The plant-like Infector spreads the virus with microscopic spores. It is presumed to be the source of the first wave of infections.";
			break;
		}
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			file_name += "_LOW";
		}
	}
}
