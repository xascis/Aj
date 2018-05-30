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
    public bool _isRunning;
    public float oldPosition;

    void Start()
    {
        heathBar.value = CalculateHealth();
        energyBar.value = CalculateEnergy();

        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();

        oldPosition = _transform.position.x;
    }

// Update is called once per frame
    void FixedUpdate()
    {
        if (oldPosition == _transform.position.x) _isRunning = false;
        if (oldPosition != _transform.position.x) _isRunning = true;
        oldPosition = _transform.position.x;
        
        print(_transform.position.x);
        energyBar.value = CalculateEnergy();

        if (energyBar.value == 0) _navMeshAgent.speed = 2f;

        if (_isRunning && GameManager.currentEnergy > 0) GameManager.currentEnergy -= 2f * Time.deltaTime;
        if (!_isRunning && GameManager.currentEnergy < 20f) {
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