using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class Troop : MonoBehaviour
{
    [SerializeField] protected int speed;
    [SerializeField] protected int hp;
    [SerializeField] protected int armour;
    [SerializeField] protected int attSpeed;
    [SerializeField] protected int coolDown;
    public abstract void SpecialMoove();
    public abstract void Attak();
    public abstract void TakeDamage();
}
