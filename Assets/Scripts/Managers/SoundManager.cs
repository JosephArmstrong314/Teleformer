using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    // Singleton Instance
    public static SoundManager Instance;

    // AudioMixers
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    // Array of Sounds (see Sound.cs for class definition)
    // This is where we should put ALL of the sounds for the WHOLE game
    [SerializeField] private Sound[] sounds;

    // Singeton Awake() pattern
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        // Initialize each Sound component to be used throughout the game
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            // Remember to set the audioType in the Inspector
            switch (s.audioType) {
                case Sound.AudioType.Music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
                case Sound.AudioType.SFX:
                    s.source.outputAudioMixerGroup = sfxMixerGroup;
                    break;
            }

            // Plays the background music (which should be set to playOnAwake)
            if (s.playOnAwake) {
                s.source.Play();
            }
        }
    }

    void Start() {
        float mainVolume = PlayerPrefs.GetFloat("MainVolume");
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        //Debug.Log("SoundManager::Start::mainVolume = " + mainVolume.ToString());
        //Debug.Log("SoundManager::Start::musicVolume = " + musicVolume.ToString());
        //Debug.Log("SoundManager::Start::sfxVolume = " + sfxVolume.ToString());

        AudioListener.volume = mainVolume;
        UpdateMusicAudioMixerVolume(musicVolume);
        UpdateSFXAudioMixerVolume(sfxVolume);
    }

    public void UpdateMusicAudioMixerVolume(float value) {
        SoundManager.Instance.musicMixerGroup.audioMixer.SetFloat("ExposedMusicVolume", Mathf.Log10(value) * 20);
    }

    public void UpdateSFXAudioMixerVolume(float value) {
        SoundManager.Instance.sfxMixerGroup.audioMixer.SetFloat("ExposedSFXVolume", Mathf.Log10(value) * 20);
    }

    public void PlayClipByName(string _clipName) {
        Sound soundToPlay = Array.Find(sounds, var => var.clipName == _clipName);
        soundToPlay.source.PlayOneShot(soundToPlay.audioClip);
    }
}