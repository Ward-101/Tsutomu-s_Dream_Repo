using UnityEngine;

public class Scr_PropsOnBeat : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField, Range(0.001f, 1f)] private float zoomTimeScale = 1f;
    [SerializeField] private AnimationCurve zoomCurve;

    [Header("States")]
    public bool shouldScale;

    [Header("DON'T TOUCH")]
    [SerializeField] private GameObject[] objectsToScale;
    [SerializeField] private Vector2[] baseScales;

    private float zoomScale;
    private float timeStartedZoom;
    private Scr_Conductor conductor;
    private int spriteSwapNbr = 0;

    public static Scr_PropsOnBeat instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;

        objectsToScale = new GameObject[transform.childCount];
        baseScales = new Vector2[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            objectsToScale[i] = transform.GetChild(i).gameObject;
            baseScales[i] = new Vector2(objectsToScale[i].transform.localScale.x, objectsToScale[i].transform.localScale.y);
        }

        if (conductor != null)
        {
            conductor.Ticked += SwapSprites;
        }
    }

    private void Update()
    {
        if (shouldScale)
        {
            ZoomToBeat();
        }
    }

    private void SwapSprites()
    {
        for (int i = 0; i < objectsToScale.Length; i++)
        {
            if (spriteSwapNbr == 0)
            {
                objectsToScale[i].GetComponent<SpriteRenderer>().sprite = objectsToScale[i].GetComponent<Scr_SpriteSwap>().spriteA;
                spriteSwapNbr++;
            }
            else
            {
                objectsToScale[i].GetComponent<SpriteRenderer>().sprite = objectsToScale[i].GetComponent<Scr_SpriteSwap>().spriteB;
                spriteSwapNbr--;
            }
        }
    }

    public void StartZoomToBeat(float _zoomScale)
    {
        shouldScale = true;
        timeStartedZoom = (float)AudioSettings.dspTime;
        zoomScale = _zoomScale;
    }

    private void ZoomToBeat()
    {
        float normalizedCurveTime = ((float)AudioSettings.dspTime - timeStartedZoom) / (conductor.secPerBeat * zoomTimeScale);

        for (int i = 0; i < objectsToScale.Length; i++)
        {
            if (normalizedCurveTime <= 1)
            {
                objectsToScale[i].transform.localScale = baseScales[i] * ((zoomCurve.Evaluate(normalizedCurveTime) * zoomScale + 1));
            }
            else
            {
                objectsToScale[i].transform.localScale = baseScales[i];
                
                if (i == objectsToScale.Length - 1)
                {
                    shouldScale = false;
                }
            }
        }
    }
}

