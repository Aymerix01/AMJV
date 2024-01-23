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

    private int enemiesNbr;

    private bool waiting = false;
    private float time = 0f;

    void Update()
    {
        time += Time.deltaTime;
        DisplayTime(time);
        if (!waiting)
        {
            StartCoroutine(Wait());
            enemiesNbr = GameObject.FindGameObjectsWithTag("Enemy").Length;
            enemiesNbrTMP.text = enemiesNbr+"";
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
