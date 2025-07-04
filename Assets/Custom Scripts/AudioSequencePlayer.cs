using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSequencePlayer : MonoBehaviour
{
    
    public AudioSource audioSource;          // Assign in inspector
    public AudioClip[] audioClips;           // List of clips to play in order
    public UnityEvent onSequenceComplete;    // Called after last clip finishes

    private int currentIndex = 0;
    private bool isPlaying = false;

    void Update()
    {
        // Check if current clip finished
        if (isPlaying && !audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }

    public void StartSequence()
    {
        currentIndex = 0;
        PlayNextClip();
    }

    private void PlayNextClip()
    {
        if (currentIndex < audioClips.Length)
        {
            audioSource.Stop(); // Ensure previous clip is stopped
            audioSource.clip = audioClips[currentIndex];
            audioSource.Play();
            isPlaying = true;
            currentIndex++;
        }
        else
        {
            isPlaying = false;
            onSequenceComplete?.Invoke(); // Trigger the event at the end
        }
    }
}

