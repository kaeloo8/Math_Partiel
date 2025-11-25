using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Playermovement : MonoBehaviour
{
    [SerializeField] private playerscript _ps;

    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _run;
    [SerializeField] private InputActionReference _jump;
    [SerializeField] private InputActionReference _crouch;

    private bool _can_move = true;

    private float _speed = 0f;
    private bool _runing = false;
    private bool _jumping = true;
    private bool _crouching = false;

    private Vector2 _direction = Vector2.zero;


    void Start()
    {
        if (_move != null)
        {
            _move.action.performed += Input_move;
            _move.action.canceled += Input_move;
        }
        if (_run != null)
        {
            _run.action.started += Input_run;
        }
        if (_jump != null)
        {
            _jump.action.started += Input_jump;
        }
        if (_crouch != null)
        {
            _crouch.action.started += Input_crouch;
        }
        _speed = _ps.sp_walk;
    }
    
    void OnDestroy()
    {
        if (_move != null)
        {
            _move.action.started -= Input_move;
            _move.action.canceled -= Input_move;
        }
        if (_run != null)
        {
            _run.action.started -= Input_run;
        }
        if (_jump != null)
        {
            _jump.action.started -= Input_jump;
        }
        if (_crouch != null)
        {
            _crouch.action.started -= Input_crouch;
        }
    }

    private void Input_move(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<Vector2>().normalized;
    }
    private void Input_run(InputAction.CallbackContext ctx)
    {
        _runing = !_runing;
        if (_runing)
            _speed = _ps.sp_run;
        else
            _speed = _ps.sp_walk;
    }



    private void Input_jump(InputAction.CallbackContext ctx)
    {
        if (_jumping)
        {
            _jumping = false;
            Jump();
        }
    }
    private void Input_crouch(InputAction.CallbackContext ctx)
    {
        _crouching = !_crouching;
    }

    void Update()
    {
        if (_can_move)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 deplacement = new Vector3(_direction.x, 0, _direction.y);
        transform.Translate(deplacement* _speed * Time.deltaTime);
    }
    private void Run()
    {

    }
    private void Jump()
    {
        _jumping = false;
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.AddForce(Vector3.up * _ps.jump, ForceMode.Impulse);

        }
    }
    private void Crouch()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
            _jumping = true;
    }
}
