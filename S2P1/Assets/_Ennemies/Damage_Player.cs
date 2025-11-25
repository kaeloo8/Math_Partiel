using UnityEngine;

public class Damage_Player : MonoBehaviour
{
    private Skeleton_Stats stats;

    private void Start()
    {
        stats = GetComponentInParent<Skeleton_Stats>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player_Movement>())
        {
            other.GetComponent<Player_Movement>().Hit(stats.damage);
        }
    }
}
