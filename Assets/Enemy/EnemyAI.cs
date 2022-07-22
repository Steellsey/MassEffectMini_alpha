using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Globalization;

//[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _enemyMaxHealth = 1000f;
    [SerializeField] private GameObject _enemyHealthBarCanvas;
    [SerializeField] private Slider _enemyHealthBarSlider;
    [SerializeField] private float _enemyMeleeStrikeDistance = 2f;
    [SerializeField] [Range(0, 360)] private float _enemyViewAngle = 90f;
    [SerializeField] private float _enemyViewDistance = 15f;
    [SerializeField] private float _nearDetectionDistance = 5f;
    [SerializeField] private Transform _enemyViewPoint;
    [SerializeField] private Transform _target;
    private bool _isEnemyStrikeInProcess;
    private string _enemyStrikeType;
    private float _enemyHealth;
    private Camera _cam;
    private NavMeshAgent _navMeshAgent;
    private float _enemyRotationSpeed;
    private Transform _agentTransform;
    private bool _enemyGotDamage;

    private void Start()
    {
        _cam = Camera.main;
        _enemyHealth = _enemyMaxHealth;
        _enemyHealthBarSlider.value = CalculateHealth();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _enemyRotationSpeed = _navMeshAgent.angularSpeed;
        _agentTransform = _navMeshAgent.transform;
    }

    private void Update()
    {
        _enemyHealthBarSlider.transform.rotation = Quaternion.LookRotation(_enemyHealthBarSlider.transform.position - _cam.transform.position);

        float distanceToPlayer = Vector3.Distance(_target.transform.position, _navMeshAgent.transform.position);
        if (distanceToPlayer <= _nearDetectionDistance || IsInView() || _enemyGotDamage)
        {
            RotateToTarget();
            if (distanceToPlayer <= _enemyMeleeStrikeDistance)
            {
                _navMeshAgent.isStopped = true;
                Strike(_enemyStrikeType = "melee");
            }
            else
            {
                MoveToTarget();

            }
        }
        DrawViewState();
    }

    public void TakeDamage (int damage)
    {
        _enemyGotDamage = true;
        _enemyHealth -= damage;
        if (_enemyHealth <= 0)
            Die();
        _enemyHealthBarSlider.value = CalculateHealth();
        if (_enemyHealth < _enemyMaxHealth)
            _enemyHealthBarCanvas.SetActive(true);
    }
    private bool IsInView()
    {
        float realAngle = Vector3.Angle(_enemyViewPoint.forward, _target.position - _enemyViewPoint.position);
        RaycastHit hit;
        if (Physics.Raycast(_enemyViewPoint.transform.position, _target.position - _enemyViewPoint.position, out hit, _enemyViewDistance))
        {
            if (realAngle < _enemyViewAngle / 2f && Vector3.Distance(_enemyViewPoint.position, _target.position) <= _enemyViewDistance && hit.transform == _target.transform)
            {
                return true;
            }
        }
        return false;
    }
    void Die ()
    {
        Destroy(gameObject);
    }
    float CalculateHealth()
    {
        return _enemyHealth/_enemyMaxHealth;
    }

    private void RotateToTarget()
    {
        Vector3 lookVector = _target.position - _agentTransform.position;
        lookVector.y = 0;
        if (lookVector == Vector3.zero) return;
        _agentTransform.rotation = Quaternion.RotateTowards
            (
                _agentTransform.rotation,
                Quaternion.LookRotation(lookVector , Vector3.up),
                _enemyRotationSpeed * Time.deltaTime
            );
        
    }
    private void MoveToTarget()
    {
        _navMeshAgent.SetDestination(_target.position);
        _navMeshAgent.Resume();
    }
    private void DrawViewState() 
    {       
        Vector3 left = _enemyViewPoint.position + Quaternion.Euler(new Vector3(0, _enemyViewAngle / 2f, 0)) * (_enemyViewPoint.forward * _enemyViewDistance);
        Vector3 right = _enemyViewPoint.position + Quaternion.Euler(-new Vector3(0, _enemyViewAngle / 2f, 0)) * (_enemyViewPoint.forward * _enemyViewDistance);     
        Debug.DrawLine(_enemyViewPoint.position, left, Color.yellow);
        Debug.DrawLine(_enemyViewPoint.position, right, Color.yellow);       
    }

    private void Strike(string _enemyStrikeType)
    {
        if (!_isEnemyStrikeInProcess)
        {
            if (_enemyStrikeType == "melee")
            {
                MeleeStrike();
                Invoke("ResetStrike", 0.5f);//---------
                _isEnemyStrikeInProcess = true;
            }
            //else if (_enemyStrikeType = "b")
            //{
                //if (_bulletsCount > 0)
                //{
                    //Shoot();
                    //Invoke("ResetShoot", 0.5f);//---------
                    //_isEnemyStrikeInProcess = true;
                //}
                //else
                    //StartCoroutine(ReloadWeapon());
            //}
        }
    }

    private void ResetStrike()
    {
        _isEnemyStrikeInProcess = false;
    }

    private void MeleeStrike()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(_enemyViewPoint.position, _enemyViewPoint.forward, out hitInfo, _enemyMeleeStrikeDistance))
        {
            Debug.DrawLine(_enemyViewPoint.position, _enemyViewPoint.forward, Color.red);
            PlayerSpecsManager player = hitInfo.transform.GetComponent<PlayerSpecsManager>();
            if (player != null)
            {
                player.PlayerTakeDamage(40f);

            }
            //Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
        }
    }
}
