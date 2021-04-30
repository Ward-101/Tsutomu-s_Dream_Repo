using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TempoUI : MonoBehaviour
{
    [Header("Edit")]
    public GameObject notePrefab;
    [SerializeField, Range(1, 8)] private int noteShownInAdvanceNbr = 3;

    [Header("DON'T TOUCH")]
    [SerializeField] private List<GameObject> rightNotes;
    [SerializeField] private List<GameObject> leftNotes;

    public static Scr_TempoUI instance = null;

    private Transform spawnRightTransform = null;
    private Transform spawnLeftTransform = null;
    private Transform endTransform = null;

    //private vector2 startpos;
    //private vector2 endpos;

    //private float timestartedlerping;
    //public float lerptime;

    //private bool shouldlerp = false;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    private void Start()
    {
        SetNotes();
    }

    private void Update()
    {






















        //if (input.getkeydown(keycode.space))
        //{
        //    startlerp();
        //}


        //if (shouldlerp)
        //{
        //    start.transform.position = lerp(startpos, endpos, timestartedlerping, lerptime);
        //}
        
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
    }

    //private void StartLerp()
    //{
    //    startPos = new Vector2(start.transform.position.x, start.transform.position.y);
    //    endPos = new Vector2(end.transform.position.x, end.transform.position.y);
    //    timeStartedLerping = (float)AudioSettings.dspTime;
    //    shouldLerp = true;
    //}

    //public Vector2 Lerp(Vector2 start, Vector2 end, float timeStartedLerping, float lerpTime)
    //{
    //    float timeSinceStarted = (float)AudioSettings.dspTime - timeStartedLerping;

    //    float percentageComplete = timeSinceStarted / lerpTime;

    //    var result = Vector2.Lerp(start, end, percentageComplete);

    //    return result;
    //}

}
