using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Equipement : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField, Range(0.01f, 9.99f)] private float slowScale;
    [SerializeField, Range(1f, 10f)] private float dmgMultiplicator; 

    [Header("Combo 1")]
    public string combo1Effect;
    [Tooltip("Duration in phase number"), Range(1, 10)] public int combo1Duration = 1;
    [Range(1, 3)] public int[] combo1Notes;

    [Header("Combo 2")]
    public string combo2Effect;
    [Tooltip("Duration in phase number"), Range(1, 10)] public int combo2Duration = 1;
    [Range(1, 3)] public int[] combo2Notes;

    [Header("Combo 3")]
    public string combo3Effect;
    [Tooltip("Duration in phase number"), Range(1, 10)] public int combo3Duration = 1;
    [Range(1, 3)] public int[] combo3Notes;

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

            activeBonusesUI.BonusFeedback(activeBonusesUI.slowTimeSprite);
        }
    }

    public void Defense()
    {
        //Active un sprite sur la barre de vie su joueur
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
