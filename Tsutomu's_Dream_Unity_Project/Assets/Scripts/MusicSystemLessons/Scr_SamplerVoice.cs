using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Scr_SamplerVoice : MonoBehaviour
{
    private AudioSource audioSource;
    private uint samplesUntilEnvelopeTrigger;

    private readonly Scr_ASREnvelope envelope = new Scr_ASREnvelope();

    public void Play(AudioClip audioClip,double startTime ,double attackTime, double sustainTime, double releaseTime)
    {
        sustainTime = (sustainTime > attackTime) ? (sustainTime - attackTime) : 0.0;
        envelope.Reset(attackTime, sustainTime, releaseTime, AudioSettings.outputSampleRate);

        double timeUntilTrigger = (startTime > AudioSettings.dspTime) ? (startTime - AudioSettings.dspTime) : 0.0;
        samplesUntilEnvelopeTrigger = (uint)(timeUntilTrigger * AudioSettings.outputSampleRate);

        audioSource.clip = audioClip;
        audioSource.PlayScheduled(startTime);
    }


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnAudioFilterRead(float[] buffer, int nbrChannels)
    {
        for (int sIdx = 0; sIdx < buffer.Length; sIdx += nbrChannels)
        {
            double volume = 0;

            if (samplesUntilEnvelopeTrigger == 0)
            {
                volume = envelope.GetLevel();
            }
            else
            {
                --samplesUntilEnvelopeTrigger;
            }
            

            for (int cIdx = 0; cIdx < nbrChannels; cIdx++)
            {
                buffer[sIdx + cIdx] *= (float)volume;
            }
        }
    }

}
