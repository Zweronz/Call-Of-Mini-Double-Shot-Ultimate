using UnityEngine;

public class UIUtils
{
	public static void BuildIpone5Frame(UIManager m_UIManager)
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = 1.5f;
		UIImage uIImage = null;
		if (num > num2)
		{
			float num3 = 640f / (float)Screen.height * (float)Screen.width - 960f;
			Material mat = Resources.Load("Zombie3D/UI/Materials/VerticalFrame") as Material;
			uIImage = BuildImage(0, new Rect(0f - num3 / 2f, 0f, 90f, 640f), mat, new Rect(0f, 0f, 90f, 640f), new Vector2(90f, 640f), 0);
			m_UIManager.Add(uIImage);
			uIImage = BuildImage(0, new Rect(960f + num3 / 2f - 90f, 0f, 90f, 640f), mat, new Rect(90f, 0f, 90f, 640f), new Vector2(90f, 640f), 0);
			m_UIManager.Add(uIImage);
		}
		else if (num != num2)
		{
			float num4 = 960f / (float)Screen.width * (float)Screen.height - 640f;
			Material mat2 = Resources.Load("Zombie3D/UI/Materials/HorizontalFrameUI") as Material;
			uIImage = BuildImage(0, new Rect(0f, 640f + num4 / 2f - 64f, 1024f, 64f), mat2, new Rect(0f, 0f, 1024f, 64f), new Vector2(1024f, 64f), 0);
			m_UIManager.Add(uIImage);
			uIImage = BuildImage(0, new Rect(0f, 0f - num4 / 2f, 1024f, 64f), mat2, new Rect(0f, 64f, 1024f, 64f), new Vector2(1024f, 64f), 0);
			m_UIManager.Add(uIImage);
		}
	}

	public static UIBlock BuildBlock(int id, Rect scrRect)
	{
		UIBlock uIBlock = new UIBlock();
		uIBlock.Id = id;
		uIBlock.Rect = AutoUI.AutoRect(scrRect);
		return uIBlock;
	}

	public static UIText BuildUIText(int id, Rect scrRect, UIText.enAlignStyle align_style, int bNeedShiftToRight = 2)
	{
		UIText uIText = new UIText();
		uIText.Id = 0;
		uIText.AlignStyle = align_style;
		Rect rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		if (rect.height < 15f)
		{
		}
		uIText.Rect = rect;
		return uIText;
	}

	public static UIImage BuildImage(int id, Rect scrRect, Material mat, Rect rcMat, Vector2 rect_size, int bNeedShiftToRight = 2)
	{
		UIImage uIImage = new UIImage();
		uIImage.Id = id;
		uIImage.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uIImage.SetTexture(mat, AutoUI.AutoRect(rcMat), AutoUI.AutoSize(rect_size));
		return uIImage;
	}

	public static UIClickButton BuildClickButton(int id, Rect scrRect, Material mat, Rect rcNormal, Rect rcPressed, Rect rcDisabled, Vector2 rect_size, int bNeedShiftToRight = 2)
	{
		UIClickButton uIClickButton = new UIClickButton();
		uIClickButton.Id = id;
		uIClickButton.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uIClickButton.SetTexture(UIButtonBase.State.Normal, mat, AutoUI.AutoRect(rcNormal), AutoUI.AutoSize(rect_size));
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, mat, AutoUI.AutoRect(rcPressed), AutoUI.AutoSize(rect_size));
		uIClickButton.SetTexture(UIButtonBase.State.Disabled, mat, AutoUI.AutoRect(rcDisabled), AutoUI.AutoSize(rect_size));
		return uIClickButton;
	}

	public static UIPushButton BuildPushButton(int id, Rect scrRect, Material mat, Rect rcNormal, Rect rcPressed, Rect rcDisabled, Vector2 rect_size, int bNeedShiftToRight = 2)
	{
		UIPushButton uIPushButton = new UIPushButton();
		uIPushButton.Id = id;
		uIPushButton.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uIPushButton.SetTexture(UIButtonBase.State.Normal, mat, AutoUI.AutoRect(rcNormal), AutoUI.AutoSize(rect_size));
		uIPushButton.SetTexture(UIButtonBase.State.Pressed, mat, AutoUI.AutoRect(rcPressed), AutoUI.AutoSize(rect_size));
		uIPushButton.SetTexture(UIButtonBase.State.Disabled, mat, AutoUI.AutoRect(rcDisabled), AutoUI.AutoSize(rect_size));
		return uIPushButton;
	}

	public static UISelectButton BuildSelectButton(int id, Rect scrRect, Material mat, Rect rcNormal, Rect rcPressed, Rect rcDisabled, Vector2 rect_size, int bNeedShiftToRight = 2)
	{
		UISelectButton uISelectButton = new UISelectButton();
		uISelectButton.Id = id;
		uISelectButton.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uISelectButton.SetTexture(UIButtonBase.State.Normal, mat, AutoUI.AutoRect(rcNormal), AutoUI.AutoSize(rect_size));
		uISelectButton.SetTexture(UIButtonBase.State.Pressed, mat, AutoUI.AutoRect(rcPressed), AutoUI.AutoSize(rect_size));
		uISelectButton.SetTexture(UIButtonBase.State.Disabled, mat, AutoUI.AutoRect(rcDisabled), AutoUI.AutoSize(rect_size));
		return uISelectButton;
	}

	public static UIJoystickButtonEx BuildJoystickButtonEx(int id, Rect scrRect, Material mat, Rect rcNormal, Rect rcPressed, Rect rcDisabled, Vector2 rect_size, float minDistance, float maxDistance, int bNeedShiftToRight = 0)
	{
		UIJoystickButtonEx uIJoystickButtonEx = new UIJoystickButtonEx();
		uIJoystickButtonEx.Id = id;
		uIJoystickButtonEx.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uIJoystickButtonEx.MinDistance = minDistance;
		uIJoystickButtonEx.MaxDistance = maxDistance;
		uIJoystickButtonEx.SetTexture(UIButtonBase.State.Normal, mat, AutoUI.AutoRect(rcNormal), AutoUI.AutoSize(rect_size));
		uIJoystickButtonEx.SetTexture(UIButtonBase.State.Pressed, mat, AutoUI.AutoRect(rcPressed), AutoUI.AutoSize(rect_size));
		return uIJoystickButtonEx;
	}

	public static UIMove BuildUIMove(int id, Rect scrRect, float minX, float minY, int bNeedShiftToRight = 2)
	{
		UIMove uIMove = new UIMove();
		uIMove.Id = id;
		uIMove.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uIMove.MinX = minX;
		uIMove.MinY = minY;
		return uIMove;
	}

	public static UIMoveOuter BuildUIMoveOuter(int id, Rect scrRect, float minX, float minY, int bNeedShiftToRight = 2)
	{
		UIMoveOuter uIMoveOuter = new UIMoveOuter();
		uIMoveOuter.Id = id;
		uIMoveOuter.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uIMoveOuter.MinX = minX;
		uIMoveOuter.MinY = minY;
		return uIMoveOuter;
	}

	public static UIGroupControl BuildUIGroupControl(int id, Rect scrRect, int bNeedShiftToRight = 2)
	{
		UIGroupControl uIGroupControl = new UIGroupControl();
		uIGroupControl.Id = id;
		uIGroupControl.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		return uIGroupControl;
	}

	public static UIMacOSLoading BuildUIMacOSLoading(int id, Rect scrRect, int bNeedShiftToRight = 2)
	{
		UIMacOSLoading uIMacOSLoading = new UIMacOSLoading();
		uIMacOSLoading.Id = id;
		uIMacOSLoading.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		return uIMacOSLoading;
	}

	public static UIProgressBar BuildUIProgressBar(int id, Rect scrRect, int bNeedShiftToRight = 2)
	{
		UIProgressBar uIProgressBar = new UIProgressBar();
		uIProgressBar.Id = id;
		uIProgressBar.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		return uIProgressBar;
	}

	public static UIProgressBarRounded BuildUIProgressBarRounded(int id, Rect scrRect, int bNeedShiftToRight = 2)
	{
		UIProgressBarRounded uIProgressBarRounded = new UIProgressBarRounded();
		uIProgressBarRounded.Id = id;
		uIProgressBarRounded.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		return uIProgressBarRounded;
	}

	public static UIProgressBarProgressive BuildUIProgressBarRounded(int id, Rect scrRect, Material mat_bg, Material mat_fg, Rect rcBg_Tex, Rect rcFg_Tex, float percent, int bNeedShiftToRight = 2)
	{
		UIProgressBarProgressive uIProgressBarProgressive = new UIProgressBarProgressive();
		uIProgressBarProgressive.Id = id;
		uIProgressBarProgressive.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(scrRect), bNeedShiftToRight);
		uIProgressBarProgressive.SetParam(mat_bg, mat_fg, AutoUI.AutoRect(new Rect(398f, 708f, 174f, 20f)), AutoUI.AutoRect(new Rect(572f, 708f, 174f, 20f)), percent);
		return uIProgressBarProgressive;
	}
}
