using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CharacterState
{
    protected Transform transform;
    public int positionOfCharacter;
    protected float speed;
    protected float timeWaiting;
    protected float rangeToSeePlayer;
    protected float rangeToSeeEnemy;
    protected float rangeToAttackPlayer;
    protected GameObject[] gridArray;


    private float rangeToSeeOpponent;
    

    public virtual CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {
        transform = characterT;
        positionOfCharacter = posCharacter;
        speed = s;
        timeWaiting = t;
        gridArray = g;
        rangeToSeeOpponent = r;
        if (transform.gameObject.CompareTag("Player"))
        {
            rangeToSeePlayer = 0;
            rangeToSeeEnemy = r;
        } else if (transform.gameObject.CompareTag("Enemy"))
        {
            rangeToSeePlayer = r;
            rangeToSeeEnemy = 0;
        }
        rangeToAttackPlayer = ra;
        return this;
    }
    
    public virtual CharacterState UpdateState()
    {
        return this;
    }
    public virtual CharacterState Exit(CharacterState newState)
    {
        newState = newState.Enter(transform, positionOfCharacter, speed, timeWaiting, rangeToSeeOpponent, rangeToAttackPlayer, gridArray);
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
    public GameObject GetPlayerTransform()
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

    public GameObject[] GetEnemiesGameObject()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesSeen = new List<GameObject>();
        foreach (GameObject enemy in Enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < rangeToSeeEnemy)
            {
                enemiesSeen.Add(enemy);
            }
        }
        return enemiesSeen.ToArray();
    }

    protected int ChooseDestinationRandom()
    {
        int rangeOfAction = transform.gameObject.GetComponent<CharacterStateController>().rangeOfAction;
        if(rangeOfAction == 0)
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
        else
        {
            int dest = Random.Range(0, rangeOfAction);
            while (gridArray[dest] == null || gridArray[dest].GetComponent<GridStat>().hasEntityOnIt ||
                gridArray[dest].GetComponent<GridStat>().isDestinationForEntity ||
                dest == positionOfCharacter)
            {
                dest = Random.Range(0, rangeOfAction);
            }
            Debug.Log(dest);
            return dest;
        }
        
    }
}