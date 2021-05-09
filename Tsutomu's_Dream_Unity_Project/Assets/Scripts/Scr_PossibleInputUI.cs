using UnityEngine;
using UnityEngine.UI;

public class Scr_PossibleInputUI : MonoBehaviour
{
    private Scr_Partition partition;

    private Image aInputImage;
    private Image bInputImage;
    private Image xInputImage;
    private Image yInputImage;

    public static Scr_PossibleInputUI instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        partition = Scr_Partition.instance;

        aInputImage = transform.GetChild(1).GetComponent<Image>();
        bInputImage = transform.GetChild(2).GetComponent<Image>();
        xInputImage = transform.GetChild(3).GetComponent<Image>();
        yInputImage = transform.GetChild(4).GetComponent<Image>();

        DisplayPossibleInput();
    }

    public void DisplayPossibleInput()
    {
        //Get the current active slot to later check for possible inputs
        Scr_SlotBehavior activeSlot = partition.activeSlot.GetComponent<Scr_SlotBehavior>();

        HideAllPossibleInput();

        //Run through every possible inputs and change the sprite of the corresponding input to ON
        for (int i = 0; i < activeSlot.possibleInput.Length; i++)
        {

            if (activeSlot.possibleInput[i] == 0)
            {
                aInputImage.sprite = aInputImage.GetComponent<Scr_SpriteSwap>().spriteA;
            }
            else if (activeSlot.possibleInput[i] == 1)
            {
                bInputImage.sprite = bInputImage.GetComponent<Scr_SpriteSwap>().spriteA;
            }
            else if (activeSlot.possibleInput[i] == 2)
            {
                xInputImage.sprite = xInputImage.GetComponent<Scr_SpriteSwap>().spriteA;
            }
            else if (activeSlot.possibleInput[i] == 3)
            {
                yInputImage.sprite = yInputImage.GetComponent<Scr_SpriteSwap>().spriteA;
            }
        }
    }

    /// <summary>
    /// Set all inputs display state to OFF
    /// </summary>
    public void HideAllPossibleInput()
    {

        aInputImage.sprite = aInputImage.GetComponent<Scr_SpriteSwap>().spriteB;
        bInputImage.sprite = bInputImage.GetComponent<Scr_SpriteSwap>().spriteB;
        xInputImage.sprite = xInputImage.GetComponent<Scr_SpriteSwap>().spriteB;
        yInputImage.sprite = yInputImage.GetComponent<Scr_SpriteSwap>().spriteB;
    }

}
