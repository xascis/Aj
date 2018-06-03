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

	public Transform[] Destination;

	private int _current;

	private GameObject _player;
	public const float distanceToFollow = 10.0f;
	public const float maxDistanceToFollow = 50.0f;

	private bool _isPatroling;
	private bool _isFollowing;
	private bool _isWaiting;

	private float _timer;
	private float _timerMax;


	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();
		// _rigidBody = GetComponent<RigidBody>();

		_player = GameObject.FindWithTag("Player");
		
	}
	
	// Update is called once per frame
	void Update () {
		_animator.SetFloat("speed", _navMeshAgent.desiredVelocity.magnitude);

		if (Vector3.Distance(_player.transform.position, _transform.position) < distanceToFollow
		    && Vector3.Distance(Destination[_current].position, _transform.position) < maxDistanceToFollow)
		{
			_navMeshAgent.SetDestination(_player.transform.position);
		}
		else
		{
			if(_transform.position.x != Destination[_current].position.x){
				_navMeshAgent.SetDestination(Destination[_current].position);
			} else
			{
				// asigna nuevo destino
				if (Waited(5))
				{
					_current = (_current + 1) % Destination.Length;
					_timer = 0;
				}
			}
		}

		// _navMeshAgent.SetDestination(_destination1.transform.position);
		// _isWalking = true;
	}

	// función espera x segundos
	private bool Waited(float seconds)
	{
		_timerMax = seconds;

		_timer += Time.deltaTime;

		if (_timer >= _timerMax)
		{
			return true; //max reached - waited x - seconds
		}

		return false;
	}
}
