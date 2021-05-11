using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_ChainInputUI : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite aChainInput;
    public Sprite bChainInput;
    public Sprite xChainInput;
    public Sprite yChainInput;

    [Header("DON'T TOUCH")]
    public Image[] chainInputSlots;

    public static Scr_ChainInputUI instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        chainInputSlots = new Image[transform.childCount];

        for (int i = 0; i < chainInputSlots.Length; i++)
        {
            chainInputSlots[i] = transform.GetChild(i).GetComponent<Image>();
            chainInputSlots[i].gameObject.SetActive(false);
        }
    }

    public void clearInputSlots()
    {
        for (int i = 0; i < chainInputSlots.Length; i++)
        {
            chainInputSlots[i].gameObject.SetActive(false);
        }
    }
}
