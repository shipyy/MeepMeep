using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundClip
{
    public AudioClip clip;

    public string name;
    [Range(0,1)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
