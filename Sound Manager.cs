using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioClip gameplayBGM;


    public AudioSource bgMusicSource;
    public AudioClip bgMusicClip;


    public AudioClip eatSound;
    public AudioClip screamSound;
    


    public AudioSource sfxSource;
    public AudioClip gameOverSound;
    public AudioSource asfxSource;
    public AudioClip buttonClickSound;
    private void Start()
    {
        PlayBackgroundMusic();
    }
    public void PlayBackgroundMusic()
    {
        if (bgMusicSource != null && bgMusicClip != null)
        {


            bgMusicSource.clip = bgMusicClip;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }
        
    }
    public void PlayBGM()
    {
        if (!bgMusicSource.isPlaying)
            bgMusicSource.Play();
    }


    public void StopBackgroundMusic()
    {
        if (bgMusicSource != null && bgMusicSource.isPlaying)
        {
            bgMusicSource.Stop();
        }
    }



    private void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayGameplayBGM()
    {
        if (bgmSource && gameplayBGM)
        {
            bgmSource.clip = gameplayBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        if (bgmSource && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }
    // ✅ New method to allow volume control
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
    public void PlayButtonClick()
    {
        if (sfxSource && buttonClickSound)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or ButtonClickSound!");
        }
    }
    public void OnStartGame()
    {
        SoundManager.Instance.PlayGameplayBGM();
        // other logic to start the game
    }
    public void PlayGameOverSound()
    {
        if (gameOverSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(gameOverSound);
        }
    }
}
