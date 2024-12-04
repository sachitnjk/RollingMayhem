using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tank Base Type", menuName = "TestTank")]
public class TankBaseSO : ScriptableObject
{
    public float maxSpeed;
    public float acceleration;
    public float decceleration;
}
