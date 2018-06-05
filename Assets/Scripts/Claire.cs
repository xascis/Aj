using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claire : MonoBehaviour {

	private Animator _animator;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform _transform;

	private GameObject _player;

	public const float distanceToFollow = 7.0f;
	public const float distanceToStop = 2.0f;


	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _transform = GetComponent<Transform>();
		
		_player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		_animator.SetFloat("speed", _navMeshAgent.desiredVelocity.magnitude);

		if (Vector3.Distance(_player.transform.position, _transform.position) < distanceToFollow)
		{
			_navMeshAgent.SetDestination(_player.transform.position);
		}
		if (Vector3.Distance(_player.transform.position, _transform.position) > distanceToFollow)
		{
			_navMeshAgent.SetDestination(_transform.transform.position);
		}
		if (Vector3.Distance(_player.transform.position, _transform.position) < distanceToStop) {
			_navMeshAgent.SetDestination(_transform.transform.position);
		}


	}
}
