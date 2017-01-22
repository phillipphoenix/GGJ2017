using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaveInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject _waveInstantiatorPrefab;

    [ContextMenu("Spawn wave without movement")]
    private void SpawnWaveWithoutMovement()
    {
        Instantiate(_waveInstantiatorPrefab, transform.position, transform.rotation).GetComponent<WaveMover>().enabled = false;
    }

}
