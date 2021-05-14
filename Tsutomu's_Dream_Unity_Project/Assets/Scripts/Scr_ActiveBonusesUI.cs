using UnityEngine;
using UnityEngine.UI;

public class Scr_ActiveBonusesUI : MonoBehaviour
{
    [Header("Requirements")]
    public Sprite slowTimeSprite;
    public Sprite defUpSprite;
    public Sprite dmgUpSprite;

    [HideInInspector] public GameObject[] bonusSlots;

    public static Scr_ActiveBonusesUI instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        bonusSlots = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            bonusSlots[i] = transform.GetChild(i).gameObject;
            bonusSlots[i].SetActive(false);
        }
    }

    public void BonusFeedback(Sprite bonusSprite)
    {
        for (int i = 0; i < bonusSlots.Length; i++)
        {
            if (!bonusSlots[i].activeInHierarchy)
            {
                bonusSlots[i].SetActive(true);
                bonusSlots[i].GetComponent<Image>().sprite = bonusSprite;
                break;
            }
        }
    }

    public void EndBonusFeedback(Sprite bonusSprite)
    {
        for (int i = 0; i < bonusSlots.Length; i++)
        {
            if (bonusSlots[i].activeInHierarchy && bonusSlots[i].GetComponent<Image>().sprite == bonusSprite)
            {
                bonusSlots[i].SetActive(false);
                break;
            }
        }
    }
}
