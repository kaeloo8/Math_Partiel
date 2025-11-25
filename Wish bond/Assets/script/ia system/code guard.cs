using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    FollowRoutine,
    SawPlayer,
    Suspicious,
    Chase,
    Attack,
    Dead,
}

public class codeguard : MonoBehaviour
{
    private EnemyState state = EnemyState.Idle;

    [Header("References")]
    [SerializeField] private guardbase _gb;
    [SerializeField] private NavMeshAgent _agent;

    [Header("Patrol Parameters")]
    [SerializeField] private List<Transform> _liste_position;
    [SerializeField] private float TimeToWaitBetweenPoint = 2f;
    [SerializeField] private float TimeBeforeFindPlayer = 2f;
    [SerializeField] private float PreDetectionDelay = 0.5f;
    [SerializeField] private float SuspiciousDuration = 3f;

    public Transform _target_pos;
    private List<GameObject> _liste_object_see = new List<GameObject>();

    [Header("UI")]
    [SerializeField] private Image q1;
    [SerializeField] private Image q2;

    private int _currentIndex = 0;
    private float _waitTimer = 0f;
    private float _watchTimer = 0f;
    private float _preDetectTimer = 0f;
    private float _suspiciousTimer = 0f;
    private Quaternion _suspiciousBaseRotation;

    void Start()
    {
        if (_gb == null || !TryGetComponent(out _agent))
        {
            Debug.LogError("Guard setup error, destruction");
            Destroy(this);
            return;
        }

        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.Idle: UpdateIdle(); break;
            case EnemyState.FollowRoutine: UpdatePatrol(); break;
            case EnemyState.SawPlayer: WatchPlayer(); break;
            case EnemyState.Suspicious: UpdateSuspicious(); break;
            case EnemyState.Chase: UpdateChase(); break;
        }

        HandleVisionDetection();
    }

    private void UpdateIdle()
    {
        _waitTimer -= Time.deltaTime;
        if (_waitTimer <= 0)
            ChangeState(EnemyState.FollowRoutine);
    }

    private void UpdatePatrol()
    {
        if (_liste_position.Count == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            ChangeState(EnemyState.Idle);
    }

    private void MoveToNextPoint()
    {
        if (_liste_position.Count == 0) return;

        _agent.SetDestination(_liste_position[_currentIndex].position);
        _currentIndex = (_currentIndex + 1) % _liste_position.Count;
    }

    private void HandleVisionDetection()
    {
        if (_target_pos != null && _liste_object_see.Contains(_target_pos.gameObject))
        {
            _preDetectTimer += Time.deltaTime;

            if (_preDetectTimer >= PreDetectionDelay && state != EnemyState.SawPlayer && state != EnemyState.Chase)
                ChangeState(EnemyState.SawPlayer);
        }
        else
        {
            _preDetectTimer = 0f;
        }
    }

    private void WatchPlayer()
    {
        if (_target_pos == null)
        {
            ChangeState(EnemyState.Suspicious);
            return;
        }

        Vector3 dir = (_target_pos.position - transform.position);
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }

        if (q1 != null) q1.gameObject.SetActive(true);
        if (q2 != null) q2.gameObject.SetActive(true);

        _watchTimer += Time.deltaTime;
        if (q2 != null)
            q2.fillAmount = Mathf.Clamp01(_watchTimer / TimeBeforeFindPlayer);

        if (_watchTimer >= TimeBeforeFindPlayer)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        if (!_liste_object_see.Contains(_target_pos.gameObject))
        {
            ChangeState(EnemyState.Suspicious);
            return;
        }
    }

    private void UpdateChase()
    {
        if (_target_pos == null)
        {
            ChangeState(EnemyState.Suspicious);
            return;
        }

        Vector3 dir = (_target_pos.position - transform.position);
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }

        _agent.SetDestination(_target_pos.position);

        if (!_liste_object_see.Contains(_target_pos.gameObject))
            ChangeState(EnemyState.Suspicious);
    }

    private void UpdateSuspicious()
    {
        _suspiciousTimer += Time.deltaTime;

        float angleOffset = Mathf.Sin(_suspiciousTimer * 2f) * 45f;
        Quaternion offsetRotation = Quaternion.Euler(0, angleOffset, 0);
        transform.rotation = _suspiciousBaseRotation * offsetRotation;

        if (_suspiciousTimer >= SuspiciousDuration)
            ChangeState(EnemyState.FollowRoutine);
    }

    public void ChangeState(EnemyState _state)
    {
        state = _state;

        if (q1 != null) q1.gameObject.SetActive(false);
        if (q2 != null) q2.gameObject.SetActive(false);

        if (state == EnemyState.Idle)
        {
            _waitTimer = TimeToWaitBetweenPoint;
        }
        else if (state == EnemyState.FollowRoutine)
        {
            MoveToNextPoint();
        }
        else if (state == EnemyState.SawPlayer)
        {
            _watchTimer = 0f;
            if (q2 != null) q2.fillAmount = 0f;
        }
        else if (state == EnemyState.Suspicious)
        {
            _suspiciousTimer = 0f;
            _suspiciousBaseRotation = transform.rotation;
        }
    }

    public void AddTargetView(GameObject other)
    {
        if (!_liste_object_see.Contains(other))
            _liste_object_see.Add(other);

        if (other.GetComponent<Playermovement>())
            _target_pos = other.transform;
    }

    public void RemoveTargetView(GameObject other)
    {
        _liste_object_see.Remove(other);
    }
}