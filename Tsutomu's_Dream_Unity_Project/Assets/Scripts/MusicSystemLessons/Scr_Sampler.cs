using System.Collections;
using UnityEngine;

public class Scr_Sampler : MonoBehaviour
{
    //[Header("Debug")]
    //[SerializeField] private bool debugPlay;

    [Header("Config")]
    [SerializeField] private Scr_MetronomeV2 metronome;
    [SerializeField] private AudioClip audioClip;
    [SerializeField, Range(1, 8)] private int nbrVoices = 2;
    [SerializeField] private Scr_SamplerVoice samplerVoicePrefab;
    [SerializeField, Range(0f, 2f)] private double attackTime;
    [SerializeField, Range(0f, 2f)] private double sustainTime;
    [SerializeField, Range(0f, 2f)] private double releaseTime;

    private Scr_SamplerVoice[] samplerVoices;
    private int nextVoiceIndex;

    private void Awake()
    {
        samplerVoices = new Scr_SamplerVoice[nbrVoices];

        for (int i = 0; i < nbrVoices; i++)
        {
            Scr_SamplerVoice samplerVoice = Instantiate(samplerVoicePrefab);

            // ??
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;
            // ??

            samplerVoices[i] = samplerVoice;
        }

        nextVoiceIndex = 0;
    }

    private void OnEnable()
    {
        if (metronome != null)
        {
            metronome.Ticked += HandleTicked;
        }
    }

    private void OnDisable()
    {
        if (metronome != null)
        {
            metronome.Ticked -= HandleTicked;
        }
    }

    private void HandleTicked(double tickTime)
    {
        samplerVoices[nextVoiceIndex].Play(audioClip, tickTime, attackTime, sustainTime, releaseTime);

        nextVoiceIndex = (nextVoiceIndex + 1) % samplerVoices.Length;
    }


    //private void Update()
    //{
    //    if (debugPlay)
    //    {
    //        Play();

    //        debugPlay = false;
    //    }
    //}

    //private void Play()
    //{
    //    samplerVoices[nextVoiceIndex].Play(audioClip, attackTime, sustainTime, releaseTime);

    //    nextVoiceIndex++;
    //    if(nextVoiceIndex >= samplerVoices.Length)
    //    {
    //        nextVoiceIndex = 0;
    //    }
    //}
}
