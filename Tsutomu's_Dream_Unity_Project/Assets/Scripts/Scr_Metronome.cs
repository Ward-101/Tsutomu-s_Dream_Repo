using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Metronome : MonoBehaviour
{
    //public static Scr_Metronome instance = null;

    [SerializeField]
    private float bpm = 120;
    private bool ticking = false;

    public delegate void TickAction(int tickCount);
    public static event TickAction OnTick;

    private void Start()
    {
        StartCoroutine(Ticker());
    }

    private IEnumerator Ticker()
    {
        ticking = true;
        int tickCount = 0;

        while (ticking)
        {
            float intervalTime = 60.0f / bpm;

            float startTime = Time.time;

            //Computation Start

            Debug.Log("Tick");
            if (OnTick != null)
            {
                OnTick(tickCount);
            }

            //Computation End

            float duration = Time.time - startTime;
            float waitTime = intervalTime - duration;

            tickCount++;

            yield return new WaitForSeconds(waitTime);
        }
    }

}
