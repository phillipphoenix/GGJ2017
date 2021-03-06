﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance { get; set; }

    public int SecondsSurvived { get { return (int) _secondsSurvived; } private set { _secondsSurvived = value; } }

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

    [SerializeField]
    private GameObject _introPanelGo;
    [SerializeField]
    private GameObject _countdownGo;
    [SerializeField]
    private GameObject _outroPanelGo;
    [SerializeField]
    private Text _countdownNumberText;

    private bool _spawnerStarted;
    private bool _rotationStarted;
    private float _currentDelay = -1f;
    private float _delayTimer;
    private float _delayReductionTimer;
    private bool _gameOver;

    [SerializeField]
    private SteamVR_TrackedController _controller1, _controller2;
    private float _triggerPressedTimer;

    [SerializeField]
    private float _minWaveSpawnAngle = 20;
    [SerializeField]
    private float _maxWaveSpawnAngle = 90;
    private float _previousWaveSpawnAngle;

    private float _secondsSurvived;

    public UnityEvent OnLevelLoaded;
    public UnityEvent OnGameStart;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        _introPanelGo.SetActive(true);
        _outroPanelGo.SetActive(false);

    }

	// Use this for initialization
	void Start ()
	{
        OnLevelLoaded.Invoke();
    }
	
	// Update is called once per frame
	void Update () {
        // If game over, don't do anything.
	    if (_gameOver)
	    {
	        return;
	    }

        // If waves not started, wait for players to start spawner.
	    if (!_spawnerStarted)
	    {
	        HandleCountdown();
            return;
	    }

	    if (!_rotationStarted)
	    {
            OnGameStart.Invoke();

            foreach (var oceanPlaneGo in GameObject.FindGameObjectsWithTag("OceanPlane"))
            {
                oceanPlaneGo.GetComponent<Rotator>().StartRotation();
            }
	        _rotationStarted = true;
	    }

        // Count survived seconds.
        _secondsSurvived += Time.deltaTime;

        // Spawn waves.
        HandleWaveSpawning();
	}

    public void StopSpawner()
    {
        _gameOver = true;
    }

    public void ResetScore()
    {
        SecondsSurvived = 0;
    }

    private void HandleCountdown()
    {
        // If both triggers are pressed a timer will count.
        // If the timer reaches the countdown the first wave will spawn.
        if (_controller1 != null && _controller1.triggerPressed && _controller2 != null && _controller2.triggerPressed)
        {
            HandleCountdownUI(true);
            _triggerPressedTimer += Time.deltaTime;
            if (_triggerPressedTimer >= _startCountdown)
            {
                _spawnerStarted = true;
                if (_introPanelGo != null)
                {
                    _introPanelGo.SetActive(false);
                }
            }
        }
        else
        {
            HandleCountdownUI(false);
            _triggerPressedTimer = 0;
            if (_introPanelGo != null)
            {
                _introPanelGo.SetActive(true);
            }
        }
    }

    private void HandleCountdownUI(bool pressed)
    {
        if (_introPanelGo == null)
        {
            return;
        }

        // If buttons pressed, set countdown object to active.
        _countdownGo.SetActive(pressed);

        _countdownNumberText.text = ((int)(_startCountdown - _triggerPressedTimer)).ToString();
    }

    public void DisplayOutroUI()
    {
        _outroPanelGo.SetActive(true);
    }

    private void HandleWaveSpawning()
    {
        _delayTimer += Time.deltaTime;
        _delayReductionTimer += Time.deltaTime;
        // When the timer passed the delay, spawn a wave (and more!).
        if (_delayTimer >= _currentDelay)
        {
            // If initial wave, set delay to initial delay.
            if (_currentDelay < 0)
            {
                _currentDelay = _initialDelay;
            }

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
        // Random angle between a min and max angle from the last used angle.
        float angle = _previousWaveSpawnAngle + Random.Range(_minWaveSpawnAngle, _maxWaveSpawnAngle) * (Random.value > .5f ? -1 : 1);
        Vector3 pos = GetPosOnCircle(center, _spawnRadius, angle);
        if (Mathf.Abs(angle - _previousWaveSpawnAngle) > _maxWaveSpawnAngle)
        {
            Debug.Log("New angle for wave is too large. Prev: " + _previousWaveSpawnAngle + ", New: " + angle + ", Diff: " + (Mathf.Abs(angle - _previousWaveSpawnAngle)));
        }
        _previousWaveSpawnAngle = angle;

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
            Gizmos.DrawSphere(GetPosOnCircle(transform.position, _spawnRadius, i), .2f);
        }

        // Wave target circle.
        Gizmos.color = Color.red;
        for (int i = 0; i < 360; i += 10)
        {
            Gizmos.DrawSphere(GetPosOnCircle(transform.position, _targetRadius, i), .2f);
        }
    }
}
