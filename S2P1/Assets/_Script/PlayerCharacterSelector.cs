using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterSelector : MonoBehaviour
{
    public PlayerInput input; // attach in inspector
    public CharacterType selectedCharacter = CharacterType.None;

    public void SelectCharacter(CharacterType type)
    {
        selectedCharacter = type;

        LobbySpawner.Instance.SpawnRealPlayer(input, type);

        Destroy(gameObject);
    }
}
