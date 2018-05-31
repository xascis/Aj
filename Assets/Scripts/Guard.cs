using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Guard : MonoBehaviour {
	private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
	// private RigidBody _rigidBody;

	public Transform[] destination;

	private int _current;

	private bool _isWalking;

    private Vector3 currentDestination;


	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();
		// _rigidBody = GetComponent<RigidBody>();
		
	}
	
	// Update is called once per frame
	void Update () {
		_animator.SetFloat("speed", _navMeshAgent.desiredVelocity.magnitude);
		
		if(_transform.position.x != destination[_current].position.x){
			// Vector3 pos = Vector3.MoveTowards(_transform.position, destination[current].position, _navMeshAgent.desiredVelocity.magnitude );
			// _rigidBody.MovePosition(pos);
			_navMeshAgent.SetDestination(destination[_current].position);
		} else {
			_current = (_current + 1) % destination.Length;
		}

		print(_current);

		// _navMeshAgent.SetDestination(_destination1.transform.position);
		// _isWalking = true;
	}
}
