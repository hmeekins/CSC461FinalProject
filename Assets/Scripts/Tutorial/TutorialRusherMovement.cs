using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TutorialRusherMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rushDistance = 6f;
    [SerializeField] private float _resetDelay = .5f;
    [SerializeField] private TutorialParticles _particles;
    private Vector3 _startPosition;
    private bool _rushStarted = false;

    void Start()
    {
        _startPosition = transform.position;
        _particles = GameObject.Find("Particles").GetComponent<TutorialParticles>();
    }

    void Update() 
    {
        if (TutorialController.Instance.TutorialStep == TutorialStep.Rusher && !_rushStarted)
        {
            _rushStarted = true;
            StartCoroutine(RushLoop());
        }
    }

    IEnumerator RushLoop()
    {
        while (true)
        {
            while (Vector3.Distance(_startPosition, transform.position) < _rushDistance)
            {
                Vector3 target = _player.position;
                target.y = transform.position.y;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    _speed * Time.deltaTime
                );

                Vector3 dir = (target - transform.position).normalized;
                if (dir != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(dir);
                }

                yield return null;
            }

            yield return new WaitForSeconds(_resetDelay);
            _particles.Poof(transform.position);
            transform.position = _startPosition;
        }
    }
}
