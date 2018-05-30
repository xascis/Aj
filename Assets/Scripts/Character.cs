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
    private Vector3 currentDestination;
    public Slider heathBar;
    public Slider energyBar;
    private bool _isRunning;
    private bool _isWalking;
    private float _oldPosition;

    void Start()
    {
        heathBar.value = CalculateHealth();
        energyBar.value = CalculateEnergy();

        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();

        _oldPosition = _transform.position.x;
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
        
        energyBar.value = CalculateEnergy();

        if (energyBar.value == 0) _navMeshAgent.speed = 2f;

        if (_isRunning && GameManager.currentEnergy > 0) {
            GameManager.currentEnergy -= 2f * Time.deltaTime;
        }

        if (!_isRunning && !_isWalking && GameManager.currentEnergy < 20f) {
            GameManager.currentEnergy += 1f * Time.deltaTime;
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

                    currentDestination = hit.point;
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

                    currentDestination = hit.point;
                    SetDestination();
                }
            }
        }

    }

    private void SetDestination()
    {
        if (currentDestination != null)
        {
            _navMeshAgent.SetDestination(currentDestination);
        }
    }

    float CalculateHealth(){
        return GameManager.currentHealth / GameManager.maxHealth;
    }

    private void CharacterDamaged(){
        GameManager.currentHealth -= 1f;
        heathBar.value = CalculateHealth();
    }

    float CalculateEnergy(){
        return GameManager.currentEnergy / GameManager.maxEnergy;
    }
}