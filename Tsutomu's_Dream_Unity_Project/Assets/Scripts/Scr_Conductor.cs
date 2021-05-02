using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Conductor : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField] private float bpm = 120.0f;

    [Header("DON'T TOUCH")]
    public float songPositionInBeats;
    public int beatCount = 0;

    private int lastBeatCount = 0;
    [HideInInspector] public float secPerBeat;
    private float songPosition;
    private float dspSongTime;
    private float timeSinceLastBeat;
    private float currentBeatPosition = 0f;

    private AudioSource audioSource;
    public static Scr_Conductor instance = null;

    public delegate void HandleTick();

    public event HandleTick Ticked;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        secPerBeat = 60f / bpm;

        dspSongTime = (float)AudioSettings.dspTime;

        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPosition / secPerBeat;

        beatCount = Mathf.FloorToInt(songPositionInBeats);

        if (beatCount > lastBeatCount)
        {
            if (Ticked != null)
            {
                Ticked();
            }

            lastBeatCount = beatCount;
        }
    }
}
