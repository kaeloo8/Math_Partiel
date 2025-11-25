using System.Collections;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.Port;

public class Ability_Use : MonoBehaviour
{
    public Ability[] abilities;
    private float[] cooldowns;
    private Player_Stats playerstats;
    [SerializeField] Animator animator;
    [SerializeField] GameObject defaultitem;
    [SerializeField] Transform herohand;
    GameObject currentItemInstance;

    [Header("UI")]
    [SerializeField] Sprite specialattack;
    [SerializeField] Sprite capacity;
    [SerializeField] Sprite Ultime;

    [Header("INPUT")]
    [SerializeField] InputActionReference specialInput;
    [SerializeField] InputActionReference capacityInput;
    [SerializeField] InputActionReference ultimateInput;

    void OnEnable()
    {
        specialInput.action.Enable();
        capacityInput.action.Enable();
        ultimateInput.action.Enable();

        specialInput.action.performed += OnSpecial;
        capacityInput.action.performed += OnCapacity;
        ultimateInput.action.performed += OnUltimate;
    }

    void OnDisable()
    {
        specialInput.action.Disable();
        capacityInput.action.Disable();
        ultimateInput.action.Disable();

        specialInput.action.performed -= OnSpecial;
        capacityInput.action.performed -= OnCapacity;
        ultimateInput.action.performed -= OnUltimate;
    }

    void Start()
    {
        cooldowns = new float[abilities.Length];
        EquipItem(defaultitem);
        playerstats = GetComponentInParent<Player_Stats>();
    }

    void Update()
    {
        for (int i = 0; i < cooldowns.Length; i++)
            cooldowns[i] -= Time.deltaTime;
    }

    void OnSpecial(InputAction.CallbackContext ctx)
    {
        TryUse(0);
        animator.SetTrigger("SpecialAttack");
    }
    void OnCapacity(InputAction.CallbackContext ctx)
    {
        TryUse(1);
        animator.SetTrigger("Capacity");
    }
    void OnUltimate(InputAction.CallbackContext ctx)
    {
        TryUse(2);
    }

    void TryUse(int index)
    {
        if (cooldowns[index] > 0) return;

        Ability a = abilities[index];

        if (a.heldItemPrefab)
            EquipItemTemporary(a.heldItemPrefab, a.TimebeforestopHold);

        a.behavior.Execute(a, transform, playerstats);
        cooldowns[index] = a.cooldown;
    }
    void EquipItem(GameObject prefab)
    {
        for (int i = herohand.childCount - 1; i >= 0; i--)
            Destroy(herohand.GetChild(i).gameObject);

        GameObject proj = Instantiate(
            prefab,
            herohand.position,
            herohand.rotation
        );

        proj.transform.SetParent(herohand);
        proj.transform.localPosition = Vector3.zero;
        proj.transform.localRotation = Quaternion.identity;
        proj.transform.localScale = Vector3.one;

    }
    void EquipItemTemporary(GameObject prefab, float timer)
    {
        for (int i = herohand.childCount - 1; i >= 0; i--)
            Destroy(herohand.GetChild(i).gameObject);

        GameObject proj = Instantiate(
            prefab,
            herohand.position,
            herohand.rotation
        );

        proj.transform.SetParent(herohand);
        proj.transform.localPosition = Vector3.zero;
        proj.transform.localRotation = Quaternion.identity;
        proj.transform.localScale = Vector3.one;

        StartCoroutine(ReEquipDefaultAfter(timer));
    }
    IEnumerator ReEquipDefaultAfter(float time)
    {
        yield return new WaitForSeconds(time);
        EquipItem(defaultitem);
    }
}
