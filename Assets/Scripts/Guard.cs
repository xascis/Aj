﻿using System.Collections;
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
	public const float maxDistanceToFollow = 20.0f;

	private bool _isPatroling;
	private bool _isFollowing;
	private bool _isWaiting;

	private float _timer;
	private float _timerMax;

	public AudioSource sound;

	private bool _isPlayingSound;


	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();

		_player = GameObject.FindWithTag("Player");

		sound = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		// animación dependiendo de la velocidad
		_animator.SetFloat("speed", _navMeshAgent.desiredVelocity.magnitude);

		// persigue al jugador
		if (Vector3.Distance(_player.transform.position, _transform.position) < distanceToFollow
		    && Vector3.Distance(Destination[_current].position, _player.transform.position) < maxDistanceToFollow)
		{
			sound.pitch = 1.5f;
			if (!_isPlayingSound)
			{
				_isPlayingSound = true;
				sound.Play();
			}
			_navMeshAgent.speed = 2f;
			_navMeshAgent.SetDestination(_player.transform.position);
		}
		else
		{
			if (_transform.position.x != Destination[_current].position.x)
			{
				sound.pitch = 1f;
				if (!_isPlayingSound)
				{
					_isPlayingSound = true;
					sound.Play();
				}
				_navMeshAgent.speed = 1f;
				_navMeshAgent.SetDestination(Destination[_current].position);
			}
			else
			{
				sound.Stop();
				_isPlayingSound = false;
				// asigna nuevo destino
				if (Waited(5))
				{
					_current = (_current + 1) % Destination.Length;
					_timer = 0;
				}
			}
		}
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

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			Messenger.Broadcast("attack");
		}
	}
}
