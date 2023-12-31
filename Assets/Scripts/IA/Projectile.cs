using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Transform player;
    [HideInInspector] public float attackDmg;
    [SerializeField] private float speedProjectile = 10f;

    private void Start()
    {
        Destroy(transform.gameObject, 3f);
    }

    private void Update()
    {
        if (player != null)
        {
            transform.Translate((player.position+new Vector3(0, player.localScale.y,0)-transform.position).normalized* speedProjectile*Time.deltaTime, Space.World);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterStateController>().pv -= attackDmg;
            Destroy(transform.gameObject);
        }
    }
}
