﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class HeadsetPlayer : MonoBehaviour
{
    [SerializeField]
    private int _hitPoints;
    [SerializeField]
    private string _waveLayer = "Waves";
    [SerializeField]
    private bool _spawnPulseOnHit = true;
    [SerializeField]
    private GameObject _pulsePrefab;
    [SerializeField]
    private bool _reloadLevelUponDeath = true;

    public UnityEvent OnHitPointsZeroEvent;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_waveLayer))
        {
            Pulse(other.gameObject.transform.position);
            Destroy(other.gameObject);
            _hitPoints--;
            if (_hitPoints == 0)
            {
                Debug.Log("HeadsetPlayer has lost all hit points.");
                OnHitPointsZeroEvent.Invoke();
                if (_reloadLevelUponDeath)
                {
                    Debug.Log("Reloading scene!");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }

    private void Pulse(Vector3 center)
    {
        if (!_spawnPulseOnHit)
        {
            return;
        }
        // Instantiate pulse as child
        if (_pulsePrefab != null)
        {
            GameObject go = Instantiate(_pulsePrefab);
            go.transform.parent = transform;
            go.transform.position = center;
        }
    }
}
