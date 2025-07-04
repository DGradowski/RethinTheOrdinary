using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
	public static SoundMixerManager instance;

	[SerializeField] private AudioMixer m_mixer;

	private void Awake()
	{
		SetMasterVolume(0.5f);
		SetSoundFXVolume(0.5f);
		SetMusicVolume(0.5f);

		if (instance == null)
		{
			instance = this;
		}
	}

	public void SetMasterVolume(float level)
	{
		m_mixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
	}

	public void SetSoundFXVolume(float level)
	{
		m_mixer.SetFloat("fxVolume", Mathf.Log10(level) * 20f);
	}

	public void SetMusicVolume(float level)
	{
		m_mixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
	}
}
