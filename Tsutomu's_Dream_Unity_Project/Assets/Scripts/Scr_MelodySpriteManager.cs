using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MelodySpriteManager : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField] private float attackAnimTimeScale;

    [Header("Requirements")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite attackSprite;

    [Header("States : DON'T TOUCH")]
    public bool isAttack;

    private float currentTime;
    private float attackAnimStartTime;

    private SpriteRenderer spriteRenderer;

    private Scr_Conductor conductor;

    public static Scr_MelodySpriteManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartAttackAnim()
    {
        attackAnimStartTime = (float)AudioSettings.dspTime;
        isAttack = true;
    }

    private void Update()
    {
        if (isAttack)
        {
            if (spriteRenderer.sprite != attackSprite)
            {
                spriteRenderer.sprite = attackSprite;
            }

            currentTime = (float)AudioSettings.dspTime;

            if (currentTime - attackAnimStartTime >= (conductor.secPerBeat * 0.5) * attackAnimTimeScale)
            {
                isAttack = false;
            }
        }
        else
        {
            if (spriteRenderer.sprite != idleSprite)
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
    }
}
