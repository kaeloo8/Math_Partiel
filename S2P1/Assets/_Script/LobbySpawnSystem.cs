using UnityEngine;
using UnityEngine.InputSystem;

public class LobbySpawnSystem : MonoBehaviour
{
    public Transform[] spawnPoints;
    private int spawnIndex = 0;

    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;
    }

    private void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= HandlePlayerJoined;
    }

    private void HandlePlayerJoined(PlayerInput player)
    {
        if (spawnIndex >= spawnPoints.Length)
        {
            Debug.LogWarning("Pas assez de spawn points !");
            return;
        }

        player.transform.position = spawnPoints[spawnIndex].position;

        Debug.Log($"Player join ! Device = {player.devices[0].displayName}");
        spawnIndex++;
    }
}
