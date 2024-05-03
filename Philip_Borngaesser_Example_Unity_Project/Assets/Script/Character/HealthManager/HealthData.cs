using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HealthData" ,fileName = "HealthData")]
public class HealthData : ScriptableObject
{
    public int health = 100;
    public TeamFlag team;
}
