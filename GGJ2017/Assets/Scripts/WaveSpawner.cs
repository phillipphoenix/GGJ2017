using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Countdown till first wave in seconds.")]
    private float _startCountdown = 3f;
    [SerializeField]
    private float _initialDelay;
    [SerializeField]
    private float _delayReductionPercentage;
    [SerializeField]
    [Tooltip("The countdown till next reduction in wave delay (in seconds).")]
    private float _delayReductionCountdown;

    [SerializeField]
    private float _spawnRadius;
    [SerializeField]
    private float _targetRadius;
    [SerializeField]
    private GameObject _wavePrefab;
    [SerializeField]
    private GameObject _waveContainer;

    private bool _spawnerStarted;
    private float _currentDelay;
    private float _delayTimer;
    private float _delayReductionTimer;

    [SerializeField]
    private SteamVR_TrackedController _controller1, _controller2;
    private float _triggerPressedTimer;

	// Use this for initialization
	void Start ()
	{
	    _currentDelay = _initialDelay;
	}
	
	// Update is called once per frame
	void Update () {
        // If waves not started, wait for players to start spawner.
	    if (!_spawnerStarted)
	    {
	        HandleCountdown();
            return;
	    }

        // Spawn waves.
        HandleWaveSpawning();
	}

    private void HandleCountdown()
    {
        // If both triggers are pressed a timer will count.
        // If the timer reaches the countdown the first wave will spawn.
        if (_controller1 != null && _controller1.triggerPressed && _controller2 != null && _controller2.triggerPressed)
        {
            _triggerPressedTimer += Time.deltaTime;
            if (_triggerPressedTimer >= _startCountdown)
            {
                _spawnerStarted = true;
            }
        }
        else
        {
            _triggerPressedTimer = 0;
        }
    }

    private void HandleWaveSpawning()
    {
        _delayTimer += Time.deltaTime;
        _delayReductionTimer += Time.deltaTime;
        // When the timer passed the delay, spawn a wave (and more!).
        if (_delayTimer >= _currentDelay)
        {
            // Reset delay timer.
            _delayTimer = 0;

            // Spawn a wave.
            SpawnWave();
        }

        // Reduce the delay between waves, when the delay reduction timer passes the delay reduction countdown.
        if (_delayReductionTimer >= _delayReductionCountdown)
        {
            // Reset reduction timer.
            _delayReductionTimer = 0;

            // Subtract a percentage of the wave spawn delay from the current value.
            _currentDelay -= _currentDelay * _delayReductionPercentage;

            Debug.Log("Delay has been reduced to: " + _currentDelay);
        }
    }

    [ContextMenu("Spawn wave")]
    private void SpawnWave()
    {
        // Get random position on the spawn circle.
        Vector3 center = transform.position;
        float angle = Random.value * 360;
        Vector3 pos = GetPosOnCircle(center, _spawnRadius, angle);

        // Get look at angle.
        Vector3 posTarget = Random.insideUnitSphere * _targetRadius;
        posTarget.y = 0;
        Quaternion rotation = Quaternion.LookRotation(posTarget - pos, Vector3.up);

        // Spawn wave.
        GameObject go = Instantiate(_wavePrefab, pos, rotation);
        if (_waveContainer != null)
        {
            go.transform.parent = _waveContainer.transform;
        }
    }

    private Vector3 GetPosOnCircle(Vector3 center, float radius, float angle)
    {
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = 0;
        pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return pos;
    }

    [ContextMenu("Test spawning")]
    private void TestSpawning()
    {
        _spawnerStarted = true;
    }

    private void OnDrawGizmos()
    {
        // Wave spawn circle.
        Gizmos.color = Color.blue;
        for (int i = 0; i < 360; i += 10)
        {
            Gizmos.DrawSphere(GetPosOnCircle(transform.position, _spawnRadius, i), .3f);
        }

        // Wave target circle.
        Gizmos.color = Color.red;
        for (int i = 0; i < 360; i += 10)
        {
            Gizmos.DrawSphere(GetPosOnCircle(transform.position, _targetRadius, i), .3f);
        }
    }
}
