using UnityEngine;

public class AfterimageSpawner : MonoBehaviour
{
    [Header("Afterimage")]
    public GameObject afterimagePrefab;
    public float spawnInterval = 0.05f;
    public float lifetime = 0.3f;

    private bool spawning = false;

    public void StartSpawning() => spawning = true;
    public void StopSpawning() => spawning = false;

    private float timer = 0f;

    void Update()
    {
        if (!spawning) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnAfterimage();
            timer = 0f;
        }
    }

    void SpawnAfterimage()
    {
        GameObject go = Instantiate(afterimagePrefab, transform.position, transform.rotation);
        go.transform.localScale = transform.localScale;
        Destroy(go, lifetime);
    }
}
