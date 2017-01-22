using System.Collections;
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
    private GameObject _confettiSpawnerPrefab;
    [SerializeField]
    private bool _reloadLevelUponDeath = true;

    public UnityEvent OnHitEvent;
    public UnityEvent OnHitPointsZeroEvent;
    public UnityEvent OnDeathSfxDoneEvent;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_waveLayer))
        {
            OnHitEvent.Invoke();
            Pulse(other.gameObject.transform.position);
            Destroy(other.gameObject);
            _hitPoints--;
            if (_hitPoints == 0)
            {
                Debug.Log("HeadsetPlayer has lost all hit points.");
                OnHitPointsZeroEvent.Invoke();
                Invoke("InvokeDeathSfxDone", SfxPlayer.Instance.death.length);

                if (_reloadLevelUponDeath)
                {
                    ReloadScene();
                }
                WaveSpawner.Instance.ResetScore();
            }
        }
    }

    void InvokeDeathSfxDone() {
        OnDeathSfxDoneEvent.Invoke();
    }

    public void ReloadScene()
    {
        Debug.Log("Reloading scene!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Pulse(Vector3 center)
    {
        if (!_spawnPulseOnHit)
        {
            return;
        }
        // Instantiate pulse as child
        if (_confettiSpawnerPrefab != null)
        {
            GameObject confetti = Instantiate(_confettiSpawnerPrefab);
            confetti.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
