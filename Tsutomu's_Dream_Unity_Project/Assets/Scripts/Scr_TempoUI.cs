using System.Collections.Generic;
using UnityEngine;

public class Scr_TempoUI : MonoBehaviour
{
    [Header("Edit")]
    public GameObject notePrefab;
    [SerializeField, Range(0, 8)] private int noteShownInAdvanceNbr = 4;

    [Header("DON'T TOUCH")]
    [SerializeField] private List<GameObject> rightNotes;
    [SerializeField] private List<GameObject> leftNotes;
    [SerializeField] private List<float> timeStartedLerping;

    [Header("States : DON'T TOUCH")]
    [SerializeField] private bool shouldlerp = false;
    //[SerializeField] private bool shouldSpawnNotes = true;

    private Transform spawnRightTransform = null;
    private Transform spawnLeftTransform = null;
    private Transform endTransform = null;

    private float lerpTime;
    
    private Scr_Conductor conductor;

    public static Scr_TempoUI instance = null;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;

        spawnRightTransform = transform.GetChild(2);
        spawnLeftTransform = transform.GetChild(3);
        endTransform = transform.GetChild(4);

        SetNotes();

        if (conductor != null)
        {
            conductor.Ticked += SpawnNotes;
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
    }

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
            GameObject noteRight = (GameObject)Instantiate(notePrefab, spawnRightTransform.position, Quaternion.identity, transform.GetChild(0));
            GameObject noteLeft = (GameObject)Instantiate(notePrefab, spawnLeftTransform.position, Quaternion.identity, transform.GetChild(1));

            noteLeft.GetComponent<SpriteRenderer>().flipX = true;

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
                    timeStartedLerping[i] = (float)AudioSettings.dspTime;
                    break;
                }
                else
                {
                    if (rightNotes[i].transform.position == endTransform.position)
                    {
                        rightNotes[i].transform.position = spawnRightTransform.position;
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
                    timeStartedLerping[i + noteShownInAdvanceNbr + 1] = (float)AudioSettings.dspTime;
                    break;
                }
                else
                {
                    if (leftNotes[i].transform.position == endTransform.position)
                    {
                        leftNotes[i].transform.position = spawnLeftTransform.position;
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

}
