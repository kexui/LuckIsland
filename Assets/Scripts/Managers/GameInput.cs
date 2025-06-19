using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private GameControl gameControl;
    public event EventHandler OnDiceAction;

    private void Awake()
    {
        Instance = this;
        gameControl = new GameControl();

        gameControl.Camera.Enable();
        gameControl.Player.Enable();
    }
    private void Start()
    {
        if (gameControl!=null)
        {
            gameControl.Player.Dice.performed += Dice_performed;
            //gameControl.Player.Dice.performed += Di;
        }
        else
        {
            Debug.LogError("gameControlŒ¥≥ı ºªØ/Œ¥ø’");
        }
    }
    private void Dice_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
        OnDiceAction?.Invoke(this,EventArgs.Empty);
    }
    public Vector3 GetMovement()
    {
        Vector2 inputVector2 = gameControl.Camera.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputVector2.x, 0, inputVector2.y);
        return direction;
    }
    public Vector3 GetMoveMentDirectionNormalized()
    {
        Vector2 inputVector2 = gameControl.Camera.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputVector2.x, 0, inputVector2.y);
        direction = direction.normalized;
        //print(direction);
        return direction;
    }
    public float GetZoom()
    {
        float y = gameControl.Camera.Zoom.ReadValue<float>();
        return y;
    }
}
