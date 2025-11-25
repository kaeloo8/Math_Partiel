using UnityEngine;
using UnityEngine.InputSystem;

public class Barbarian_Attack : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] InputActionReference attackInput;
    [SerializeField] Collider hitbox;

    private void OnEnable()
    {
        attackInput.action.Enable();
        attackInput.action.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        attackInput.action.Disable();
        attackInput.action.performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {

        anim.SetTrigger("BasicAttack");
        StartCoroutine(EnableHitboxTemporarily());
    }

    private System.Collections.IEnumerator EnableHitboxTemporarily()
    {
        hitbox.enabled = true;
        yield return new WaitForSeconds(0.3f);
        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitbox.enabled)
        {
            Debug.Log("Touché : " + other.name);
        }
    }

}
