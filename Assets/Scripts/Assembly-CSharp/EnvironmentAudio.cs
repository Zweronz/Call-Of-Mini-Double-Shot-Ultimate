using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class EnvironmentAudio : MonoBehaviour
{
	private List<AudioSource> m_EnvironmentAudios;

	private int m_NextEnvironmentAudioPlayInterval = 5;

	private float m_LastEnvironmentAudioPlayTime;

	private void Start()
	{
		m_EnvironmentAudios = new List<AudioSource>();
		AudioSource[] componentsInChildren = base.gameObject.GetComponentsInChildren<AudioSource>();
		foreach (AudioSource item in componentsInChildren)
		{
			m_EnvironmentAudios.Add(item);
		}
		m_LastEnvironmentAudioPlayTime = Time.time;
	}

	private void Update()
	{
		if (m_EnvironmentAudios.Count > 0 && Time.time - m_LastEnvironmentAudioPlayTime >= (float)m_NextEnvironmentAudioPlayInterval)
		{
			int index = Random.Range(0, m_EnvironmentAudios.Count);
			if (GameApp.GetInstance().GetGameState().MusicOn)
			{
				m_EnvironmentAudios[index].Play();
			}
			m_NextEnvironmentAudioPlayInterval = Random.Range(5, 11);
			m_LastEnvironmentAudioPlayTime = Time.time;
		}
	}
}
