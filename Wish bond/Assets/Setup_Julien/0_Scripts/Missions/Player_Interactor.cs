using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Tout objet avec un collider + un script qui implémente I_Interactable sera détecté,
// et le joueur pourra interagir avec la touche choisie.
public class PlayerInteractZone : MonoBehaviour
{
    [Header("Touche d'interaction (New Input System)")]
    [SerializeField] private Key interactKey = Key.E;

    // Liste de tous les objets interactifs actuellement dans la zone.
    private readonly List<I_Interactable> _interactablesInRange = new List<I_Interactable>();

    // Objet interactif "courant"
    private I_Interactable _currentTarget;

    private void OnTriggerEnter(Collider other)
    {
        // On cherche un script qui implémente I_Interactable sur l'objet ou son parent.
        I_Interactable interactable = other.GetComponentInParent<I_Interactable>();
        if (interactable == null)
            return;

        if (!_interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Add(interactable);
        }

        _currentTarget = interactable;

        Debug.Log("[PlayerInteractZone] Entré dans la zone d'un objet interactif : " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        I_Interactable interactable = other.GetComponentInParent<I_Interactable>();
        if (interactable == null)
            return;

        if (_interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Remove(interactable);
            Debug.Log("[PlayerInteractZone] Sorti de la zone d'un objet interactif : " + other.gameObject.name);
        }

        // Si on quitte l'objet courant, on choisit un autre dans la liste (ou rien).
        if (_currentTarget == interactable)
        {
            _currentTarget = _interactablesInRange.Count > 0
                ? _interactablesInRange[_interactablesInRange.Count - 1]
                : null;
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // Quand on appuie sur la touche d'interaction
        if (Keyboard.current[interactKey].wasPressedThisFrame)
        {
            if (_currentTarget != null)
            {
                Debug.Log("[PlayerInteractZone] Interaction avec l'objet courant.");
                _currentTarget.Interact();
            }
            else
            {
                Debug.Log("[PlayerInteractZone] Aucun objet interactif dans la zone.");
            }
        }
    }
}
