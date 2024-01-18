using UnityEngine;
using UnityEngine.UI;

public class CharacterStateController : MonoBehaviour
{
    private CharacterState currentState;

    public int positionOfCharacter;
    public float health;
    [SerializeField] private float speed;
    public float attackDmg;
    public float armor;
    [SerializeField] private float timeWaiting;
    [SerializeField] private float rangeToSeeOpponent;
    [SerializeField] private float rangeToAttackOpponent;
    [HideInInspector] public GridStat[] gridArray;

    public bool selected;

    [HideInInspector] public float pv;

    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject projectile;
    public Image FlagImg;

    public bool possessFlag;

    public CharacterStateController opponentToAttack;

    public int rangeOfAction;

    private void Start()
    {
        pv = health;
        selected = false;
        gridArray = GridHex.GetGrid(transform);
        currentState = new Idle();
        currentState = currentState.Enter(transform, positionOfCharacter, speed, timeWaiting, rangeToSeeOpponent, rangeToAttackOpponent, gridArray);
        if (!possessFlag)
        {
            FlagImg.gameObject.SetActive(false);
        }
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
        if (opponentToAttack == null)
        {
            CharacterStateController playerTarget = currentState.GetPlayerTransform();
            if (rangeToAttackOpponent <= 1.5 && playerTarget != null)
            {
                playerTarget.pv -= attackDmg / playerTarget.armor;
            }
            else
            {
                if (projectile != null && playerTarget != null)
                {
                    Projectile p = Instantiate(projectile).GetComponent<Projectile>();
                    p.transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
                    p.target = playerTarget.transform;
                    p.attackDmg = attackDmg;
                    p.armor = playerTarget.armor;
                }
            }
        }
        else
        {
            if (rangeToAttackOpponent <= 1.5)
            {
                opponentToAttack.pv -= attackDmg / opponentToAttack.armor;
            }
            else
            {
                if (projectile != null)
                {
                    Projectile p = Instantiate(projectile).GetComponent<Projectile>();
                    p.transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
                    p.target = opponentToAttack.transform;
                    p.attackDmg = attackDmg;
                    p.armor = opponentToAttack.armor;
                }
            }
        }
    }
}
