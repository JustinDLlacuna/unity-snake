using System.Collections.Generic;
using UnityEngine;

public class BeepPlayer : MonoBehaviour
{
    private static BeepPlayer instance;

    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> beeps;

    public static BeepPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BeepPlayer>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(BeepPlayer).Name;
                    instance = obj.AddComponent<BeepPlayer>();
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
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; 
        audioSource.Stop(); 
    }

    public void Play()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.PlayOneShot(beeps[Random.Range(0, beeps.Count)]);
    } 
}

