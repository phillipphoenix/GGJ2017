using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMover : MonoBehaviour
{

    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float Duration { get { return _duration; } set { _duration = value; } }

    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _duration = 5f;

    public void Awake()
    {
        Invoke("DestroySelf", _duration);
    }

	// Update is called once per frame
	void Update ()
	{
	    transform.position += transform.forward * _moveSpeed;
	}

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

}
