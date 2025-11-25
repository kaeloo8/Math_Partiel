using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    private HashSet<CharacterType> takenCharacters = new HashSet<CharacterType>();

    private void Awake()
    {
        Instance = this;
    }

    public bool IsTaken(CharacterType c)
    {
        return takenCharacters.Contains(c);
    }

    public bool TrySelect(string deviceKey, CharacterType c)
    {
        if (takenCharacters.Contains(c))
            return false;

        takenCharacters.Add(c);
        GameData.playerCharacters[deviceKey] = c;

        return true;
    }

    public void Unselect(string deviceKey)
    {
        if (GameData.playerCharacters.ContainsKey(deviceKey))
        {
            CharacterType c = GameData.playerCharacters[deviceKey];
            takenCharacters.Remove(c);
            GameData.playerCharacters.Remove(deviceKey);
        }
    }
}
