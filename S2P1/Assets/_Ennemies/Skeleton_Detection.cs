using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton_Detection : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionDistance = 5f;
    [SerializeField] private float detectionTime = 2f;
    [SerializeField] private SphereCollider detectionZone;

    [Header("UI Feedback")]
    [SerializeField] private Image questionMark;
    [SerializeField] private Image questionFill;
    [SerializeField] private Image findSprite;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Skeleton_Stats stats;

    public event System.Action<Transform> OnPlayerDetected;

    private Transform _playerTransform;
    private Coroutine detectionRoutine;
    private bool hasDetectedPlayer = false;

    public bool GetBoolDetectedPlayer()
    {
        return hasDetectedPlayer;
    }
    public void SetBoolDetectedPlayer(bool param)
    {
        hasDetectedPlayer = param;
    }

    public Transform GetPlayerTransform()
    {
        return _playerTransform;
    }
    public void SetPlayerTransform(Transform a)
    {
        _playerTransform = a;
    }

    private void Start()
    {
        detectionZone.radius = detectionDistance;
        ResetUI();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player_Movement>())
        {
            if (hasDetectedPlayer) return;

            _playerTransform = other.transform;
            if (detectionRoutine == null)
                detectionRoutine = StartCoroutine(DetectPlayer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player_Movement>())
        {
            if (hasDetectedPlayer) return;

            if (detectionRoutine != null)
            {
                StopCoroutine(detectionRoutine);
                detectionRoutine = null;
            }
            ResetUI();
        }
    }

    private IEnumerator DetectPlayer()
    {
        questionMark.gameObject.SetActive(true);
        findSprite.gameObject.SetActive(false);
        questionFill.fillAmount = 0f;

        float elapsed = 0f;
        bool halfTriggered = false;

        while (elapsed < detectionTime)
        {
            elapsed += Time.deltaTime;
            questionFill.fillAmount = Mathf.Clamp01(elapsed / detectionTime);

            if (!halfTriggered && elapsed >= detectionTime / 2f)
            {
                halfTriggered = true;
                CallHalfTimeFunction();
            }

            yield return null;
        }

        questionMark.gameObject.SetActive(false);
        findSprite.gameObject.SetActive(true);

        hasDetectedPlayer = true;
        OnPlayerDetected?.Invoke(_playerTransform);

        yield return new WaitForSeconds(0.5f);
        findSprite.gameObject.SetActive(false);
    }
    void CallHalfTimeFunction()
    {
        animator.SetTrigger("Detect");

        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            if (transform.parent != null)
            {
                transform.parent.rotation = lookRotation;
            }
        }
    }

    private void ResetUI()
    {
        questionMark.gameObject.SetActive(false);
        findSprite.gameObject.SetActive(false);
        questionFill.fillAmount = 0f;
    }
}