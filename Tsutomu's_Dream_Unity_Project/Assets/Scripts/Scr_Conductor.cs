using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_Conductor : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField] private float bpm = 120.0f;
    [SerializeField] private int breakBeatNbr = 3;
    [SerializeField] private int endBreakBeatNbr = 4;
    [SerializeField] private float breakSpatialBlend = 0.65f;

    [Header("States : DON'T TOUCH")]
    public bool isBreak = true;
    public bool isEndBreak = false;

    [Header("DON'T TOUCH")]
    public float songPositionInBeats;
    public float beatProgression;
    public int beatCount = 0;

    [Header("Requirements")]
    public TMP_Text endBreakText;

    private int breakCurrentBeat = 1;
    private int endBreakCurrentBeat = 1;
    private int lastBeatCount = 0;
    [HideInInspector] public float secPerBeat;
    private float songPosition;
    private float dspSongTime;
    private AudioSource songAudioSource;
    private AudioSource breakAudioSource;

    private Scr_PossibleInputUI possibleInputUI;
    private Scr_ChainInputUI chainInputUI;
    private Scr_Controller controller;

    public static Scr_Conductor instance = null;

    public delegate void HandleTick();
    public event HandleTick Ticked;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        breakAudioSource = transform.GetChild(0).GetComponent<AudioSource>();

        controller = Scr_Controller.instance;
        possibleInputUI = Scr_PossibleInputUI.instance;
        chainInputUI = Scr_ChainInputUI.instance;

        secPerBeat = 60f / bpm;

        dspSongTime = (float)AudioSettings.dspTime;

        songAudioSource = GetComponent<AudioSource>();
        songAudioSource.Play();
    }

    private void RecalculateBPM()
    {
        secPerBeat = 60f / bpm;
    }

    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPosition / secPerBeat;
        beatProgression = songPositionInBeats % 1;

        beatCount = Mathf.FloorToInt(songPositionInBeats);

        if (beatCount > lastBeatCount)
        {
            if (Ticked != null)
            {
                Ticked();
            }

            BreakManager();
            
            lastBeatCount = beatCount;
        }
    }

    private void BreakManager()
    {
        if (isBreak)
        {
            if (!isEndBreak)
            {
                if (breakCurrentBeat < breakBeatNbr - 1)
                {
                    breakCurrentBeat++;
                }
                else
                {
                    breakCurrentBeat = 1;
                    isEndBreak = true;
                }

                songAudioSource.spatialBlend = breakSpatialBlend;
            }
            else
            {
                EndBreak();
            }
        }
        else
        {
            endBreakText.gameObject.SetActive(false);
        }
    }

    private void EndBreak()
    {
        //Clear the note from the chain
        controller.chain.Clear();

        //Clear chainUI
        chainInputUI.clearInputSlots();

        //Reset the spatial blend
        songAudioSource.spatialBlend = 0f;

        breakAudioSource.Play();

        if (!endBreakText.gameObject.activeInHierarchy)
        {
            endBreakText.gameObject.SetActive(true);
            endBreakCurrentBeat = 1;

            possibleInputUI.DisplayPossibleInput();
        }

        if (endBreakCurrentBeat < endBreakBeatNbr)
        {
            endBreakText.text = endBreakCurrentBeat.ToString();
            endBreakCurrentBeat++;
        }
        else
        {
            endBreakText.text = "GO";
            isBreak = false;
            isEndBreak = false;
        }
    }
}
