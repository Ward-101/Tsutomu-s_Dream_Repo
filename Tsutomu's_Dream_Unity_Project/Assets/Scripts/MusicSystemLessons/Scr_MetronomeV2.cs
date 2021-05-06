using UnityEngine;
using System;

public class Scr_MetronomeV2 : MonoBehaviour
{
    public event Action<double> Ticked;


    [SerializeField, Tooltip("Le tempo en batement par minute")] private double tempo = 120.0;
    [SerializeField, Tooltip("le nombre de tick par batement"), Range(1, 8)] private int subdivisions = 4;

    private double tickLengh;

    private double nextTickTime;

    private int tickCount = 1;

    private void Reset()
    {
        Recalculate();
        nextTickTime = AudioSettings.dspTime + tickLengh;
    }

    private void Recalculate()
    {
        double beatsPerSecond = tempo / 60.0;
        double tickPerSecond = beatsPerSecond * subdivisions;
        tickLengh = 1.0 / tickPerSecond;
    }

    private void Awake()
    {
        Reset();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Recalculate();
        }
    }

    private void FixedUpdate()
    {
        double currentTime = AudioSettings.dspTime;

        currentTime += Time.deltaTime;

        while (currentTime > nextTickTime)
        {
            if (Ticked != null)
            {
                Ticked(nextTickTime);
            }

            nextTickTime += tickLengh;
            tickCount++;
        }
    }
}
