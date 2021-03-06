using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_TempoUI : MonoBehaviour
{
    [Header("Edit")]
    public GameObject notePrefab;
    [SerializeField, Range(0, 8)] private int noteShownInAdvanceNbr = 4;

    [SerializeField] private Color neonBlue;
    [SerializeField] private Color neonYellow;

    [SerializeField] private AnimationCurve zoomCurve;
    [SerializeField, Range(0.001f, 1f)] private float zoomTimeScale = 1f;
    [SerializeField] private float zoomScale;

    [SerializeField] private AnimationCurve growthCurve;
    [SerializeField] private float growthRate = 0.8f;

    [Header("States : DON'T TOUCH")]
    [SerializeField] private bool shouldlerp = false;
    [SerializeField] private bool shouldScaleHeart = false;
    [SerializeField] private bool shouldSucessInputScale = false;

    [Header("DON'T TOUCH")]
    [SerializeField] private List<GameObject> rightNotes;
    [SerializeField] private List<GameObject> leftNotes;
    [SerializeField] private List<float> timeStartedLerping;


    private RectTransform spawnRightTransform = null;
    private RectTransform spawnLeftTransform = null;
    private RectTransform endTransform = null;
    private GameObject successInputGo;
    private Image trackImage = null;

    private float lerpTime;
    private int spriteSwapNbr = 0;
    private float timeStartedHeartBeat;
    private float timeStartedSucessInput;
    private Vector2 heartBaseScale;
    private Vector2 sucessInputBaseScale;

    private Scr_Conductor conductor;

    public static Scr_TempoUI instance = null;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;

        spawnRightTransform = transform.GetChild(4).GetComponent<RectTransform>();
        spawnLeftTransform = transform.GetChild(5).GetComponent<RectTransform>();
        endTransform = transform.GetChild(6).GetComponent<RectTransform>();
        successInputGo = transform.GetChild(0).gameObject;

        trackImage = transform.GetChild(1).GetComponent<Image>();

        heartBaseScale = endTransform.sizeDelta;
        sucessInputBaseScale = successInputGo.GetComponent<RectTransform>().sizeDelta;
        successInputGo.SetActive(false);

        SetNotes();

        if (conductor != null)
        {
            conductor.Ticked += SpawnNotes;
            conductor.Ticked += StartHeartBeat;
        }
    }


    private void Update()
    {
        if (conductor.isEndBreak && !shouldlerp)
        {
            StartLerp();
        }
        else if (conductor.isBreak && !conductor.isEndBreak)
        {
            EndLerp();
        }

        if (shouldlerp)
        {
            LerpNotes();
        }
        else
        {
            for (int i = 0; i < noteShownInAdvanceNbr +1; i++)
            {
                leftNotes[i].SetActive(false);
                rightNotes[i].SetActive(false);
            }
        }

        if (shouldScaleHeart)
        {
            HeartBeat();
        }

        if (shouldSucessInputScale)
        {
            SucessInput();
        }
    }

    private void StartHeartBeat()
    {
        if (spriteSwapNbr == 0)
        {
            endTransform.gameObject.GetComponent<Image>().color = new Color(neonBlue.r, neonBlue.g, neonBlue.b, 250);
            trackImage.color = neonBlue;
            spriteSwapNbr++;
        }
        else
        {
            endTransform.gameObject.GetComponent<Image>().color = new Color(neonYellow.r, neonYellow.g, neonYellow.b, 250);
            trackImage.color = neonYellow;
            spriteSwapNbr--;
        }

        timeStartedHeartBeat = (float)AudioSettings.dspTime;
        shouldScaleHeart = true;
    }

    private void HeartBeat()
    {
        float normalizedCurveTime = ((float)AudioSettings.dspTime - timeStartedHeartBeat) / (conductor.secPerBeat * zoomTimeScale);

        if (normalizedCurveTime <= 1)
        {
            endTransform.sizeDelta = heartBaseScale * ((zoomCurve.Evaluate(normalizedCurveTime) * zoomScale + 1));
        }
        else
        {
            endTransform.sizeDelta = heartBaseScale;
            shouldScaleHeart = false;
        }
    }

    public void StartSuccessInput()
    {
        successInputGo.SetActive(true);

        successInputGo.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 250f);
        successInputGo.GetComponent<Image>().color = endTransform.GetComponent<Image>().color;

        timeStartedSucessInput = (float)AudioSettings.dspTime;
        shouldSucessInputScale = true;
    }

    public void SucessInput()
    {
        float normalizedCurveTime = ((float)AudioSettings.dspTime - timeStartedSucessInput) / (conductor.secPerBeat);

        if (normalizedCurveTime <= 1)
        {
            successInputGo.GetComponent<RectTransform>().sizeDelta = sucessInputBaseScale * ((growthCurve.Evaluate(normalizedCurveTime) * growthRate + 1));
            successInputGo.GetComponent<Image>().color = new Color(successInputGo.GetComponent<Image>().color.r, successInputGo.GetComponent<Image>().color.g, successInputGo.GetComponent<Image>().color.b, (1 - (1 * (growthCurve.Evaluate(normalizedCurveTime)))));
        }
        else
        {
            successInputGo.GetComponent<RectTransform>().sizeDelta = sucessInputBaseScale;
            successInputGo.SetActive(false);
            shouldSucessInputScale = false;
        }
    }

    #region Lerp Func
    private void StartLerp()
    {
        lerpTime = conductor.secPerBeat * noteShownInAdvanceNbr;
        shouldlerp = true;
    }

    private void EndLerp()
    {
        shouldlerp = false;
    }

    private void SetNotes()
    {
        rightNotes = new List<GameObject>();
        leftNotes = new List<GameObject>();
        timeStartedLerping = new List<float>();

        for (int i = 0; i < noteShownInAdvanceNbr + 1; i++)
        {
            GameObject noteRight = (GameObject)Instantiate(notePrefab, spawnRightTransform.position, Quaternion.identity, transform.GetChild(2));
            GameObject noteLeft = (GameObject)Instantiate(notePrefab, spawnLeftTransform.position, Quaternion.identity, transform.GetChild(3));

            noteLeft.transform.eulerAngles = new Vector2(0, 180);

            noteRight.SetActive(false);
            noteLeft.SetActive(false);

            rightNotes.Add(noteRight);
            leftNotes.Add(noteLeft);

            timeStartedLerping.Add(0f);
            timeStartedLerping.Add(0f);
        }

    }

    private void SpawnNotes()
    {
        if (shouldlerp)
        {
            for (int i = 0; i < noteShownInAdvanceNbr + 1; i++)
            {
                if (!rightNotes[i].activeInHierarchy)
                {
                    rightNotes[i].SetActive(true);
                    rightNotes[i].GetComponent<RectTransform>().position = spawnRightTransform.position;
                    timeStartedLerping[i] = (float)AudioSettings.dspTime;
                    break;
                }
                else
                {
                    if (rightNotes[i].GetComponent<RectTransform>().position.x == endTransform.position.x)
                    {
                        rightNotes[i].GetComponent<RectTransform>().position = spawnRightTransform.position;
                        timeStartedLerping[i] = (float)AudioSettings.dspTime;
                        break;
                    }
                }
            }

            for (int i = 0; i < noteShownInAdvanceNbr + 1; i++)
            {
                if (!leftNotes[i].activeInHierarchy)
                {
                    leftNotes[i].SetActive(true);
                    leftNotes[i].GetComponent<RectTransform>().position = spawnLeftTransform.position;
                    timeStartedLerping[i + noteShownInAdvanceNbr + 1] = (float)AudioSettings.dspTime;
                    break;
                }
                else
                {
                    if (leftNotes[i].GetComponent<RectTransform>().position.x == endTransform.position.x)
                    {
                        leftNotes[i].GetComponent<RectTransform>().position = spawnLeftTransform.position;
                        timeStartedLerping[i + noteShownInAdvanceNbr + 1] = (float)AudioSettings.dspTime;
                        break;
                    }
                }
            }
        }
    }


    private void LerpNotes()
    {
        Vector2 spawnRightPos = new Vector2(spawnRightTransform.position.x, spawnRightTransform.position.y);
        Vector2 spawnLeftPos = new Vector2(spawnLeftTransform.position.x, spawnLeftTransform.position.y);
        Vector2 endPos = new Vector2(endTransform.position.x, endTransform.position.y);


        for (int i = 0; i < noteShownInAdvanceNbr + 1; i++)
        {
            if (rightNotes[i].activeInHierarchy)
            {
                rightNotes[i].transform.position = Lerp(spawnRightPos, endPos, timeStartedLerping[i], lerpTime);
                
            }

            if (leftNotes[i].activeInHierarchy)
            {
                leftNotes[i].transform.position = Lerp(spawnLeftPos, endPos, timeStartedLerping[i + noteShownInAdvanceNbr + 1], lerpTime);
            }
        }
    }

    public Vector2 Lerp(Vector2 start, Vector2 end, float timeStartedLerping, float lerpTime)
    {
        float timeSinceStarted = (float)AudioSettings.dspTime - timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector2.Lerp(start, end, percentageComplete);

        return result;
    }
    #endregion

    

}
