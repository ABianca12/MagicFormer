using System;
using UnityEngine;

public class ThrowingVariables : MonoBehaviour
{
    public float ThrowingStrength = 100.0f;
    public float GrounderDistance = 0.05f;
    public float GroundingForce = -1.5f;

    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 80;

    public float FallAcceleration = 150;

    [Header("LAYERS")]
    [Tooltip("Wall Collision Layer")]
    public LayerMask collisionLayer;
}
