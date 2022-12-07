using System.IO;
using UnityEngine;

namespace Zombie3D
{
	public class GameApp
	{
		protected static GameApp instance;

		protected GameConfigScript gameConfig;

		protected GameState gameState;

		protected GameScene scene;

		protected GameScript script;

		protected bool m_bLoadMap;

		protected GameApp()
		{
		}

		public static GameApp GetInstance()
		{
			if (instance == null)
			{
				instance = new GameApp();
			}
			return instance;
		}

		public void Save()
		{
			string text = Utils.SavePath();
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			gameState.Save(text + "/MyGameData");
		}

		public void Load()
		{
			string text = Utils.SavePath();
			if (File.Exists(text + "/MyGameData"))
			{
				gameState.Load(text + "/MyGameData");
			}
		}

		public void RemoveUserDataFile()
		{
			string text = Utils.SavePath();
			if (File.Exists(text + "/MyGameData"))
			{
				File.Delete(text + "/MyGameData");
			}
		}

		public void Init()
		{
			script = GameObject.Find("GameApp").GetComponent<GameScript>();
			if (GameObject.Find("GameConfig") != null)
			{
				gameConfig = GameObject.Find("GameConfig").GetComponent<GameConfigScript>();
			}
			ConfigManager.Instance();
			if (gameState == null)
			{
				gameState = new GameState();
				Load();
				gameState.Init();
			}
			if (m_bLoadMap)
			{
				scene = new GameScene();
				scene.Init(Application.loadedLevel - 1);
			}
			else
			{
				scene = null;
			}
		}

		public void Loop(float deltaTime)
		{
			if (scene != null)
			{
				scene.DoLogic(deltaTime);
			}
		}

		public GameConfigScript GetGameConfig()
		{
			return gameConfig;
		}

		public GameScene GetGameScene()
		{
			return scene;
		}

		public GameState GetGameState()
		{
			return gameState;
		}

		public void SetLoadMap(bool bLoadMap)
		{
			m_bLoadMap = bLoadMap;
		}
	}
}
