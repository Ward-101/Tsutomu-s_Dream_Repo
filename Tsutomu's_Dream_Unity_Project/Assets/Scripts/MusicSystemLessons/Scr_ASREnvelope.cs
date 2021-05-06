using System;

public class Scr_ASREnvelope
{
    private enum State
    {
        Idle,
        Attack,
        Sustain,
        Release
    }

    private State state;
    private double attackIncrement;
    private uint sustainSamples;
    private double releaseIncrement;
    private double outputLevel;

    public void Reset(double attackTime_s, double sustainTime_s, double releaseTime_s, int sampleRate)
    {
        state = State.Attack;

        // ??
        attackIncrement = (attackTime_s > 0) ? (1.0 / (attackTime_s * sampleRate)) : 1.0;
        sustainSamples = (uint)(sustainTime_s * sampleRate);
        releaseIncrement = (releaseTime_s > 0.0) ? (1.0 / (releaseTime_s * sampleRate)) : 1.0;
        // ??

        outputLevel = 0.0;
    }

    public double GetLevel()
    {
        switch (state)
        {
            case State.Idle:
                outputLevel = 0.0;
                break;
            case State.Attack:
                outputLevel += attackIncrement;

                if (outputLevel > 1.0)
                {
                    outputLevel = 1.0;
                    state = State.Sustain;
                }

                break;
            case State.Sustain:
                if ((sustainSamples == 0) || (--sustainSamples == 0))
                {
                    state = State.Release;
                }

                break;
            case State.Release:
                outputLevel -= releaseIncrement;

                if (outputLevel < 0.0)
                {
                    outputLevel = 0.0;
                    state = State.Idle;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return outputLevel;
    }
}
