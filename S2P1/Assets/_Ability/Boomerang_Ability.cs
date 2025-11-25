using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviors/Boomerang")]
public class Boomerang_Ability : AbilityBehavior
{
    public float travelDistance = 10f;
    public float speed = 10f;
    public float spinSpeed = 360f;

    public Vector3 spinAxis = Vector3.forward;
    public override void Execute(Ability ability, Transform user, Player_Stats stats)
    {

        if (!ability.projectilePrefab) return;

        GameObject proj = Instantiate(
            ability.projectilePrefab,
            user.position + new Vector3(0,1,0) + user.forward * 1f,
            user.rotation
        );


        BoomerangProjectile bp = proj.GetComponent<BoomerangProjectile>();
        if (bp)
        {
            bp.Init(user, speed, spinSpeed, travelDistance, spinAxis);
        }
    }
}
