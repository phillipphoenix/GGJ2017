using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class HeadsetPlayer : MonoBehaviour
{
    [SerializeField]
    private int _hitPoints;
    [SerializeField]
    private string _waveLayer = "Waves";

    public UnityEvent OnHitPointsZeroEvent;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_waveLayer))
        {
            Destroy(other.gameObject);
            _hitPoints--;
            if (_hitPoints == 0)
            {
                Debug.Log("HeadsetPlayer has lost all hit points.");
                OnHitPointsZeroEvent.Invoke();
            }
        }
    }
}
