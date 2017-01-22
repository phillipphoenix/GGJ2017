using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveboxInstantiator : MonoBehaviour
{

    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private int _waveLength, _waveDepth;
    [SerializeField]
    private float _boxSpacing = 0.02f;
    [SerializeField]
    private float _rowDelay = 0.15f;
    // Use this for initialization
    void Start()
    {
        Wave();
    }

    void Wave()
    {
        var wave = new GameObject("Wave");
        wave.transform.position = transform.position;
        wave.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90, 0));
        wave.transform.parent = transform;

        int halfLength = _waveLength / 2;
        int halfDepth = _waveDepth / 2;
        for (int i = -halfLength; i < halfLength; i++)
        {
            for (int k = -halfDepth; k < halfDepth; k++)
            {
                var box = Instantiate(_prefab);
                box.transform.parent = wave.transform;
                box.transform.localPosition = new Vector3(i + (_boxSpacing * i), 0, k + (_boxSpacing * k));
                box.transform.localEulerAngles = new Vector3(0, 0, 0);
                box.GetComponent<Wavebox>().delay = (i + halfLength) * _rowDelay; // + (k * 0.5f);
            }
        }
    }
}

