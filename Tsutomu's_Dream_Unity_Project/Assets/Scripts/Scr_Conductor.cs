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

    private Scr_PossibleInputUI possibleInputUI;

    public static Scr_Conductor instance = null;

    public delegate void HandleTick();
    public event HandleTick Ticked;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        possibleInputUI = Scr_PossibleInputUI.instance;

        secPerBeat = 60f / bpm;

        dspSongTime = (float)AudioSettings.dspTime;

        GetComponent<AudioSource>().Play();
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
