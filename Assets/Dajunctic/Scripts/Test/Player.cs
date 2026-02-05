using KBCore.Refs;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Player : ValidatedMonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 20f;

    [SerializeField, Child] Animator animator;
    
    private Vector2 m_MoveInput;

    public Transform Transform { get; private set; }
    void Awake()
    {
        Transform = GetComponent<Transform>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        var moveDirection = new Vector3(m_MoveInput.x, 0, m_MoveInput.y).normalized;
        Transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Transform.rotation = Quaternion.Lerp(Transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        animator.SetFloat("Speed", moveDirection.magnitude * moveSpeed);
    }

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
        
    }

    public void HandleMove(CallbackContext ctx)
    {
        m_MoveInput = ctx.ReadValue<Vector2>();
    }
}
