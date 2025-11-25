using System.Collections;
using UnityEngine;

[SerializeField] public enum SkeletonType { Minion, Tank, Ranged, Mage }

public class Skeleton_Movement : MonoBehaviour
{
    public enum SkeletonState { Idle, Chase, Attack, Stun, Die }

    [Header("References")]
    private Skeleton_Detection detection;
    [SerializeField] private Skeleton_Stats stats;
    [SerializeField] private Animator animator;

    [Header("Combat Settings")]
    [SerializeField] private SkeletonType type;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float hitFlashTime;

    private Transform _target;
    private Vector2 _targetOffset;
    private readonly int[] choices = { -1, 0, 1 };
    private Renderer[] renderers;
    private Color[] baseColors;

    private SkeletonState currentState = SkeletonState.Idle;
    bool canMove = false; 
    private float _lastAttackTime = 0f;
    private float stunTimer = 0f;

    public void EnableMovement()
    {
        canMove = true;
    }
    private void Awake()
    {
        detection = GetComponentInChildren<Skeleton_Detection>();
    }

    private void Start()
    {
        stats.health = stats.Maxhealth;
        detection.OnPlayerDetected += SetTarget;
        renderers = GetComponentsInChildren<Renderer>();

        baseColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            baseColors[i] = renderers[i].material.color;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case SkeletonState.Idle:
                break;

            case SkeletonState.Chase:
                if (canMove) ChasePlayer();
                break;

            case SkeletonState.Attack:
                AttackPlayer();
                break;

            case SkeletonState.Stun:
                StunTime();
                break;

            case SkeletonState.Die:
                break;
        }
    }

    private void SetTarget(Transform target)
    {
        _target = target;
        _targetOffset = new Vector2(
            choices[Random.Range(0, choices.Length)],
            choices[Random.Range(0, choices.Length)]
        );

        ChangeState(SkeletonState.Chase);
    }

    private void ChasePlayer()
    {
        if (_target == null) return;

        Vector3 targetPos = _target.position + (Vector3)_targetOffset;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            stats.speed * Time.deltaTime
        );

        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * stats.rotationspeed
            );
        }

        float dist = Vector3.Distance(transform.position, _target.position);
        if (dist <= attackDistance)
            ChangeState(SkeletonState.Attack);
    }

    private void AttackPlayer()
    {
        if (Time.time - _lastAttackTime < stats.AttackCooldown)
            return;

        animator.SetTrigger("BasicAttack");
        _lastAttackTime = Time.time;

        float dist = Vector3.Distance(transform.position, _target.position);
        if (dist > attackDistance)
            ChangeState(SkeletonState.Chase);
    }

    public void Hit(Transform Owner, Transform Item, float Damage, bool Stunt, float StuntTime, float BumpForce)
    {
        if (Owner == null)
        {
            detection.SetPlayerTransform(Owner);
            SetTarget(Owner);
            EnableMovement();
            detection.SetBoolDetectedPlayer(true);
        }

        stats.health -= Damage;
        StartCoroutine(FlashRed());
        animator.SetTrigger("Hit");

        if (Stunt)
            Stun(StuntTime);
        if (BumpForce > 0)
            Bump(Item, BumpForce);

    }

    private IEnumerator FlashRed()
    {
        foreach (var r in renderers)
            r.material.color = Color.red;

        yield return new WaitForSeconds(hitFlashTime);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = baseColors[i];
    }


    public void Stun(float duration)
    {
        ChangeState(SkeletonState.Stun);
        animator.SetTrigger("Stunt");
        animator.SetBool("Stunt Bool", true);
        stunTimer = duration;
    }
    public void Bump(Transform bumporigin, float bumpForce)
    {
        Vector3 direction = (transform.position - bumporigin.position);
        direction.y = 0f;
        direction.Normalize();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * bumpForce, ForceMode.Impulse);
        }
        else
        {
            transform.position += direction * bumpForce;
        }
    }

    public void StunTime()
    {
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                animator.SetBool("Stunt Bool", false);
                RecoverFromStun();
            }
        }
    }

    private void RecoverFromStun() => ChangeState(SkeletonState.Chase);

    public void Die()
    {
        ChangeState(SkeletonState.Die);
        Destroy(gameObject, 2f);
    }

    private void ChangeState(SkeletonState newState)
    {
        currentState = newState;
        animator.SetBool("IsWalking", currentState == SkeletonState.Chase);
    }

}