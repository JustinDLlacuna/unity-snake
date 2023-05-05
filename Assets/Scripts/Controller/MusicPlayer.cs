using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private bool playMusic;

    private static MusicPlayer instance;

    private AudioSource soundEffectAudioSource;
    private AudioSource musicAudioSource;

    [SerializeField] private List<AudioClip> beeps;

    [SerializeField] private List<AudioClip> gOver;

    [SerializeField] private List<AudioClip> music;
    public static MusicPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicPlayer>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(MusicPlayer).Name;
                    instance = obj.AddComponent<MusicPlayer>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soundEffectAudioSource = gameObject.AddComponent<AudioSource>();
        soundEffectAudioSource.playOnAwake = false;
        soundEffectAudioSource.spatialBlend = 0; 
        soundEffectAudioSource.Stop();

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.playOnAwake = false;
        musicAudioSource.spatialBlend = 0;
        musicAudioSource.Stop();
    }

    private void Update()
    {
        if (playMusic && !musicAudioSource.isPlaying)
        {
            PlayMusic();
        }
    }

    public void PlayEatFruit()
    {
        soundEffectAudioSource.PlayOneShot(beeps[Random.Range(0, beeps.Count)]);
    } 

    public void PlayGameOver()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
            playMusic = false;
        }

        soundEffectAudioSource.PlayOneShot(gOver[Random.Range(0, gOver.Count)]);
    }

    public void PlayMusic()
    {
        playMusic = true;
        musicAudioSource.clip = music[Random.Range(0, music.Count)];
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        playMusic = false;

        if (musicAudioSource == null)
            return;

        musicAudioSource.Stop();
    }
}

