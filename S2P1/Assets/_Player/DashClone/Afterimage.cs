using UnityEngine;

public class Afterimage : MonoBehaviour
{
    public float shrinkSpeed = 2f;   // vitesse à laquelle l'afterimage rapetisse
    public float minScale = 0.1f;    // taille minimale avant destruction

    private Vector3 initialScale;

    void Awake()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Réduction progressive
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        // Si trop petit, détruire
        if (transform.localScale.x <= minScale ||
            transform.localScale.y <= minScale ||
            transform.localScale.z <= minScale)
        {
            Destroy(gameObject);
        }
    }
}
