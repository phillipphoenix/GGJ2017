using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanCreator : MonoBehaviour {

	public GameObject _wavePrefab;
	GameObject _oceanCreator;
	public float _spawnRadius, _targetRadius;
	public int numberOfPlanes;
	public int planesPerRow;
	int spawnCount = 0;

	void Start () {
		_oceanCreator = this.gameObject;

		for(int i = 0; i < numberOfPlanes; i++){
			spawnCount++;
			if (spawnCount >= planesPerRow){
				spawnCount = 0;
				_spawnRadius += 2;
			}
							
			GenerateOcean ();
		}

			
	}
	

	void Update () {
		
	}

	private void GenerateOcean()
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
		GameObject go = Instantiate(_wavePrefab);
		if (_oceanCreator != null)
		{
			go.transform.parent = _oceanCreator.transform;
		}
        go.transform.localPosition = pos;
        go.transform.localRotation = rotation;
    }

	private Vector3 GetPosOnCircle(Vector3 center, float radius, float angle)
	{
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
		pos.y = 0;
		pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
		return pos;
	}
}
