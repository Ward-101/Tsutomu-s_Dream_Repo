using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TempoUI : MonoBehaviour
{
    [Header("Edit")]
    public GameObject notePrefab;
    [SerializeField, Range(1, 8)] private int noteShownInAdvanceNbr = 4;

    [Header("DON'T TOUCH")]
    [SerializeField] private List<GameObject> rightNotes;
    [SerializeField] private List<GameObject> leftNotes;

    public static Scr_TempoUI instance = null;

    private Transform spawnRightTransform = null;
    private Transform spawnLeftTransform = null;
    private Transform endTransform = null;

    private List<float> timeStartedLerping;
    private float lerpTime;

    private Scr_Conductor conductor;

    private bool shouldlerp = false;

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
        if (conductor.beatCount >= 4 && !shouldlerp)
        {
            StartLerp();
        }

        if (shouldlerp)
        {
            LerpNotes();
        }
    }

    private void StartLerp()
    {
        lerpTime = conductor.secPerBeat * noteShownInAdvanceNbr;
        shouldlerp = true;
    }

        

    private void SetNotes()
    {
        rightNotes = new List<GameObject>();
        leftNotes = new List<GameObject>();

        for (int i = 0; i < noteShownInAdvanceNbr; i++)
        {
            GameObject noteRight = (GameObject)Instantiate(notePrefab, transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
            GameObject noteLeft = (GameObject)Instantiate(notePrefab, transform.GetChild(1).position, Quaternion.identity, transform.GetChild(1));

            noteLeft.GetComponent<SpriteRenderer>().flipX = true;

            noteRight.SetActive(false);
            noteLeft.SetActive(false);

            rightNotes.Add(noteRight);
            leftNotes.Add(noteLeft);
        }

        timeStartedLerping = new List<float>();
    }

    private void SpawnNotes()
    {
        if (shouldlerp)
        {
            for (int i = 0; i < noteShownInAdvanceNbr; i++)
            {
                if (!rightNotes[i].activeInHierarchy)
                {
                    rightNotes[i].SetActive(true);
                    timeStartedLerping.Insert(i,(float)AudioSettings.dspTime);
                    break;
                }
            } 
        }
    }


    private void LerpNotes()
    {
        Vector2 spawnRightPos = new Vector2(spawnRightTransform.position.x, spawnRightTransform.position.y);
        Vector2 spawnLeftPos = new Vector2(spawnLeftTransform.position.x, spawnLeftTransform.position.y);
        Vector2 endPos = new Vector2(endTransform.position.x, endTransform.position.y);


        for (int i = 0; i < noteShownInAdvanceNbr; i++)
        {
            if (rightNotes[i].activeInHierarchy)
            {
                rightNotes[i].transform.position = Lerp(spawnRightPos, endPos, timeStartedLerping[i], lerpTime);
                
            }

            if (leftNotes[i].activeInHierarchy)
            {
                leftNotes[i].transform.position = Lerp(spawnLeftPos, endPos, 0f, lerpTime);
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
