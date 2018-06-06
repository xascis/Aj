using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Vector3 _currentDestination;
    public Slider HeathBar;
    public Slider EnergyBar;

    private float _currentHealth;
    private static float _maxHealth = 20f;

    private float _currentEnergy;
    private static float _maxEnergy = 20f;

    private bool _isRunning;
    private bool _isWalking;
    private float _oldPosition;

    void Start()
    {
        _currentEnergy = 20f;
        _currentHealth = 20f;

        HeathBar.value = CalculateHealth();
        EnergyBar.value = CalculateEnergy();

        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();

        _oldPosition = _transform.position.x;

        Messenger.AddListener("attack", Attacked);
    }

// Update is called once per frame
    void FixedUpdate()
    {
        if (_oldPosition == _transform.position.x) {
            _isRunning = false;
            _isWalking = false;
        } else if (_navMeshAgent.speed == 2f) {
            _isWalking = true;
            _isRunning = false;
        } else {
            _isRunning = true;
            _isWalking = false;
        }
        
        // if (_oldPosition != _transform.position.x && _navMeshAgent.speed == 2f) _isWalking = true
        // if (_oldPosition != _transform.position.x && _navMeshAgent.speed == 4f) _isRunning = true;
        _oldPosition = _transform.position.x;
        
        EnergyBar.value = CalculateEnergy();
        HeathBar.value = CalculateHealth();

        if (EnergyBar.value == 0) _navMeshAgent.speed = 2f;

        if (_isRunning && _currentEnergy > 0) {
            _currentEnergy -= 2f * Time.deltaTime;
        }

        if (!_isRunning && !_isWalking && _currentEnergy < 20f) {
            _currentEnergy += 1f * Time.deltaTime;
        }

        _animator.SetFloat("speed", _navMeshAgent.desiredVelocity.magnitude);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200.0f, LayerMask.GetMask("Ground")))
            {
                if (hit.transform != null)
                {
                    _navMeshAgent.speed = 2f;

                    _currentDestination = hit.point;
                    SetDestination();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200.0f, LayerMask.GetMask("Ground")))
            {
                if (hit.transform != null)
                {
                    _navMeshAgent.speed = 4f;

                    _currentDestination = hit.point;
                    SetDestination();
                }
            }
        }

        if (HeathBar.value == 0) Restart();

    }

    private void SetDestination()
    {
        if (_currentDestination != null)
        {
            _navMeshAgent.SetDestination(_currentDestination);
        }
    }

    // valor de la barra de salud
    float CalculateHealth(){
        return _currentHealth / _maxHealth;
    }

    // valor de la barra de energia
    float CalculateEnergy(){
        return _currentEnergy / _maxEnergy;
    }

//    private void CharacterDamaged(){
//        _currentHealth -= 1f;
//        HeathBar.value = CalculateHealth();
//    }

    private void Attacked()
    {
//        _ani.SetTrigger("isInjured");
        _currentHealth -= 2f * Time.deltaTime;
//        Messenger<float>.Broadcast("energy", initialEnergy);
    }

    private void Restart(){
        _transform.position = new Vector3(-46f, 1f, 45.517f);
        _navMeshAgent.SetDestination(_transform.position);
        _currentHealth = _maxHealth;
    }
}