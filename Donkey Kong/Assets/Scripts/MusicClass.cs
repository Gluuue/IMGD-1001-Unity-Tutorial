using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private AudioSource _audioSource;


    private void Awake() {
        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic() {
        if (_audioSource.isPlaying) {
            return;
        } else {
            _audioSource.Play();
        }
    }

    public void StopMusic() {
        _audioSource.Stop();
    }



}
