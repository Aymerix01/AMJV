using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public float attackDmg;
    [HideInInspector] public float armor;
    [SerializeField] private float speedProjectile = 10f;

    private void Start()
    {
        Destroy(transform.gameObject, 3f);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.Translate((target.position+new Vector3(0, target.localScale.y,0)-transform.position).normalized* speedProjectile*Time.deltaTime, Space.World);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == target.tag)
        {
            other.gameObject.GetComponent<CharacterStateController>().pv -= attackDmg / armor;
            Destroy(transform.gameObject);
        }
    }
}
