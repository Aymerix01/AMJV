using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    protected Transform transform;
    public int positionOfCharacter;
    protected float speed;
    protected float timeWaiting;
    protected float rangeToSeePlayer;
    protected GameObject[] gridArray;

    public virtual CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        transform = characterT;
        positionOfCharacter = posCharacter;
        speed = s;
        timeWaiting = t;
        gridArray = g;
        rangeToSeePlayer = r;
        return this;
    }
    
    public virtual CharacterState UpdateState()
    {
        return this;
    }
    public virtual CharacterState Exit(CharacterState newState)
    {
        newState = newState.Enter(transform, positionOfCharacter, speed, timeWaiting, rangeToSeePlayer, gridArray);
        return newState;
    }

    protected bool SeePlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < rangeToSeePlayer)
            {
                return true;
            }
        }
        return false;
    }

    protected bool IsIAarrivedEtape(int etape, GameObject[] path)
    {
        if (transform.position == new Vector3(path[etape].transform.position.x, 0.34f + path[etape].transform.position.y, path[etape].transform.position.z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected GameObject GetPlayerTransform()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestPlayer = rangeToSeePlayer;
        GameObject target = null;
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < closestPlayer)
            {
                closestPlayer = Vector3.Distance(player.transform.position, transform.position);
                target = player;
            }
        }
        return target;
    }

    protected int ChooseDestinationRandom()
    {
        int dest = Random.Range(0, gridArray.Length - 1);
        while (gridArray[dest] == null || gridArray[dest].GetComponent<GridStat>().hasEntityOnIt ||
            gridArray[dest].GetComponent<GridStat>().isDestinationForEntity ||
            dest == positionOfCharacter)
        {
            dest = Random.Range(0, gridArray.Length - 1);
        }
        return dest;
    }
}