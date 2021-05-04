using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Partition : MonoBehaviour
{
    [SerializeField] private GameObject startSlot = null;

    [Header("DON'T TOUCH")]
    public GameObject activeSlot;

    public static Scr_Partition instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        activeSlot = startSlot;
    }
}
