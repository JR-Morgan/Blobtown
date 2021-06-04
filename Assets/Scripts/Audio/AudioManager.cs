using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    public Sound[] audioClips;


    protected override void Awake()
    {
        foreach (Sound clip in audioClips)
        {
            clip.source = gameObject.AddComponent<AudioSource>();
            clip.source.clip = clip.clip;

            clip.source.volume = clip.Volume;
        }
    }

    public void Play (string name, bool loop, float volume)
    {
        Sound s = Array.Find(audioClips, sound => sound.name == name);
        if (!s.source.isPlaying)
        {
            s.source.Play();
            s.source.loop = loop;

            s.source.volume = 0f;

            StartCoroutine(FadeAudioSource.StartFade(s.source, 4f, volume));

        }
    }

    void Start()
    {
        Play("Layer1", true, 1);
        Play("Layer2", true, 0);
        Play("Layer3", true, 0);
    }

    public void PlayLayer2()
    {
        Play("layer2", true, 1);
    }

    public void PlayLayer3()
    {
        Play("layer3", true, 1);
    }

}
