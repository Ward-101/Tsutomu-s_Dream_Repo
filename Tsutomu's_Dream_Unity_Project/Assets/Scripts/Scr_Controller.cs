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

    public static Scr_Controller instance = null;
    private Scr_Conductor conductor;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;
    }

    private void Update()
    {
        GetInput();
        InputEvent();
    }

    private void GetInput()
    {
        aInput = Input.GetButtonDown("Xbox_A");
        bInput = Input.GetButtonDown("Xbox_B");
        xInput = Input.GetButtonDown("Xbox_X");
        yInput = Input.GetButtonDown("Xbox_Y");
    }

    private void InputEvent()
    {
        if (aInput || bInput || xInput || yInput)
        {
            if ((0f < conductor.songPositionInBeats % 1 && conductor.songPositionInBeats % 1 < inputTiming) || (1f - inputTiming < conductor.songPositionInBeats % 1 && conductor.songPositionInBeats % 1 < 1f))
            {
                Debug.Log("yas");
            }
            else
            {
                Debug.Log("nase");
            }
        }
    }
}
