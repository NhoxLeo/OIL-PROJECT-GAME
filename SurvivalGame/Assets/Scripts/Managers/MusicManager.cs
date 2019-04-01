using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioClip[] tracks;
    private AudioSource audioSrc;
    private bool isPlayingMusic;
    private float timer, maxTime = 15f;

    public void Start() {
        audioSrc = GetComponent<AudioSource>();
        timer = Random.Range(0, 5);
    }

    public void Update() {
        if (!audioSrc.isPlaying)
            isPlayingMusic = false;
        if (!isPlayingMusic) {
            timer -= Time.deltaTime;
            if (timer <= 0)
                PlayMusic();
        }
    }

    public void PlayMusic() {
        if (!isPlayingMusic) {
            audioSrc.PlayOneShot(tracks[Random.Range(0, tracks.Length)]);
            isPlayingMusic = true;
            timer = Random.Range(0, maxTime);
        }
    }
}
