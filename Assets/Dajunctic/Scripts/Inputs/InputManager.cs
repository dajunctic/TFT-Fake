using System;
using Dajunctic;
using UnityEngine;

public class InputManager: Singleton<InputManager>
{
    PlayerInputAction inputActions;
    
    public static event Action OnTestFirstSkill;
    public static event Action OnTestSecondSkill;
    public static event Action OnTestThirdSkill;
    public static event Action OnTestFourthSkill;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new PlayerInputAction();
        inputActions.Player.TestFirstSkill.performed += OnKeydownTestFirstSkill;
        inputActions.Player.TestSecondSkill.performed += OnKeydownTestSecondSkill;
        inputActions.Player.TestThirdSkill.performed += OnKeydownTestThirdSkill;
        inputActions.Player.TestFourthSkill.performed += OnKeydownTestFourthSkill;
    }

    private void OnKeydownTestFirstSkill(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnTestFirstSkill?.Invoke();
    }

    private void OnKeydownTestSecondSkill(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnTestSecondSkill?.Invoke();
    }

    private void OnKeydownTestThirdSkill(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnTestThirdSkill?.Invoke();
    }

    private void OnKeydownTestFourthSkill(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnTestFourthSkill?.Invoke();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }
    
    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    public Vector2 GetMoveInputVector()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

}