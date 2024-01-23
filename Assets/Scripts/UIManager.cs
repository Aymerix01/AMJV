using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeTMP;
    [SerializeField]
    private TMP_Text enemiesNbrTMP;

    [HideInInspector] 
    public bool victoire;

    [SerializeField] private GameObject victoryScreen;

    private int enemiesNbr;

    private bool waiting = false;
    private float time = 0f;

    private void Start()
    {
        victoire = false;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (!victoire)
        {
            DisplayTime(time);
        }
        if (!waiting)
        {
            StartCoroutine(Wait());
            enemiesNbr = GameObject.FindGameObjectsWithTag("Enemy").Length;
            enemiesNbrTMP.text = enemiesNbr + "";
            if (enemiesNbr <= 0 && victoryScreen != null)
            {
                Time.timeScale = 0f;
                victoryScreen.SetActive(true);
            }
        }
        
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(1);
        waiting = false;
    }
}
