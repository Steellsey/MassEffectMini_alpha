using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;

    [SerializeField]
    private float moveSpeed = 4f;
    [SerializeField]
    private float runSpeed = 6f;
    private bool playerRunning;
    private float currentSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private Camera camera;
    private bool playerInCoverZone;
    private bool playerCovered;
    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);
        var viewVector = new Vector3(-_input.ViewVector.y, 0, _input.ViewVector.x);
        if (viewVector == Vector3.zero)
        {
            RotateTowardMouseVector(movementVector);
            if (playerInCoverZone && !playerCovered)
            {
                transform.localScale = new Vector3(transform.localScale.x*1.3f, transform.localScale.y/2f, transform.localScale.z*1.3f);//---------Cover Scale
                playerCovered = true;
            }

        }
        else
        {
            RotateTowardMouseVector(viewVector);
            if (playerInCoverZone)
            {
                if (playerCovered)
                {
                    transform.localScale = new Vector3(transform.localScale.x/1.3f, transform.localScale.y*2f, transform.localScale.z/1.3f);//---------Normal Scale
                    playerCovered = false;
                }
                else
                {
                    
                }
            }
        }

    }

    private void RotateTowardMouseVector(Vector3 viewVector)
    {
        if(viewVector.magnitude == 0) return;
        var rotation = Quaternion.LookRotation(viewVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = currentSpeed * Time.deltaTime;
        //var speed;
        //if (!playerRunning)
            //speed = currentSpeed * Time.deltaTime;
        //else
            //speed = currentSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    public void PlayerCoverIn()
    {
        playerInCoverZone = true;
        //if (viewVector != Vector3.zero)
            //transform.localScale = new Vector3(transform.localScale.x*1.3f, transform.localScale.y/2f, transform.localScale.z*1.3f);//---------Normal Scale
    }
    public void PlayerCoverOut()
    {
        playerInCoverZone = false;
        if (playerCovered)
        {
            playerCovered = false;
            transform.localScale = new Vector3(transform.localScale.x/1.3f, transform.localScale.y*2f, transform.localScale.z/1.3f);//---------
        }
    }

    public void ActionHandling()
    {
        if (!playerInCoverZone)
        {
            CancelInvoke();
            playerRunning = true;
            currentSpeed = runSpeed;
            Invoke("ActionStop", 0.5f);//---------
        }
        if (playerInCoverZone)
        {
            print("Pepeprignut");
        }
    }

    private void ActionStop()
    {
        playerRunning = false;
        currentSpeed = moveSpeed;
    }
}
