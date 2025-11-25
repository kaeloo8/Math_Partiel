using UnityEngine;

public class Skeleton_AnimationEvent : MonoBehaviour
{
    public void EnableMovementRelay()
    {
        GetComponentInParent<Skeleton_Movement>()?.EnableMovement();
    }
}