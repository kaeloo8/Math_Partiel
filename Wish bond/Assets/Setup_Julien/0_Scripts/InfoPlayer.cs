using UnityEngine;

public class InfoPlayer : MonoBehaviour
{
    public static InfoPlayer Instance { get; private set; }

    [Header("Stats de déplacement (sera utilisé par le controller du joueur)")]
    [Range(0f, 10f)] public float walkSpeed = 5f;
    [Range(0f, 30f)] public float runSpeed = 15f;
    [Range(0f, 15f)] public float jumpHeight = 7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float JumpHeight => jumpHeight;

}
