using UnityEngine;

public class Scr_Partition : MonoBehaviour
{
    [Header("Combo Editor")]
    public string combo1Effect;
    public int[] combo1Notes;
    public string combo2Effect;
    public int[] combo2Notes;
    public string combo3Effect;
    public int[] combo3Notes;

    [Header("Edit")]
    public GameObject startSlot;

    [Header("DON'T TOUCH")]
    public GameObject activeSlot;

    public static Scr_Partition instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        //Set first active slot to the start slot
        activeSlot = startSlot;
    }
}
