using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BeatCircles : MonoBehaviour
{
    [SerializeField] private Scr_MetronomeV2 metronome;

    private Transform[] arrayOfTransformRight;
    private Transform[] arrayOfTransformLeft;

    private Transform halfCircleRightTranform;
    private Transform halfCircleLeftTranform;
    private int nextPosition = 3;

    private void Awake()
    {
        arrayOfTransformRight = new Transform[transform.GetChild(0).childCount];
        arrayOfTransformLeft = new Transform[transform.GetChild(1).childCount];

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            arrayOfTransformRight[i] = transform.GetChild(0).GetChild(i);
            arrayOfTransformLeft[i] = transform.GetChild(1).GetChild(i);
        }

        halfCircleRightTranform = transform.GetChild(2);
        halfCircleLeftTranform = transform.GetChild(3);
    }

    private void Start()
    {
        metronome.Ticked += OnTick;
    }

    private void OnTick(double tickTime)
    {

        halfCircleRightTranform.position = arrayOfTransformRight[nextPosition].position;
        halfCircleLeftTranform.position = arrayOfTransformLeft[nextPosition].position;

        if (nextPosition == 0)
        {
            nextPosition = 3;
        }
        else nextPosition--;
    }
}
