using UnityEngine;

/// <summary>
/// Fait tourner la caméra de gauche à droite autour de sa rotation de départ.
/// </summary>
public class SecurityCameraRotation : MonoBehaviour
{
    [Header("Référence vers les caractéristiques")]
    [Tooltip("Glisser ici le script CaracteristicSecurityCamera.")]
    public CaracteristicSecurityCamera caracteristic;

    private float baseY;

    private void Start()
    {
        if (caracteristic == null)
        {
            Debug.LogError("SecurityCameraRotation : aucune référence CaracteristicSecurityCamera assignée sur " + gameObject.name);
            enabled = false;
            return;
        }
        baseY = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (caracteristic == null)
            return;

        // Si la caméra ne doit plus tourner
        if (!caracteristic.CanRotate)
        {
            // On se cale sur l'angle fixe défini dans les caractéristiques
            Vector3 euler = transform.localEulerAngles;
            euler.y = baseY + caracteristic.FixedAngleY;
            transform.localEulerAngles = euler;
            return;
        }

        // Récupère les valeurs dans le script de caractéristiques
        float radius = caracteristic.RotationRadius;
        float speed = caracteristic.RotationSpeed;

        // Sécurité : si les paramètres sont 0 ou négatifs, on ne fait rien
        if (radius <= 0f || speed <= 0f)
            return;

        // Calcul du balayage gauche / droite
        float halfRadius = radius * 0.5f;
        float ping = Mathf.PingPong(Time.time * speed, radius); 
        float offset = ping - halfRadius;

        Vector3 eulerRot = transform.localEulerAngles;
        eulerRot.y = baseY + offset;
        transform.localEulerAngles = eulerRot;
    }
}
