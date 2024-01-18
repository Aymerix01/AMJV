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
    protected GridStat[] gridArray;


    private float rangeToSeeOpponent;
    private GameObject[] players;
    private GameObject[] Enemies;
    private CharacterStateController characterStateController;

    public virtual CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GridStat[] g)
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
        players = GameObject.FindGameObjectsWithTag("Player");
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        characterStateController = transform.GetComponent<CharacterStateController>();
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
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < rangeToSeePlayer)
            {
                return true;
            }
        }
        return false;
    }
    protected bool IsIAarrivedEtape(int etape, GridStat[] path)
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
    public CharacterStateController GetPlayerTransform()
    {
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
        return target.GetComponent<CharacterStateController>();
    }
    /*public GameObject[] GetEnemiesGameObject()
    {
        List<GameObject> enemiesSeen = new List<GameObject>();
        foreach (GameObject enemy in Enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < rangeToSeeEnemy)
            {
                enemiesSeen.Add(enemy);
            }
        }
        return enemiesSeen.ToArray();
    }*/
    protected int ChooseDestinationRandom()
    {
        int rangeOfAction = characterStateController.rangeOfAction;
        if(rangeOfAction == 0)
        {
            int dest = Random.Range(0, gridArray.Length - 1);
            while (gridArray[dest] == null || gridArray[dest].hasEntityOnIt ||
                gridArray[dest].isDestinationForEntity ||
                dest == positionOfCharacter)
            {
                dest = Random.Range(0, gridArray.Length - 1);
            }
            return dest;
        }
        else
        {
            int dest = Random.Range(0, rangeOfAction);
            while (gridArray[dest] == null || gridArray[dest].hasEntityOnIt ||
                gridArray[dest].isDestinationForEntity ||
                dest == positionOfCharacter)
            {
                dest = Random.Range(0, rangeOfAction);
            }
            return dest;
        }
        
    }
}