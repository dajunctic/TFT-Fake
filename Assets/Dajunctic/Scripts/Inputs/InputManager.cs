using System;
using Dajunctic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager: Singleton<InputManager>
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference firstSkillAction;
    [SerializeField] private InputActionReference secondSkillAction;
    [SerializeField] private InputActionReference thirdSkillAction;
    [SerializeField] private InputActionReference fourthSkillAction;

    [SerializeField] private InputActionReference rightClickAction;
    [SerializeField] private InputActionReference pointAction;
    [SerializeField] private InputActionReference emotionAction;

    
    public static event Action OnTestFirstSkill;
    public static event Action OnTestSecondSkill;
    public static event Action OnTestThirdSkill;
    public static event Action OnTestFourthSkill;

    public static event Action<Vector2> OnRightClickEvent;

    protected override void Awake()
    {
        base.Awake();
        rightClickAction.action.performed += OnRightClick;

        firstSkillAction.action.performed += OnKeydownTestFirstSkill;
        secondSkillAction.action.performed += OnKeydownTestSecondSkill;
        thirdSkillAction.action.performed += OnKeydownTestThirdSkill;
        fourthSkillAction.action.performed += OnKeydownTestFourthSkill;


        emotionAction.action.started += OnEmotionStarted;
        emotionAction.action.canceled += OnEmotionCanceled;
    }


    private void OnEmotionStarted(InputAction.CallbackContext context)
    {
        var mousePosition = pointAction.action.ReadValue<Vector2>();

        var data = new ShowEmotionUIEvent
        {
            Enable = true,
            Position = mousePosition
        };
        // this.Raise(data);

        FindFirstObjectByType<EmotionManager>()?.ToggleEmotionUI(data);  
    }

    private void OnEmotionCanceled(InputAction.CallbackContext context)
    {
        var data = new ShowEmotionUIEvent
        {
            Enable = false
        };
        // this.Raise(data);
        FindFirstObjectByType<EmotionManager>()?.ToggleEmotionUI(data);

    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = pointAction.action.ReadValue<Vector2>();
        OnRightClickEvent?.Invoke(mousePosition);
    }

    private void OnKeydownTestFirstSkill(InputAction.CallbackContext context)
    {
        OnTestFirstSkill?.Invoke();
    }

    private void OnKeydownTestSecondSkill(InputAction.CallbackContext context)
    {
        OnTestSecondSkill?.Invoke();
    }

    private void OnKeydownTestThirdSkill(InputAction.CallbackContext context)
    {
        OnTestThirdSkill?.Invoke();
    }

    private void OnKeydownTestFourthSkill(InputAction.CallbackContext context)
    {
        OnTestFourthSkill?.Invoke();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        firstSkillAction.action.Enable();
        secondSkillAction.action.Enable();
        thirdSkillAction.action.Enable();
        fourthSkillAction.action.Enable();
        pointAction.action.Enable();
        rightClickAction.action.Enable();
        emotionAction.action.Enable();
    }
    
    private void OnDisable()
    {
        moveAction.action.Disable();
        firstSkillAction.action.Disable();
        secondSkillAction.action.Disable();
        thirdSkillAction.action.Disable();
        fourthSkillAction.action.Disable();
        pointAction.action.Disable();
        rightClickAction.action.Disable();
        emotionAction.action.Disable();
    }
    public Vector2 GetMoveInputVector()
    {
        return moveAction.action.ReadValue<Vector2>();
    }

}