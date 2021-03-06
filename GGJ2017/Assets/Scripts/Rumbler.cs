﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Rumbler
{

    private readonly VRTK_ControllerActions _controllerActions;

    private MonoBehaviour _monoBehaviour;
    private Coroutine _rumbleCoroutine;

    public Rumbler(MonoBehaviour monoBehaviour, VRTK_ControllerActions controllerActions)
    {
        _monoBehaviour = monoBehaviour;
        _controllerActions = controllerActions;
    }

    /// <summary>
    /// Rumble the controller with the given intensity for a single tick.
    /// </summary>
    /// <param name="intensity">The intensity of the rumble (between 0 and 1).</param>
    public void Rumble(float intensity)
    {
        _controllerActions.TriggerHapticPulse(intensity);
    }

    /// <summary>
    /// Rumble the controller with the given intensity over the given duration.
    /// </summary>
    /// <param name="intensity">The intensity of the rumble (between 0 and 1).</param>
    /// <param name="duration">The duration of the rumble.</param>
    public void Rumble(float intensity, float duration, float interval = .05f)
    {
        _controllerActions.TriggerHapticPulse(intensity, duration, interval);
    }

    /// <summary>
    /// Starts a rumble which lowers the interval gradually from slow to fast rumble over the given time.
    /// </summary>
    /// <param name="buildUpDuration">The time it should take from initial to end values for the interval.</param>
    public void StartRumbleBuildUp(float buildUpDuration)
    {
        StopRumble();
        _rumbleCoroutine = _monoBehaviour.StartCoroutine(RumbleBuildUp(buildUpDuration));
        _monoBehaviour.Invoke("StopRumble", buildUpDuration);
    }

    /// <summary>
    /// Starts a rumble which lowers the interval gradually from slow to fast rumble over the given time.
    /// </summary>
    /// <param name="buildUpDuration">The time it should take from initial to end values for the interval.</param>
    /// <param name="initInterval">The starting interval (default: 0.07).</param>
    /// <param name="endInterval">The ending interval (default: 0.001).</param>
    public void StartRumbleBuildUp(float buildUpDuration, float initInterval, float endInterval)
    {
        StopRumble();
        _rumbleCoroutine = _monoBehaviour.StartCoroutine(RumbleBuildUp(buildUpDuration, initInterval, endInterval));
        _monoBehaviour.Invoke("StopRumble", buildUpDuration);
    }

    /// <summary>
    /// Stops any ongoing rumble build up sequence.
    /// </summary>
    public void StopRumble()
    {
        Debug.Log("Stopping rumble build up!");
        if (_monoBehaviour != null && _rumbleCoroutine != null)
        {
            _monoBehaviour.StopCoroutine(_rumbleCoroutine);
        }   
    }

    public void StartRumble(float duration, float interval = 0.001f) {
        StopRumble();
        _rumbleCoroutine = _monoBehaviour.StartCoroutine(RumbleCo(duration, interval));
        _monoBehaviour.Invoke("StopRumble", duration);
    }

    private IEnumerator RumbleBuildUp(float buildUpDuration, float initInterval = 0.07f, float endInterval = 0.001f)
    {
        float intervalDifference = initInterval - endInterval;
        int numberOfIncrements = 20;
        float incrementValue = intervalDifference / numberOfIncrements;
        float delay = buildUpDuration / numberOfIncrements;
        float rumbleDuration = delay - 0.05f;

        for (float interval = initInterval; interval >= endInterval; interval -= incrementValue)
        {
            Rumble(1f, rumbleDuration, interval);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator RumbleCo(float duration, float interval = 0.001f) {
        int numberOfIncrements = 20;
        float delay = duration / numberOfIncrements;
        float rumbleDuration = delay - 0.05f;

        for (float i = 0; i < numberOfIncrements; ++i) {
            Rumble(1f, rumbleDuration, interval);
            yield return new WaitForSeconds(delay);
        }
    }
}
