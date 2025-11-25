using UnityEngine;

public enum AbilityType { Melee, Projectile, Area }

[CreateAssetMenu(menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public AbilityType type;
    public string abilityName;
    public float damage;
    public float cooldown;
    public float range;

    public Transform Owner;
    public GameObject projectilePrefab;
    public GameObject fxPrefab;
    public GameObject heldItemPrefab;
    public float TimebeforestopHold;
    public AbilityBehavior behavior;
}


public abstract class AbilityBehavior : ScriptableObject
{
    public abstract void Execute(Ability ability, Transform user, Player_Stats stats);
}
