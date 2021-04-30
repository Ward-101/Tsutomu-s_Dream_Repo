using System.Collections;
using UnityEngine;

public class Scr_Sampler : MonoBehaviour
{
    //[Header("Debug")]
    //[SerializeField] private bool debugPlay;

    [Header("Config")]
    [SerializeField] private Scr_StepSequencer sequencer;
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

            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;

            samplerVoices[i] = samplerVoice;
        }

        nextVoiceIndex = 0;
    }

    private void OnEnable()
    {
        if (sequencer != null)
        {
            sequencer.Ticked += HandleTicked;
        }
    }

    private void OnDisable()
    {
        if (sequencer != null)
        {
            sequencer.Ticked -= HandleTicked;
        }
    }

    private void HandleTicked(double tickTime, int midiNoteNumber, double duration)
    {
        float pitch = Scr_MusicMathUtils.MidiNoteToPitch(midiNoteNumber, Scr_MusicMathUtils.MidiNoteC4);
        samplerVoices[nextVoiceIndex].Play(audioClip, pitch, tickTime, attackTime, sustainTime, releaseTime);

        nextVoiceIndex = (nextVoiceIndex + 1) % samplerVoices.Length;
    }
}
