using System;
using Dajunctic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager: Singleton<InputManager>
{
    [SerializeField] private InputActionReference firstSkillAction;
    [SerializeField] private InputActionReference secondSkillAction;
    [SerializeField] private InputActionReference thirdSkillAction;
    [SerializeField] private InputActionReference fourthSkillAction;

    [SerializeField] private InputActionReference rightClickAction;
    [SerializeField] private InputActionReference pointAction;
    [SerializeField] private InputActionReference emotionAction;
    [SerializeField] private InputActionReference buyXpAction;
    [SerializeField] private InputActionReference rerollAction;

    public static event Action OnTestFirstSkill;
    public static event Action OnTestSecondSkill;
    public static event Action OnTestThirdSkill;
    public static event Action OnTestFourthSkill;

    public static event Action<Vector2> OnRightClickEvent;

    private Camera _camera;

    private bool IsInputBlocked()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            
            var input = EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>();
            if (input != null && input.isFocused) return true;

            var inputLegacy = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
            if (inputLegacy != null && inputLegacy.isFocused) return true;
        }
        return false;
    }

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

        buyXpAction.action.performed += OnBuyXP;

        rerollAction.action.performed += OnReroll;

        _camera = Camera.main;
    }

    private void OnBuyXP(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        
        this.Raise(new RequestBuyXPEvent());
    }

    private void OnReroll(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        
        this.Raise(new RequestRerollEvent());
    }

    private void OnEmotionStarted(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        var mousePosition = pointAction.action.ReadValue<Vector2>();

        this.Raise(new ShowEmotionUIEvent
        {
            Enable = true,
            Position = mousePosition
        });
    }

    private void OnEmotionCanceled(InputAction.CallbackContext context)
    {
        
        this.Raise(new ShowEmotionUIEvent
        {
            Enable = false
        });
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        
        if (IsInputBlocked()) return;
        Vector2 mousePosition = pointAction.action.ReadValue<Vector2>();

        if (_camera == null) _camera = Camera.main;
        if (_camera != null)
        {
            var ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var targetPosition = hitInfo.point;

                this.Raise(new SpawnFxEvent
                {
                    id = "fx_click_marker",
                    position = targetPosition,
                });
            }
        }
       
        OnRightClickEvent?.Invoke(mousePosition);
    }

    private void OnKeydownTestFirstSkill(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        OnTestFirstSkill?.Invoke();
    }

    private void OnKeydownTestSecondSkill(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        OnTestSecondSkill?.Invoke();
    }

    private void OnKeydownTestThirdSkill(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        OnTestThirdSkill?.Invoke();
    }

    private void OnKeydownTestFourthSkill(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        OnTestFourthSkill?.Invoke();
    }

    private void OnEnable()
    {
        firstSkillAction.action.Enable();
        secondSkillAction.action.Enable();
        thirdSkillAction.action.Enable();
        fourthSkillAction.action.Enable();
        pointAction.action.Enable();
        rightClickAction.action.Enable();
        emotionAction.action.Enable();
        buyXpAction.action.Enable();
        rerollAction.action.Enable();
    }
    
    private void OnDisable()
    {
        firstSkillAction.action.Disable();
        secondSkillAction.action.Disable();
        thirdSkillAction.action.Disable();
        fourthSkillAction.action.Disable();
        pointAction.action.Disable();
        rightClickAction.action.Disable();
        emotionAction.action.Disable();
        buyXpAction.action.Disable();
        rerollAction.action.Disable();
    
    }

}
