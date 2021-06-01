using System;
using UnityEditor.Audio;
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

    public void Play (string name, bool loop)
    {
        Sound s = Array.Find(audioClips, sound => sound.name == name);
        s.source.Play();
        s.source.loop = loop;
    }

    void Start()
    {
        Play("Layer1", true);
    }
}
