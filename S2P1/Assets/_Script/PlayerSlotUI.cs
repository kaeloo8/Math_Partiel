using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSlotUI : MonoBehaviour
{
    public TMP_Text deviceNameText;
    public Button[] characterButtons;
    public Image deviceIcon;
    public Sprite iconGamepad;
    public Sprite iconKeyboardMouse;

    private string deviceKey;

    public void Init(string key, string deviceName)
    {
        deviceKey = key;
        deviceNameText.text = deviceName;
        
        if (key.StartsWith("Gamepad"))
            deviceIcon.sprite = iconGamepad;
        else
            deviceIcon.sprite = iconKeyboardMouse;

        RefreshButtons();
    }


    public void OnChooseCharacter(int index)
    {
        CharacterType selected = (CharacterType)index;

        bool success = CharacterSelectionManager.Instance.TrySelect(deviceKey, selected);

        if (success)
        {
            RefreshAllSlots();
        }
        else
        {
            Debug.Log("Personnage d�j� pris !");
        }
    }

    public void RefreshButtons()
    {
        return;
        for (int i = 0; i < characterButtons.Length; i++)
        {
            CharacterType c = (CharacterType)i;
            characterButtons[i].interactable = !CharacterSelectionManager.Instance.IsTaken(c);
        }
    }

    public static void RefreshAllSlots()
    {
        foreach (var slot in FindObjectsOfType<PlayerSlotUI>())
            slot.RefreshButtons();
    }
}
