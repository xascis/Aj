using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claire : MonoBehaviour {

	private Animator _animator;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform _transform;

	private GameObject _player;
	private GameObject _finish;

	public const float distanceToFollow = 7.0f;
	public const float distanceToStop = 2.0f;


	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _transform = GetComponent<Transform>();
		
		_player = GameObject.FindWithTag("Player");
		_finish = GameObject.FindWithTag("Finish");
	}
	
	// Update is called once per frame
	void Update () {
		_animator.SetFloat("speed", _navMeshAgent.desiredVelocity.magnitude);

		// sigue al jugador
		if (Vector3.Distance(_player.transform.position, _transform.position) < distanceToFollow)
		{
			_navMeshAgent.SetDestination(_player.transform.position);
		}
		// esta parada
		if (Vector3.Distance(_player.transform.position, _transform.position) > distanceToFollow)
		{
			_navMeshAgent.SetDestination(_transform.transform.position);
		}
		// se para si está muy cerca del jugador para no chocar
		if (Vector3.Distance(_player.transform.position, _transform.position) < distanceToStop) {
			_navMeshAgent.SetDestination(_transform.transform.position);
		}

		// fin del juego
		if(Vector3.Distance(_finish.transform.position, _transform.position) < 5.0f) {
			print("Congratulations!!");
		}
	}
}
