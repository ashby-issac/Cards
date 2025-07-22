using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AudioType
{
    CardMatch,
    CardMismatch,
    CardFlip,
    GameOver
}

[System.Serializable]
public class AudioData
{
    public AudioType type;
    public AudioClip clip;
}

public class SFXManagger : MonoBehaviour
{
    [SerializeField] private AudioData[] audioDatas;
    [SerializeField] private AudioSource audioSource;

    private Dictionary<AudioType, AudioClip> clipsDict = new Dictionary<AudioType, AudioClip>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        foreach (var data in audioDatas)
            clipsDict.Add(data.type, data.clip);
    }

    public void PlaySound(AudioType type)
    {
        if (clipsDict.ContainsKey(type))
        {
            audioSource.clip = clipsDict[type];
            audioSource.Play();
        }
    }
}
