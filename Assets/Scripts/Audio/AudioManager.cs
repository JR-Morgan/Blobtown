using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] audioClips;

    void Awake()
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
        }

        s.source.loop = loop;

        s.source.volume = 0f;

        StartCoroutine(FadeAudioSource.StartFade(s.source, 10f, volume));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Play("Layer1", true, 1);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Play("Layer2", true, 1);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Play("Layer3", true, 1);

        }

    }

    void Start()
    {
        Play("Layer1", true, 0);
        Play("Layer2", true, 0);
        Play("Layer3", true, 0);
    }
}
