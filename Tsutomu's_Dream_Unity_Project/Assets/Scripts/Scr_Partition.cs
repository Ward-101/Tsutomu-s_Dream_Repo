using UnityEngine;

public class Scr_Partition : MonoBehaviour
{
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
