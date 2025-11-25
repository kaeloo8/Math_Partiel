using UnityEngine;

public class BoomerangProjectile : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform owner;

    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float travelDistance = 10f;

    [Header("Rotation")]
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private Vector3 spinAxis = Vector3.forward;

    private Vector3 startPos;
    private bool returning = false;
    private Vector3 moveDir; 

    public Transform GetOwner()
    { return owner; }

    public void Init(Transform user, float speed, float spinSpeed, float travelDistance, Vector3 spinAxis)
    {
        owner = user;
        this.speed = speed;
        this.spinSpeed = spinSpeed;
        this.travelDistance = travelDistance;
        this.spinAxis = spinAxis;

        startPos = transform.position;
        moveDir = transform.forward;
    }

    void Update()
    {
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);

        if (!returning)
        {
            transform.position += moveDir * speed * Time.deltaTime;

            if (Vector3.Distance(startPos, transform.position) >= travelDistance)
            {
                returning = true;
            }
        }
        else
        {
            Vector3 dir = (owner.position + new Vector3(0,1,0) - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            if (Vector3.Distance(owner.position, transform.position) < 1.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}