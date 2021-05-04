using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PhaseManager : MonoBehaviour
{
    public static Scr_PhaseManager instance = null;

    [Header("DON'T TOUCH")]
    public bool isActivePhase = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }


}
