using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    public GameObject playerSlotPrefab;
    public Transform slotContainer;

    private List<string> assignedDevices = new List<string>();
    private int maxPlayers = 4;

    private void Update()
    {
        // Manettes
        foreach (var pad in Gamepad.all)
        {
            if (pad.buttonSouth.wasPressedThisFrame)
                TryJoin("Gamepad_" + pad.deviceId, "Manette " + pad.deviceId);
        }

        // Clavier
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            TryJoin("KeyboardMouse", "Clavier / Souris");

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            TryJoin("KeyboardMouse", "Clavier / Souris");
    }

    private void TryJoin(string deviceKey, string deviceName)
    {
        if (assignedDevices.Count >= maxPlayers)
            return;

        if (assignedDevices.Contains(deviceKey))
            return;

        assignedDevices.Add(deviceKey);
        CreatePlayerSlot(deviceKey, deviceName);


    }

    private void CreatePlayerSlot(string deviceKey, string deviceName)
    {
        GameObject slot = Instantiate(playerSlotPrefab, slotContainer);

        var ui = slot.GetComponent<PlayerSlotUI>();

        ui.Init(deviceKey, deviceName);
    }

}
