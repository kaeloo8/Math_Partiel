using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SecurityCameraFrustum : MonoBehaviour
{
    [Header("Référence vers les caractéristiques")]
    public CaracteristicSecurityCamera caracteristic;

    [Header("Réglages principaux")]
    [Tooltip("Tag de la cible à détecter (joueur).")]
    [SerializeField] private string targetTag = "Player";

    [Tooltip("Layers pris en compte pour le raycast (mur, joueur, etc.).")]
    [SerializeField] private LayerMask visibilityMask = ~0;

    [Header("Objet qui affiche la zone rouge")]
    [Tooltip("GameObject qui contient le mesh rouge")]
    [SerializeField] private GameObject frustumObject;

    [Tooltip("MeshFilter")]
    [SerializeField] private MeshFilter frustumMeshFilter;

    [Header("Debug")]
    [Tooltip("True si au moins un joueur est vu ce frame.")]
    public bool viewPlayer = false;

    [Tooltip("Afficher des Debug.Log quand le joueur entre/sort du champ de vision.")]
    public bool debugLogs = true;

    // Références internes
    private Camera cam;
    private Mesh frustumMesh;

    public bool ViewPlayer => viewPlayer;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (caracteristic == null)
        {
            Debug.LogError("SecurityCameraFrustum : aucune référence CaracteristicSecurityCamera assignée sur " + gameObject.name);
            enabled = false;
            return;
        }

        if (frustumMeshFilter == null && frustumObject != null)
        {
            frustumMeshFilter = frustumObject.GetComponent<MeshFilter>();
        }

        // On s'assure que la caméra voit au moins jusqu'à la distance de détection
        float distance = caracteristic.DetectionDistance;
        if (cam.farClipPlane < distance)
            cam.farClipPlane = distance;

        // On génère une première fois le mesh du cône rouge
        BuildFrustumMesh();

        // Au démarrage, on affiche/masque le frustum selon l'état de la détection
        SetFrustumVisible(caracteristic.CanDetectPlayer);
    }

    private void Update()
    {
        if (caracteristic == null)
            return;

        // Si la détection est désactivée, on coupe la zone rouge et on force viewPlayer à false
        if (!caracteristic.CanDetectPlayer || caracteristic.DetectionDistance <= 0f)
        {
            SetFrustumVisible(false);

            if (viewPlayer)
            {
                viewPlayer = false;

                if (debugLogs)
                    Debug.Log("SecurityCameraFrustum : détection désactivée -> viewPlayer = false sur " + gameObject.name);
            }

            return;
        }

        // Détection active -> on affiche le GameObject de frustum
        SetFrustumVisible(true);

        CheckPlayerVisibility();
    }

    /// <summary>
    /// Active ou désactive le GameObject qui affiche le mesh rouge de détection.
    /// </summary>
    private void SetFrustumVisible(bool visible)
    {
        if (frustumObject == null)
            return;
        if (frustumObject.activeSelf != visible)
        {
            frustumObject.SetActive(visible);
        }
    }

    /// <summary>
    /// Regarde si un joueur est visible par la caméra ce frame.
    /// Met à jour viewPlayer + les Debug.Log.
    /// </summary>
    private void CheckPlayerVisibility()
    {
        bool playerSeenThisFrame = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject player in players)
        {
            Collider col = player.GetComponent<Collider>();
            if (col == null)
                continue;

            if (CanSeeCollider(col))
            {
                playerSeenThisFrame = true;
                break;
            }
        }

        if (playerSeenThisFrame && !viewPlayer)
        {
            viewPlayer = true;

            if (debugLogs)
                Debug.Log("Player détecté par " + gameObject.name);
        }
        else if (!playerSeenThisFrame && viewPlayer)
        {
            viewPlayer = false;

            if (debugLogs)
                Debug.Log("Player perdu de vue par " + gameObject.name);
        }
    }

    /// <summary>
    /// Retourne true si le collider est dans le cône de vision
    /// ET qu'aucun mur ne bloque la vue.
    /// </summary>
    private bool CanSeeCollider(Collider col)
    {
        if (cam == null || caracteristic == null)
            return false;

        Bounds bounds = col.bounds;
        Vector3 center = bounds.center;

        // On teste plusieurs points dans le collider (centre, haut, bas, gauche, droite)
        Vector3[] testPoints =
        {
            center,
            new Vector3(center.x, bounds.max.y, center.z),
            new Vector3(center.x, bounds.min.y, center.z),
            new Vector3(bounds.min.x, center.y, center.z),
            new Vector3(bounds.max.x, center.y, center.z),
        };

        float maxDistance = caracteristic.DetectionDistance;
        if (maxDistance <= 0f)
            return false;

        foreach (Vector3 worldPoint in testPoints)
        {
            Vector3 origin = cam.transform.position;
            Vector3 dir = worldPoint - origin;
            float distance = dir.magnitude;

            if (distance > maxDistance)
                continue;

            dir /= distance;

            Vector3 viewportPos = cam.WorldToViewportPoint(worldPoint);

            if (viewportPos.z <= 0f)
                continue;

            if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
                continue;

            if (Physics.Raycast(origin, dir, out RaycastHit hit, maxDistance, visibilityMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider == col || hit.collider.transform.IsChildOf(col.transform))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Construit le mesh du volume de vision (cône rouge)
    /// en fonction du FOV de la caméra et de la distance de détection.
    /// </summary>
    private void BuildFrustumMesh()
    {
        if (frustumMeshFilter == null)
        {
            Debug.LogWarning("SecurityCameraFrustum : aucun MeshFilter assigné pour le frustum.");
            return;
        }

        if (frustumMesh == null)
        {
            frustumMesh = new Mesh();
            frustumMesh.name = "SecurityCameraFrustumMesh";
        }
        else
        {
            frustumMesh.Clear();
        }

        if (cam == null)
            cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("SecurityCameraFrustum : impossible de trouver la Camera.");
            return;
        }

        float near = cam.nearClipPlane;
        float far = caracteristic != null ? caracteristic.DetectionDistance : cam.farClipPlane;

        if (far <= near)
            far = near + 0.01f;

        float fov = cam.fieldOfView * Mathf.Deg2Rad;
        float aspect = cam.aspect;

        float halfHeightNear = Mathf.Tan(fov * 0.5f) * near;
        float halfWidthNear = halfHeightNear * aspect;

        float halfHeightFar = Mathf.Tan(fov * 0.5f) * far;
        float halfWidthFar = halfHeightFar * aspect;

        Vector3[] vertices = new Vector3[8];

        vertices[0] = new Vector3(-halfWidthNear, -halfHeightNear, near);
        vertices[1] = new Vector3(halfWidthNear, -halfHeightNear, near);
        vertices[2] = new Vector3(halfWidthNear, halfHeightNear, near);
        vertices[3] = new Vector3(-halfWidthNear, halfHeightNear, near);

        vertices[4] = new Vector3(-halfWidthFar, -halfHeightFar, far);
        vertices[5] = new Vector3(halfWidthFar, -halfHeightFar, far);
        vertices[6] = new Vector3(halfWidthFar, halfHeightFar, far);
        vertices[7] = new Vector3(-halfWidthFar, halfHeightFar, far);

        int[] triangles = new int[36];
        int t = 0;

        // near
        triangles[t++] = 0; triangles[t++] = 1; triangles[t++] = 2;
        triangles[t++] = 0; triangles[t++] = 2; triangles[t++] = 3;

        // far
        triangles[t++] = 4; triangles[t++] = 6; triangles[t++] = 5;
        triangles[t++] = 4; triangles[t++] = 7; triangles[t++] = 6;

        // gauche
        triangles[t++] = 0; triangles[t++] = 4; triangles[t++] = 5;
        triangles[t++] = 0; triangles[t++] = 5; triangles[t++] = 1;

        // droite
        triangles[t++] = 2; triangles[t++] = 6; triangles[t++] = 7;
        triangles[t++] = 2; triangles[t++] = 7; triangles[t++] = 3;

        // bas
        triangles[t++] = 1; triangles[t++] = 5; triangles[t++] = 6;
        triangles[t++] = 1; triangles[t++] = 6; triangles[t++] = 2;

        // haut
        triangles[t++] = 3; triangles[t++] = 7; triangles[t++] = 4;
        triangles[t++] = 3; triangles[t++] = 4; triangles[t++] = 0;

        frustumMesh.vertices = vertices;
        frustumMesh.triangles = triangles;
        frustumMesh.RecalculateNormals();

        frustumMeshFilter.sharedMesh = frustumMesh;
    }
}
