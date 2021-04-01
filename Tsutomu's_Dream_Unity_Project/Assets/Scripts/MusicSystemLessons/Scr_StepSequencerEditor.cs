using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Scr_StepSequencer))]
public class Scr_StepSequencerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Scr_StepSequencer sequencer = (Scr_StepSequencer)target;

        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();

        List<Scr_StepSequencer.Step> steps = sequencer.GetSteps();

        int nbrSteps = EditorGUILayout.IntSlider("# steps", steps.Count, 1, 32);

        while (nbrSteps > steps.Count)
        {
            steps.Add(new Scr_StepSequencer.Step());
        }
        while (nbrSteps < steps.Count)
        {
            steps.RemoveAt(steps.Count - 1);
        }

        for (int i = 0; i < steps.Count; i++)
        {
            Scr_StepSequencer.Step step = steps[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 60;
            EditorGUILayout.LabelField("Step " + (i + 1), GUILayout.Width(60));
            step.active = EditorGUILayout.Toggle("Active", step.active, GUILayout.Width(80));
            step.midiNoteNumber = EditorGUILayout.IntField("Note", step.midiNoteNumber);
            step.duration = EditorGUILayout.FloatField("Duration", (float)step.duration);
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
}
