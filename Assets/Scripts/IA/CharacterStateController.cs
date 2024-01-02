using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateController : MonoBehaviour
{
    private CharacterState currentState;

    public int positionOfCharacter;
    public float health;
    [SerializeField] private float speed;
    [SerializeField] private float attackDmg;
    [SerializeField] private float timeWaiting;
    [SerializeField] private float rangeToSeeOpponent;
    [SerializeField] private float rangeToAttackPlayer;
    private GameObject[] gridArray;

    [HideInInspector] public float pv;

    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject projectile;

    private void Start()
    {
        pv = health;
        gridArray = GridHex.GetGrid(transform);
        currentState = new Idle();
        currentState = currentState.Enter(transform, positionOfCharacter, speed, timeWaiting, rangeToSeeOpponent, rangeToAttackPlayer, gridArray);
    }

    private void Update()
    {
        currentState = currentState.UpdateState();
        UpdatePositionOfCharacter();
        healthBar.fillAmount = pv / health;
    }

    private void UpdatePositionOfCharacter()
    {
        positionOfCharacter = currentState.positionOfCharacter;
    }

    public void GetAttacked()
    {
        GameObject playerTarget = currentState.GetPlayerTransform();
        if (rangeToAttackPlayer == 0)
        {
            playerTarget.GetComponent<CharacterStateController>().pv -= attackDmg;
        } else
        {
            if (projectile != null)
            {
                GameObject p = Instantiate(projectile);
                p.transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
                p.GetComponent<Projectile>().player = playerTarget.transform;
                p.GetComponent<Projectile>().attackDmg = attackDmg;
            }
        }
    }
}
