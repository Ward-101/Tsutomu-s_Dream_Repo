using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Controller : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField, Range(0.001f, 0.499f)] private float inputTiming = 0.1f;
    [SerializeField, Range(0f, 100f)] private float baseDamage = 5f;
    [SerializeField, Range(0.001f, 1f)] private float zoomToBeatZoomScale = 1f;

    [Header("Inputs : DON'T TOUCH")]
    [SerializeField] private bool aInput = false;
    [SerializeField] private bool bInput = false;
    [SerializeField] private bool xInput = false;
    [SerializeField] private bool yInput = false;
    [SerializeField] private int inputIndex = -1;

    [Header("States : DON'T TOUCH")]
    [SerializeField] private bool isLookingForInputs = false;
    [SerializeField] private bool asHitNoteInBeat = false;

    [Header("DON'T TOUCH")]
    [SerializeField] private List<int> chain;

    public static Scr_Controller instance = null;
    private bool canResetAsHitNotInBeat = false;
     
    private Scr_Conductor conductor;
    private Scr_Partition partition;
    private Scr_PropsOnBeat propsOnBeat;
    private Scr_PossibleInputUI possibleInputUI;
    private Scr_DummyHealthManager dummyHealthManager;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;
        partition = Scr_Partition.instance;
        propsOnBeat = Scr_PropsOnBeat.instance;
        possibleInputUI = Scr_PossibleInputUI.instance;
        dummyHealthManager = Scr_DummyHealthManager.instance;

        chain = new List<int>();
    }

    private void Update()
    {
        LookForInput();
        GetInput();
        InputEvent();
    }

    private void GetInput()
    {
        aInput = Input.GetButtonDown("Xbox_A");
        bInput = Input.GetButtonDown("Xbox_B");
        xInput = Input.GetButtonDown("Xbox_X");
        yInput = Input.GetButtonDown("Xbox_Y");

        if (isLookingForInputs)
        {
            if (aInput) inputIndex = 0;
            else if (bInput) inputIndex = 1;
            else if (xInput) inputIndex = 2;
            else if (yInput) inputIndex = 3;
        }
    }

    private void LookForInput()
    {
        if (!conductor.isBreak)
        {
            if (conductor.beatProgression <= inputTiming || 1f - inputTiming <= conductor.beatProgression)
            {
                if (canResetAsHitNotInBeat)
                {
                    asHitNoteInBeat = false;
                    canResetAsHitNotInBeat = false;
                }


                isLookingForInputs = true;
            }
            else
            {
                isLookingForInputs = false;

                if (inputTiming < conductor.beatProgression && conductor.beatProgression < 1f - inputTiming)
                {
                    if (!asHitNoteInBeat)
                    {
                        EndPhase();
                    }
                    else
                    {
                        canResetAsHitNotInBeat = true;
                    }
                }
            }
        }
        
    }

    private void InputEvent()
    {
        if (!conductor.isBreak)
        {
            if (aInput || bInput || xInput || yInput)
            {
                if (isLookingForInputs)
                {
                    asHitNoteInBeat = true;
                    Scr_SlotBehavior activeSlot = partition.activeSlot.GetComponent<Scr_SlotBehavior>();

                    if (activeSlot.possibleInput.Length > 0)
                    {
                        for (int i = 0; i < activeSlot.possibleInput.Length; i++)
                        {
                            if (inputIndex == activeSlot.possibleInput[i])
                            {
                                propsOnBeat.StartZoomToBeat(zoomToBeatZoomScale);

                                partition.activeSlot = activeSlot.linkedSlot[i];
                                chain.Add(activeSlot.inputIndex);

                                possibleInputUI.DisplayPossibleInput();

                                dummyHealthManager.TakeDamage(baseDamage + (baseDamage * (0.2f * chain.Count - 1)));
                                break;
                            }
                            else if (inputIndex != activeSlot.possibleInput[i] && i == activeSlot.possibleInput.Length - 1)
                            {
                                EndPhase();
                            }
                        }
                    }
                    else
                    {
                        EndPhase();
                    }

                }
                else
                {
                    EndPhase();
                }
            }
        }
        
    }

    public void EndPhase()
    {
        //Start conductor's break phase
        conductor.isEndBreak = false;
        conductor.isBreak = true;

        //Rewind the partition
        partition.activeSlot = partition.startSlot;

        //Clear the note from the chain
        chain.Clear();

        //Set all possible input display to OFF
        possibleInputUI.HideAllPossibleInput();

        //Set break text to false in case of first note missed
        if (conductor.endBreakText.gameObject.activeInHierarchy)
        {
            conductor.endBreakText.gameObject.SetActive(false);
        }
    }
}
