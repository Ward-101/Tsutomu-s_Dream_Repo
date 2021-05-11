using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_DummyHealthManager : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private int extraLife = 99;

    [Header("Requirements")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text extraLifeText;

    [Header("DON'T TOUCH")]
    [SerializeField] private float currentHp;
    [SerializeField] private float remainingExtraLife;

    private Scr_Controller controller;

    public static Scr_DummyHealthManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        #region INSTANCES ACQUIREMENT
        controller = Scr_Controller.instance;
        #endregion

        currentHp = maxHp;
        remainingExtraLife = extraLife;

        hpSlider.value = currentHp / maxHp;
        extraLifeText.text = extraLife.ToString();
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp / maxHp;
        
        if (currentHp <= 0)
        {
            controller.EndPhase();

            extraLife--;
            extraLifeText.text = extraLife.ToString();

            if (extraLife <= 0)
            {
                print("You Win");
            }
            else
            {
                currentHp = maxHp;
                hpSlider.value = currentHp / maxHp;
            }
        }
    }
}
