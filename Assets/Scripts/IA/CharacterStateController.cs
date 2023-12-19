using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    private CharacterState currentState;

    [SerializeField] private int positionOfCharacter;
    [SerializeField] private float speed;
    [SerializeField] private float timeWaiting;
    [SerializeField] private float rangeToSeePlayer;


    private GameObject[] gridArray;

    private void Start()
    {
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
}