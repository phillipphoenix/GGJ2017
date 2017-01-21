using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Rumbler
{

    private readonly VRTK_ControllerActions _controllerActions;

    public Rumbler(VRTK_ControllerActions controllerActions)
    {
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
}
