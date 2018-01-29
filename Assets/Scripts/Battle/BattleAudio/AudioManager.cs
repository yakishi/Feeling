using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    AudioClip[] audioClip;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip BGMClip;

    [SerializeField]
    AudioSource BGMSource;

    AudioClip tmpSource;

    // Use this for initialization
    void Start()
    {
        BGMSource.clip = BGMClip;
        BGMSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Play()
    {
        audioSource.clip = tmpSource;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void AttackSE(int i = 0)
    {
        tmpSource = audioClip[i];
        Play();
    }
}
