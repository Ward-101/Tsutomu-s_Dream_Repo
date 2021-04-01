using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Scr_MusicMathUtils
{
    public const int MidiNoteC4 = 60;

    public static float MidiNoteToPitch(int midiNote, int baseNote)
    {
        int semitoneOffset = midiNote - baseNote;
        return Mathf.Pow(2.0f, semitoneOffset / 12.0f);
    }
}
