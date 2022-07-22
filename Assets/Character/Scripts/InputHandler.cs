using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TopDownCharacterMover;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick joystickMovementDirection;
    [SerializeField] private Joystick joystickViewDirection;
    public Vector2 InputVector {get; private set;}
    public Vector2 ViewVector {get; private set;}
    TopDownCharacterMover topDownCharacterMover;
    
    private void Awake() {
        topDownCharacterMover = gameObject.GetComponent<TopDownCharacterMover>();
    }
    private void Update()
    {
        var horizontalAxisMoveDirection = joystickMovementDirection.Horizontal;
        var verticalAxisMoveDirection = joystickMovementDirection.Vertical;
        var horizontalAxisViewDirection = joystickViewDirection.Horizontal;
        var verticalAxisViewDirection = joystickViewDirection.Vertical;
        InputVector = new Vector2(horizontalAxisMoveDirection, verticalAxisMoveDirection);
        ViewVector = new Vector2(horizontalAxisViewDirection, verticalAxisViewDirection);
    }
    public void ActionEvent()
    {
        topDownCharacterMover.ActionHandling();
    }
}
