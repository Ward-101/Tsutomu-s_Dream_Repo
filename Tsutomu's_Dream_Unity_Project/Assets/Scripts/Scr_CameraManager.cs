using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Scr_CameraManager : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField, Range(0.001f, 1f)] private float zoomTimeScale = 1f;
    [SerializeField] private AnimationCurve zoomCurve;

    [Header("States")]
    public bool shouldZoom;

    [Header("DON'T TOUCH")]
    [SerializeField] private float normalCameraSize;

    private float zoomScale;
    private float timeStartedZoom;
    private Scr_Conductor conductor;

    public static Scr_CameraManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        conductor = Scr_Conductor.instance;
        normalCameraSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if (shouldZoom)
        {
            ZoomToBeat();
        }
    }

    public void StartZoomToBeat(float _zoomScale)
    {
        shouldZoom = true;
        timeStartedZoom = (float)AudioSettings.dspTime;
        zoomScale = _zoomScale;
    }

    private void ZoomToBeat()
    {
        float normalizedCurveTime = ((float)AudioSettings.dspTime - timeStartedZoom) / (conductor.secPerBeat * zoomTimeScale);

        if (normalizedCurveTime <= 1)
        {
            Camera.main.orthographicSize = normalCameraSize / ((zoomCurve.Evaluate(normalizedCurveTime) * zoomScale + 1));
        }
        else
        {
            Camera.main.orthographicSize = normalCameraSize;
            shouldZoom = false;
        }
        
    }
}
