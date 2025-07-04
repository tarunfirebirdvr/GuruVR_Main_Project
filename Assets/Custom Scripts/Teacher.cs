using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : MonoBehaviour
{
    [Header("References")]
    public Animator teacherAnimator;
    public AudioSource audioSource;

    [Header("Data")]
    public List<RuntimeAnimatorController> animationControllers;
    public List<AudioClip> audioClips;
    public RuntimeAnimatorController idleController;

    private int currentStepIndex = 0;
    private bool isPlaying = false;

    private void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        GoToIdle(); // Start with idle
    }

    public void PlayNextStep()
    {
        if (isPlaying || currentStepIndex >= animationControllers.Count)
            return;

        StartCoroutine(PlayStepCoroutine());
    }

    IEnumerator PlayStepCoroutine()
    {
        isPlaying = true;

        // Set the new animator controller
        if (teacherAnimator && animationControllers[currentStepIndex])
        {
            teacherAnimator.runtimeAnimatorController = animationControllers[currentStepIndex];
        }

        // Play the corresponding audio
        if (audioSource && currentStepIndex < audioClips.Count && audioClips[currentStepIndex])
        {
            audioSource.clip = audioClips[currentStepIndex];
            audioSource.Play();
        }

        // Get the length of the animation
        float animLength = 0f;
        if (teacherAnimator && teacherAnimator.runtimeAnimatorController)
        {
            AnimationClip[] clips = teacherAnimator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
                animLength = clips[0].length;
        }

        // Wait until both animation and audio finish
        float waitTime = Mathf.Max(animLength, audioSource.clip != null ? audioSource.clip.length : 0f);
        yield return new WaitForSeconds(waitTime);

        // Switch back to idle after animation
        GoToIdle();

        currentStepIndex++;
        isPlaying = false;
    }

    private void GoToIdle()
    {
        if (teacherAnimator && idleController)
        {
            teacherAnimator.runtimeAnimatorController = idleController;
        }
    }

    public void ResetSteps()
    {
        currentStepIndex = 0;
        GoToIdle();
    }
}
