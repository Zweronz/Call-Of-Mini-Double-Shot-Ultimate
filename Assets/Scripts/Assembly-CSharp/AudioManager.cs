using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public void Awake()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
	}

	public AudioClip LoadMusic(string name)
	{
		return Resources.Load(name, typeof(AudioClip)) as AudioClip;
	}

	public AudioClip LoadSound(string name)
	{
		return Resources.Load(name, typeof(AudioClip)) as AudioClip;
	}

	public void PlayMusic(string name)
	{
		if (OptionsInterface.IsOpenMusic())
		{
			AudioClip audioClip = LoadMusic(name);
			GameObject gameObject = new GameObject("AudioMusic::" + audioClip.name);
			gameObject.transform.parent = base.transform;
			AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			audioSource.clip = audioClip;
			audioSource.loop = true;
			audioSource.playOnAwake = false;
			audioSource.Play();
		}
	}

	public void PlaySound(string name)
	{
		if (OptionsInterface.IsOpenSound())
		{
			AudioClip audioClip = LoadSound(name);
			GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
			gameObject.transform.parent = base.transform;
			AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			audioSource.clip = audioClip;
			audioSource.Play();
			audioSource.playOnAwake = false;
			Object.Destroy(gameObject, audioClip.length);
		}
	}

	public void StopMusic()
	{
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioMusic::"))
			{
				arrayList.Add(item.gameObject);
			}
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy((GameObject)arrayList[i]);
		}
	}

	public static void PlaySoundOnce(string strPath)
	{
	}

	public static void PlayMusicOnce(string strPath, Transform transf)
	{
	}
}
