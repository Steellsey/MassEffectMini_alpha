using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private float _playerMoveSpeed = 5f;
    [SerializeField] private float _playerRunningSpeed = 8f;
    [SerializeField] private float _playerRotateSpeed = 10f;
    [SerializeField] private new Camera camera;
    private float _playerCurrentSpeed;
    private bool _playerIsRunning;
    private bool _playerInCoverZone;
    private bool _playerIsCovered;
    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _playerCurrentSpeed = _playerMoveSpeed;
    }
    private void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);
        var viewVector = new Vector3(-_input.ViewVector.y, 0, _input.ViewVector.x);
        if (viewVector == Vector3.zero)
        {
            RotatePlayer(movementVector);
            if (_playerInCoverZone && !_playerIsCovered)
            {
                transform.localScale = new Vector3(transform.localScale.x*1.3f, transform.localScale.y/2f, transform.localScale.z*1.3f);//---------Cover Scale
                _playerIsCovered = true;
            }

        }
        else
        {
            RotatePlayer(viewVector);
            if (_playerInCoverZone)
            {
                if (_playerIsCovered)
                {
                    transform.localScale = new Vector3(transform.localScale.x/1.3f, transform.localScale.y*2f, transform.localScale.z/1.3f);//---------Normal Scale
                    _playerIsCovered = false;
                }
                else
                {
                    
                }
            }
        }

    }
    private void RotatePlayer(Vector3 viewVector)
    {
        if(viewVector.magnitude == 0) return;
        var rotation = Quaternion.LookRotation(viewVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _playerRotateSpeed * Time.timeScale);
    }
    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = _playerCurrentSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }
    public void PlayerCoverIn()
    {
        _playerInCoverZone = true;
    }
    public void PlayerCoverOut()
    {
        _playerInCoverZone = false;
        if (_playerIsCovered)
        {
            _playerIsCovered = false;
            transform.localScale = new Vector3(transform.localScale.x/1.3f, transform.localScale.y*2f, transform.localScale.z/1.3f);//---------
        }
    }
    public void ActionHandling()
    {
        if (!_playerInCoverZone)
        {
            CancelInvoke();
            _playerIsRunning = true;
            _playerCurrentSpeed = _playerRunningSpeed;
            Invoke("ActionStop", 0.5f);//---------
        }
        if (_playerInCoverZone)
        {
            print("Pepeprignut");//---------
        }
    }
    private void ActionStop()
    {
        _playerIsRunning = false;
        _playerCurrentSpeed = _playerMoveSpeed;
    }
}
