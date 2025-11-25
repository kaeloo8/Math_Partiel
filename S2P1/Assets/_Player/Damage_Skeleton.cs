using UnityEngine;

public class Damage_Skeleton : MonoBehaviour
{
    private Player_Stats stats;
    private Transform Owner;
    private Transform Item;

    public float Damage;
    public bool IsStunt;
    public float Stunt_Timer;
    public float Bump_Force;

    private void Start()
    {
        stats = GetComponentInParent<Player_Stats>();

        if (GetComponent<BoomerangProjectile>())
        {
            Owner = GetComponent<BoomerangProjectile>().GetOwner();
            Item = this.transform;
        }
        else
        {
            Owner = this.transform;
            Item = this.transform;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Skeleton_Movement>())
        {
            other.GetComponent<Skeleton_Movement>().Hit(Owner, Item, Damage, IsStunt, Stunt_Timer, Bump_Force);
        }
    }
}
