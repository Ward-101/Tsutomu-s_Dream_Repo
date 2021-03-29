using UnityEngine;

public class Scr_AudioManager : MonoBehaviour
{
    public static Scr_AudioManager instance = null;

    [Header("AudioSources")]
    public AudioSource[] listOfSources;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void PlayBeatSFX()
    {
        listOfSources[0].Play();
    }
}
