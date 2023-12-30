using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateController : MonoBehaviour
{
    private CharacterState currentState;

    public int positionOfCharacter;
    public float health;
    [SerializeField] private float speed;
    [SerializeField] private float timeWaiting;
    [SerializeField] private float rangeToSeePlayer;
    private GameObject[] gridArray;

    [HideInInspector] public float pv;

    public Image healthBar;

    private void Start()
    {
        pv = health;
        gridArray = GridHex.GetGrid(transform);
        currentState = new Idle();
        currentState = currentState.Enter(transform, positionOfCharacter, speed, timeWaiting, rangeToSeePlayer, gridArray);
    }

    private void Update()
    {
        currentState = currentState.UpdateState();
        UpdatePositionOfCharacter();
    }

    private void UpdatePositionOfCharacter()
    {
        positionOfCharacter = currentState.positionOfCharacter;
    }

    public void GetAttacked(float damage)
    {
        pv -= damage;
        if (healthBar != null)
            healthBar.fillAmount = pv / health;
    }
}
