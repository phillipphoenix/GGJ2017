using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RumblerTest : MonoBehaviour
{
    [SerializeField]
    private GameObject _controllerGo;
    [SerializeField]
    private float _rumbleDuration = 1f;
    [SerializeField]
    private float _rumbleInterval = 0.05f;

    private VRTK_ControllerActions _controllerActions;
    private Rumbler _rumbler;

	// Use this for initialization
	void Start ()
	{
	    _controllerActions = _controllerGo.GetComponent<VRTK_ControllerActions>();
		_rumbler = new Rumbler(_controllerActions);
	}

    [ContextMenu("Start rumble tick test")]
    private void StartRumbleTickTest()
    {
        StartCoroutine(RumbleTickTester());
    }

    [ContextMenu("Start rumble duration test")]
    private void StartRumbleDurationTest()
    {
        StartCoroutine(RumbleDurationTester());
    }

    [ContextMenu("Start rumble interval test")]
    private void StartRumbleIntervalTest()
    {
        StartCoroutine(RumbleIntervalTester());
    }

    private IEnumerator RumbleTickTester()
    {
        Debug.Log("Rumble duration test starting.");
        float secondBetweenRumbles = 2;

        _rumbler.Rumble(0.1f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.2f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.3f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.4f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.5f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.6f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.7f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.8f);
        yield return new WaitForSeconds(secondBetweenRumbles);
        _rumbler.Rumble(0.9f);
        yield return new WaitForSeconds(2);
        _rumbler.Rumble(1f);

        Debug.Log("Rumble tick test ended.");
    }

    private IEnumerator RumbleDurationTester()
    {
        Debug.Log("Rumble duration test starting.");
        float secondBetweenRumbles = 2;

        for (float strength = 0.1f; strength <= 1f; strength += 0.1f)
        {
            Debug.Log("Rumble duration strength: " + strength + " .");
            _rumbler.Rumble(strength, _rumbleDuration, _rumbleInterval);
            yield return new WaitForSeconds(secondBetweenRumbles);
        }

        Debug.Log("Rumble duration strength: 1.0 .");
        _rumbler.Rumble(1.0f, _rumbleDuration, _rumbleInterval);
        yield return new WaitForSeconds(secondBetweenRumbles);

        Debug.Log("Rumble duration test ended.");
    }

    private IEnumerator RumbleIntervalTester()
    {
        Debug.Log("Rumble interval test starting.");
        float secondBetweenRumbles = .1f;
        float rumbleDuration = .05f;

        for (float interval = 0.07f; interval >= 0.001f; interval -= 0.0005f)
        {
            Debug.Log("Rumble interval: " + interval + " .");
            _rumbler.Rumble(1f, rumbleDuration, interval);
            yield return new WaitForSeconds(secondBetweenRumbles);
        }

        Debug.Log("Rumble interval test ended.");
    }
}
