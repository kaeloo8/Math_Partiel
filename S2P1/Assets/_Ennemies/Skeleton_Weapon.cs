using UnityEngine;

public class Skeleton_Weapon : MonoBehaviour
{
    [SerializeField] GameObject Weapon;
    [SerializeField] Transform Hand;
    [SerializeField] BoxCollider Hitbox;
    private float damage;

    private void Start()
    {
        damage = GetComponentInParent<Skeleton_Stats>().damage;
        EquipItem(Weapon);

        Hitbox.enabled = false;
    }

    void EquipItem(GameObject prefab)
    {
        for (int i = Hand.childCount - 1; i >= 0; i--)
            Destroy(Hand.GetChild(i).gameObject);

        GameObject proj = Instantiate(
            prefab,
            Hand.position,
            Hand.rotation
        );

        proj.transform.SetParent(Hand);
        proj.transform.localPosition = Vector3.zero;
        proj.transform.localRotation = Quaternion.identity;
        proj.transform.localScale = Vector3.one;
    }

    public void Attack()
    {
        Hitbox.enabled = true;

        StartCoroutine(DisableHitboxAfterDelay(0.1f));
    }

    private System.Collections.IEnumerator DisableHitboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hitbox.enabled = false;
    }

}