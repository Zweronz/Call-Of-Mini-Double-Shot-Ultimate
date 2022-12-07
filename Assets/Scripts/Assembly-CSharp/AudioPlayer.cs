using System.Collections;
using UnityEngine;
using Zombie3D;

public class AudioPlayer
{
	protected Hashtable audioTable = new Hashtable();

	private GameObject m_MusicManager;

	public void AddAudio(Transform folderTrans, string name)
	{
		if (folderTrans != null)
		{
			Transform transform = folderTrans.Find(name);
			if (transform != null)
			{
				audioTable.Add(name, transform.GetComponent<AudioSource>());
			}
		}
	}

	public void PlaySound(string name, bool bForcePlay = false)
	{
		if (!GameApp.GetInstance().GetGameState().SoundOn)
		{
			return;
		}
		object obj = audioTable[name];
		AudioSource audioSource = obj as AudioSource;
		if (audioSource != null)
		{
			if (bForcePlay)
			{
				audioSource.Play();
			}
			else if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
	}

	public void PlayMusic(GameObject music_prefab)
	{
		if (GameApp.GetInstance().GetGameState().MusicOn)
		{
			Object.Destroy(m_MusicManager);
			m_MusicManager = null;
			m_MusicManager = new GameObject("MusicManager");
			m_MusicManager.transform.position = new Vector3(0f, 10000f, 0f);
			GameObject gameObject = Object.Instantiate(music_prefab, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = m_MusicManager.transform;
			gameObject.transform.localPosition = Vector3.zero;
			AudioSource audioSource = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
			if (audioSource != null)
			{
				audioSource.Play();
			}
		}
	}

	public static void PlaySound(AudioSource audio)
	{
		if (GameApp.GetInstance().GetGameState().MusicOn)
		{
			audio.Play();
		}
	}
}
