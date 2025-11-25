using UnityEngine;
using UnityEngine.InputSystem;

public class LobbySpawner : MonoBehaviour
{
    public static LobbySpawner Instance;

    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    private int spawnIndex = 0;

    void Awake() => Instance = this;

    public void SpawnRealPlayer(PlayerInput selectorInput, CharacterType type)
    {
        if (type == CharacterType.None)
        {
            Debug.LogError("CharacterType.None ! Le joueur n'a pas encore choisi.");
            return;
        }

        GameObject prefab = playerPrefabs[(int)type];

        if (spawnIndex >= spawnPoints.Length)
        {
            Debug.LogError("Pas assez de spawn points !");
            return;
        }

        Transform spawn = spawnPoints[spawnIndex];

        GameObject player = Instantiate(prefab, spawn.position, spawn.rotation);

        PlayerInput input = player.GetComponent<PlayerInput>();

        input.SwitchCurrentControlScheme(
            selectorInput.currentControlScheme,
            selectorInput.devices.ToArray()
        );

        spawnIndex++;
    }
}
