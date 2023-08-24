using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For finding the player easier
public abstract class Player : MonoBehaviour
{
    public abstract void ResetVelocity();
    public abstract void ResetMove();
}
