using UnityEngine;

public class AnimController : MonoBehaviour
{
    public AudioSource audioSource;        // The audio source playing clips
    public Animator animator;              // Animator to play animations
    public AnimationClip[] animationClips; // List of available animation clips

    private string lastPlayedClipName = "";

    void Update()
    {
        if (audioSource != null && animator != null && audioSource.isPlaying)
        {
            // Check if the audio clip changed
            if (audioSource.clip != null && audioSource.clip.name != lastPlayedClipName)
            {
                string currentClipName = audioSource.clip.name;
                PlayAnimationByName(currentClipName);
                lastPlayedClipName = currentClipName;
            }
        }
    }

    private void PlayAnimationByName(string clipName)
    {
        // Try to find the matching animation clip by name
        AnimationClip foundClip = null;

        foreach (var clip in animationClips)
        {
            if (clip != null && clip.name == clipName)
            {
                foundClip = clip;
                break;
            }
        }

        if (foundClip != null)
        {
            animator.Play(foundClip.name);
            Debug.Log($"Playing animation: {foundClip.name}");
        }
        else
        {
            Debug.LogWarning($"No matching animation found for audio clip: {clipName}");
        }
    }
}
