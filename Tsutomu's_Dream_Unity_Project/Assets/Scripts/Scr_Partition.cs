using UnityEngine;

public class Scr_Partition : MonoBehaviour
{
    [Header("DON'T TOUCH")]
    public GameObject startSlot;
    public GameObject activeSlot;

    public static Scr_Partition instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        //Set startSlot
        startSlot = transform.GetChild(0).gameObject;

        //Set first active slot to the start slot
        activeSlot = startSlot;
    }


    [ContextMenu("Randomize the note in each slot of the partition")]
    public void RandomizePartition()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Scr_SlotBehavior>().inputIndex = Random.Range(0, 4);
        }
    }
}
