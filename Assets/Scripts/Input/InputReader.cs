using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu (menuName = "InputReader")]
public class InputReader : ScriptableObject, InputMap.IIslandMovementActions, InputMap.IVoidMovementActions, InputMap.IMenuMapActions
{
    private InputMap inputMap;
    public bool islandMode { get; private set; } = true;
    public bool voidMode { get; private set; } = false;
    public bool menuMode { get; private set; } = false;

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new InputMap();

            inputMap.IslandMovement.SetCallbacks(this);
            inputMap.VoidMovement.SetCallbacks(this);

            SetIslandMovement();
        }
    }

    private void SetIslandMovement()
    {
        inputMap.IslandMovement.Enable();
        inputMap.VoidMovement.Disable();
        inputMap.MenuMap.Disable();
        islandMode = true;
        voidMode = false;
        menuMode = false;
    }

    private void SetVoidMovement()
    {
        inputMap.IslandMovement.Disable();
        inputMap.VoidMovement.Enable();
        inputMap.MenuMap.Disable();
        islandMode = false;
        voidMode = true;
        menuMode = false;
    }

    private void SetMenuMode()
    {
        inputMap.IslandMovement.Disable();
        inputMap.VoidMovement.Disable();
        inputMap.MenuMap.Enable();
        islandMode = false;
        voidMode = false;
        menuMode = true;
    }

    //EVENTS
    public event Action<float> MoveEvent;
    public event Action UseEvent;
    public event Action ClickEvent;
    public event Action ClickCanceledEvent;
    public event Action JumpEvent;
    public event Action SelectEvent;
    public event Action<float> ScrollEvent;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ClickEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            ClickCanceledEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(obj:context.ReadValue<float>());
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            UseEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        ScrollEvent?.Invoke(obj:context.ReadValue<float>());
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SelectEvent?.Invoke();
        }
    }
}