using System;
using UnityEngine;

public class ThrowingVariables : MonoBehaviour
{
    public float PickUpGrounderDistance = 0.05f;
    public float GroundingForce = -1.5f;
    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 80;
    public float FallAcceleration = 150;

    [Tooltip("The pace at which the throwable comes to a stop")]
    public float GroundDeceleration = 60;

    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 30;

    public float ThrowingStrength = 100.0f;
    public float GrounderDistance = 0.05f;

    [Tooltip("The time in seconds until the key will return to it's origonal position after being thrown")]
    public float keyResetTime = 10.0f;

    [Header("LAYERS")]
    //[Tooltip("Wall Collision Layer")]
    public LayerMask defaultLayer;
    public LayerMask playerLayer;
    public LayerMask pickUpLayer;
}
