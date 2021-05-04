using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Controller : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField, Range(0.001f, 0.499f)] private float inputTiming = 0.1f;

    [Header("Inputs")]
    [SerializeField] private bool aInput = false;
    [SerializeField] private bool bInput = false;
    [SerializeField] private bool xInput = false;
    [SerializeField] private bool yInput = false;

    [Header("States")]
    [SerializeField] private bool isLookingForInputs = false;
    [SerializeField] private int inputIndex = -1;

    public static Scr_Controller instance = null;
    private Scr_Conductor conductor;
    private Scr_Partition partition;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;
        partition = Scr_Partition.instance;
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
        if ((0f < conductor.beatProgression && conductor.beatProgression < inputTiming) || (1f - inputTiming < conductor.beatProgression && conductor.beatProgression < 1f))
        {
            isLookingForInputs = true;
        }
        else
        {
            isLookingForInputs = false;
        }
    }


    private void InputEvent()
    {
        if (aInput || bInput || xInput || yInput)
        {
            if (isLookingForInputs)
            {
                Scr_SlotBehavior activeSlot = partition.activeSlot.GetComponent<Scr_SlotBehavior>();

                if (activeSlot.possibleInput.Length > 0)
                {
                    for (int i = 0; i < activeSlot.possibleInput.Length; i++)
                    {
                        if (inputIndex == activeSlot.possibleInput[i])
                        {
                            partition.activeSlot = activeSlot.linkedSlot[i];
                            break;
                        }
                        else if (inputIndex != activeSlot.possibleInput[i] && i == activeSlot.possibleInput.Length - 1)
                        {
                            print("ENDPHASE 1");
                        }
                    }
                }
                else
                {
                    print("ENDPHASE 2");
                }
                
            }
            else
            {
                print("ENDPHASE 3");
            }
        }
    }
}
