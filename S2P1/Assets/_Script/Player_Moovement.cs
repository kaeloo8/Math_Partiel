using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] bool isometricDeplacement = true;

    [Header("Dash")]
    [SerializeField] float dashDistance = 5f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] AfterimageSpawner afterimageSpawner;

    [Header("Animator")]
    [SerializeField] Animator animator;

    [Header("INPUT")]
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;

    private Vector2 moveInput; 
    private float gravity = -9.81f;
    private Vector3 velocity;
    private bool isDashing = false;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();



        moveAction = playerInput.actions["Move"];
        if (moveAction == null) { Debug.Log("move non trouvé"); }
        dashAction = playerInput.actions["Dash"];
        if (dashAction == null) { Debug.Log("Dash non trouvé"); }

    }

    void OnEnable()
    {
        if (playerInput == null)
            playerInput = GetComponentInParent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];

        if (moveAction != null) moveAction.Enable();
        if (dashAction != null)
        {
            dashAction.Enable();
            dashAction.performed += OnDashPerformed;
        }
    }

    void OnDisable()
    {
        moveAction.Disable();
        dashAction.performed -= OnDashPerformed;
        dashAction.Disable();
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        animator.SetBool("IsDashing", isDashing);

        if (!isDashing)
        {
            if (isometricDeplacement) MovePlayer_Isometric();
            else MovePlayer();
        }
    }

    void MovePlayer()
    {
        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);

        Vector3 moveVec = inputDirection.normalized * moveSpeed;
        animator.SetBool("IsWalking", moveVec != Vector3.zero);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        controller.Move((moveVec + velocity) * Time.deltaTime);

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDirection);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    void MovePlayer_Isometric()
    {
        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Quaternion isoRotation = Quaternion.Euler(0, 45f, 0);
        Vector3 isoDirection = isoRotation * inputDirection;

        Vector3 moveVec = isoDirection.normalized * moveSpeed;
        animator.SetBool("IsWalking", moveVec != Vector3.zero);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        controller.Move((moveVec + velocity) * Time.deltaTime);

        if (isoDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(isoDirection);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (!isDashing)
        {
            Vector3 dashDir = isometricDeplacement
                ? Quaternion.Euler(0, 45f, 0) * new Vector3(moveInput.x, 0, moveInput.y)
                : new Vector3(moveInput.x, 0, moveInput.y);

            if (dashDir.sqrMagnitude < 0.01f)
                dashDir = transform.forward;

            StartCoroutine(Dash(dashDir.normalized));
        }
    }

    public void Hit(float damage)
    {
        Debug.Log(damage + "player");
    }

    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        afterimageSpawner.StartSpawning();

        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            controller.Move(direction * (dashDistance / dashDuration) * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        afterimageSpawner.StopSpawning();
        isDashing = false;
    }
}
