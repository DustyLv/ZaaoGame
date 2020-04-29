using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource m_AudioSource;

    public AudioClip m_Click;
    public AudioClip m_LidOpen;
    public AudioClip m_LidClose;
    public AudioClip m_TrashThrow;
    public AudioClip m_GameStartSwitch;
    public AudioClip m_GameEndSwitch;
    public AudioClip m_Error;

    public static SoundController instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlayOneShot(string sound)
    {
        switch (sound)
        {
            case "Click":
                m_AudioSource.PlayOneShot(m_Click, 1f);
                break;
            case "LidOpen":
                m_AudioSource.PlayOneShot(m_LidOpen, 0.5f);
                break;
            case "LidClose":
                m_AudioSource.PlayOneShot(m_LidClose, 0.5f);
                break;
            case "TrashThrow":
                m_AudioSource.PlayOneShot(m_TrashThrow, 0.8f);
                break;
            case "GameStartSwitch":
                m_AudioSource.PlayOneShot(m_GameStartSwitch, 0.8f);
                break;
            case "GameEndSwitch":
                m_AudioSource.PlayOneShot(m_GameEndSwitch, 0.8f);
                break;
            case "Error":
                m_AudioSource.PlayOneShot(m_Error, 1f);
                break;
        }
    }
}
