using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveboxInstantiator : MonoBehaviour
{

    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private int _waveLength, _waveDepth;
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
                box.transform.localPosition = new Vector3(i, transform.position.y, k);
                box.GetComponent<Wavebox>().delay = (i * 0.5f); // + (k * 0.5f);
            }
        }
    }
}

