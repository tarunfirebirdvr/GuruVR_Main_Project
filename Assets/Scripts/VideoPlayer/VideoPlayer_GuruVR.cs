using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;

public class VideoPlayer_GuruVR : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    [SerializeField] private Button PlayBtn;
    private bool isPlaying = false;

    void Start()
    {
        PlayBtn.enabled = false;
        // PlayOrPauseVideo();
        PlayBtn.onClick.AddListener(PlayOrPauseVideo); // ✅ Use method reference instead of string
    }

   /* public void PlayOrPauseVideo()
    {
        string videoPath = Path.Combine(Application.streamingAssetsPath, "bun33s.mp4");
        videoPlayer.url = "https://github.com/akhileshmali/College/raw/refs/heads/main/What%20is%20Electromagnetic%20Induction.mp4";

        if (!isPlaying)
        {
            videoPlayer.Play();
            isPlaying = true;
        }
        else
        {
            videoPlayer.Pause();
            isPlaying = false;
        }
    }*/
    public void PlayOrPauseVideo()
    {
        string videoPath = Path.Combine(Application.streamingAssetsPath, "bun33s.mp4");
       // videoPlayer.url = "https://res.cloudinary.com/doozyqyu8/video/upload/v1744873267/bun33s_jhdcxe.mp4";
        videoPlayer.url = "https://res.cloudinary.com/doozyqyu8/video/upload/v1744895135/Welcome_to_FireBirdVR_rsubvn.mp4";

        if (!isPlaying)
        {
            videoPlayer.Play();
           
            PlayBtn.gameObject.SetActive(false);
            isPlaying = true;
        }
       /* else
        {
            videoPlayer.Pause();
            isPlaying = false;
        }*/
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayBtn.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            videoPlayer.Pause();
            PlayBtn.gameObject.SetActive(true);
            PlayBtn.enabled = false;
        }
    }
}
