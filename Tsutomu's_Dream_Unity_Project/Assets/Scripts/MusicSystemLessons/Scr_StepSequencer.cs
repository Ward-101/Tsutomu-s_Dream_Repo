using System.Collections.Generic;
using UnityEngine;
using System;

public class Scr_StepSequencer : MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public bool active;
        public int midiNoteNumber;
        public double duration;
    }

    public delegate void HandleTick(double tickTime, int midiNoteNumber, double duration);

    public event HandleTick Ticked;

    [SerializeField] private Scr_MetronomeV2 metronome;
    [SerializeField, HideInInspector] private List<Step> steps;

    private int currentTick;

#if UNITY_EDITOR
    public List<Step> GetSteps()
    {
        return steps;
    }
#endif

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

    public void HandleTicked(double tickTime)
    {
        int nbrSteps = steps.Count;

        if (nbrSteps == 0)
        {
            return;
        }

        Step step = steps[currentTick];

        if (step.active)
        {
            if (Ticked != null)
            {
                Ticked(tickTime, step.midiNoteNumber, step.duration);
            }
        }

        currentTick = (currentTick + 1) % nbrSteps;

    }


}
