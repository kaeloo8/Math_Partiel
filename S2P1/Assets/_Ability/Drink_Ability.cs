using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviors/Drink")]
public class Drink_Ability : AbilityBehavior
{
    [SerializeField] int DamageBoost;
    public override void Execute(Ability ability, Transform user, Player_Stats stats)
    {
        stats.Level += DamageBoost;
    }
}
