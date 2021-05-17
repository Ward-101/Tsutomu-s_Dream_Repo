using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Equipement : MonoBehaviour
{
    [Header("Combo 1 : DON'T TOUCH")]
    public string combo1Effect;
    [Tooltip("Duration in phase number"), Range(1, 10)] public int combo1Duration = 1;
    [Header("Combo 1 : EDIT")]
    [Range(0, 3)] public int[] combo1Notes;

    [Header("Combo 2 : DON'T TOUCH")]
    public string combo2Effect;
    [Tooltip("Duration in phase number"), Range(1, 10)] public int combo2Duration = 1;
    [Header("Combo 2 : EDIT")]
    [Range(0, 3)] public int[] combo2Notes;

    [Header("Combo 3 : DON'T TOUCH")]
    public string combo3Effect;
    [Tooltip("Duration in phase number"), Range(1, 10)] public int combo3Duration = 1;
    [Header("Combo 3 : EDIT")]
    [Range(0, 3)] public int[] combo3Notes;

    [Header("Edit : DON'T TOUCH")]
    [SerializeField, Range(0.01f, 9.99f)] private float slowScale;
    [SerializeField, Range(1f, 10f)] private float dmgMultiplicator;

    [Header("States : DON'T TOUCH")]
    public bool isCombo1;
    public bool isCombo2;
    public bool isCombo3;

    [HideInInspector] public int combo1StartPhase;
    [HideInInspector] public int combo2StartPhase;
    [HideInInspector] public int combo3StartPhase;

    private Scr_Conductor conductor;
    private Scr_Controller controller;
    private Scr_ActiveBonusesUI activeBonusesUI;

    public static Scr_Equipement instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;
        controller = Scr_Controller.instance;
        activeBonusesUI = Scr_ActiveBonusesUI.instance;
    }

    public void StartSlowtime()
    {
        if (!isCombo1)
        {
            float slowTimeBPM = conductor.bpm - (conductor.bpm * slowScale);
            conductor.RecalculateBPM(slowTimeBPM);
            conductor.songAudioSource.pitch -= slowScale;

            activeBonusesUI.BonusFeedback(activeBonusesUI.slowTimeSprite);

            combo1StartPhase = conductor.phaseCount;

            isCombo1 = true;
        }
    }

    private void EndSlowTime()
    {
        if (isCombo1)
        {
            float normalBPM = conductor.bpm + (conductor.bpm * slowScale);
            conductor.RecalculateBPM(normalBPM);
            conductor.songAudioSource.pitch += slowScale;

            activeBonusesUI.EndBonusFeedback(activeBonusesUI.slowTimeSprite);
        }
    }

    public void StartDefense()
    {
        if (!isCombo2)
        {
            activeBonusesUI.BonusFeedback(activeBonusesUI.defUpSprite);

            combo2StartPhase = conductor.phaseCount;

            isCombo2 = true;
        }
    }

    public void EndDefense()
    {
        if (isCombo2)
        {
            activeBonusesUI.EndBonusFeedback(activeBonusesUI.defUpSprite);
        }
    }

    public void StartDmgUp()
    {
        if (!isCombo3)
        {
            controller.activeDmg = controller.baseDamage * dmgMultiplicator;
            combo3StartPhase = conductor.phaseCount;

            activeBonusesUI.BonusFeedback(activeBonusesUI.dmgUpSprite);

            isCombo3 = true;
        }
    }

    private void EndDmgUP()
    {
        controller.activeDmg = controller.baseDamage;

        activeBonusesUI.EndBonusFeedback(activeBonusesUI.dmgUpSprite);
    }

    public void EndComboBonus()
    {
        if (isCombo1)
        {
            if (conductor.phaseCount - combo1StartPhase >= combo1Duration)
            {
                EndSlowTime();
                isCombo1 = false;
            }
        }

        if (isCombo2)
        {
            if (conductor.phaseCount - combo2StartPhase >= combo2Duration)
            {
                EndDefense();
                isCombo2 = false;
            }
        }

        if (isCombo3)
        {
            if (conductor.phaseCount - combo3StartPhase >= combo3Duration)
            {
                EndDmgUP();
                isCombo3 = false;
            }
        }
    }
}
