using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TopDownCharacterMover;

public class InputHandler : MonoBehaviour
{
    public Vector2 InputVector {get; private set;}
    public Vector2 ViewVector {get; private set;}

    //public Vector3 MousePosition { get; private set;}

    public Joystick joystickMovement;
    public Joystick joystickViewDirection;
    TopDownCharacterMover topDownCharacterMover;
    
    private void Awake() {
        topDownCharacterMover = gameObject.GetComponent<TopDownCharacterMover>();
    }

    void Update()
    {
        //var h = Input.GetAxis("Horizontal");
        var h = joystickMovement.Horizontal;
        //var v = Input.GetAxis("Vertical");
        var v = joystickMovement.Vertical;

        var hView = joystickViewDirection.Horizontal;
        var vView = joystickViewDirection.Vertical;
        InputVector = new Vector2(h, v);
        ViewVector = new Vector2(hView, vView);

        //MousePosition = Input.mousePosition;
    }

    public void ActionEvent()
    {
        topDownCharacterMover.ActionHandling();
    }
}
