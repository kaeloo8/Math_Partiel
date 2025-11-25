using UnityEngine;

/// <summary>
/// Stocke toutes les caractéristiques de la caméra de surveillance.
/// Les autres scripts viennent lire/écrire ici (détection, rotation, piratage…).
/// </summary>
public class CaracteristicSecurityCamera : MonoBehaviour
{
    [Header("Activation des fonctionnalités")]
    [Tooltip("Si false, la caméra ne cherche jamais le joueur.")]
    [SerializeField] private bool canDetectPlayer = true;

    [Tooltip("Si false, la caméra ne fait plus de balayage automatique.")]
    [SerializeField] private bool canRotate = true;

    [Header("Détection du joueur")]
    [Tooltip("Distance maximale à laquelle on peut détecter le joueur.")]
    [SerializeField] private float detectionDistance = 20f;

    [Header("Rotation automatique (balayage)")]
    [Tooltip("Angle total du balayage (ex: 60 -> -30° à +30°).")]
    [SerializeField] private float rotationRadius = 60f;

    [Tooltip("Vitesse du balayage (degrés par seconde).")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Rotation fixe quand la rotation est désactivée")]
    [Tooltip("Angle autour de Y quand la caméra est statique.")]
    [SerializeField] private float fixedAngleY = 0f;

    [Tooltip("Angle minimum autorisé pour fixedAngleY.")]
    [SerializeField] private float minAngleY = -70f;

    [Tooltip("Angle maximum autorisé pour fixedAngleY.")]
    [SerializeField] private float maxAngleY = 70f;

    /// <summary>True = la caméra essaye de détecter le joueur.</summary>
    public bool CanDetectPlayer
    {
        get => canDetectPlayer;
        set => canDetectPlayer = value;
    }

    /// <summary>True = la caméra fait un balayage gauche/droite.</summary>
    public bool CanRotate
    {
        get => canRotate;
        set => canRotate = value;
    }

    /// <summary>Distance maximale de détection.</summary>
    public float DetectionDistance
    {
        get => detectionDistance;
        set => detectionDistance = Mathf.Max(0f, value);
    }

    /// <summary>Angle total du balayage (ex: 60 -> -30° à +30°).</summary>
    public float RotationRadius
    {
        get => rotationRadius;
        set => rotationRadius = Mathf.Max(0f, value);
    }

    /// <summary>Vitesse de rotation (degrés/seconde).</summary>
    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = Mathf.Max(0f, value);
    }

    /// <summary>Angle fixe utilisé quand CanRotate == false.</summary>
    public float FixedAngleY
    {
        get => fixedAngleY;
        set => fixedAngleY = Mathf.Clamp(value, minAngleY, maxAngleY);
    }

    /// <summary>Limite minimum pour FixedAngleY.</summary>
    public float MinAngleY => minAngleY;

    /// <summary>Limite maximum pour FixedAngleY.</summary>
    public float MaxAngleY => maxAngleY;
}
