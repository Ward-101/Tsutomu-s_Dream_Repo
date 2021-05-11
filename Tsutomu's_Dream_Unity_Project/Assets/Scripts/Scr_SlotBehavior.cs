using UnityEngine;

public class Scr_SlotBehavior : MonoBehaviour
{
    [Header("Edit")]
    [Tooltip("A = 0 / B = 1 / X = 2 / Y = 3"), Range(0,3)] public int inputIndex = 0;
    public GameObject[] linkedSlot;

    [HideInInspector] public int[] possibleInput;

    private Scr_Controller controller;

    private void Start()
    {
        #region INSTANCES ACQUIREMENT
        controller = Scr_Controller.instance;
        #endregion

        possibleInput = new int[linkedSlot.Length];

        //Check every linked slot to make a list of all possible input the player can press to procede to the next slot
        if (linkedSlot.Length > 0)
        {
            for (int i = 0; i < linkedSlot.Length; i++)
            {
                possibleInput[i] = linkedSlot[i].GetComponent<Scr_SlotBehavior>().inputIndex;
            }
        }
    }
}
