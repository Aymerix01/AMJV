using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    private CharacterStateController characterStateController;
    private int posEnemy;
    private List<GameObject> gameObjects;

    private void Start()
    {
        characterStateController = GetComponent<CharacterStateController>();
        gameObjects = new List<GameObject>();
        posEnemy = characterStateController.positionOfCharacter;
        for (int i = 1; i<transform.childCount; i++)
        {
            gameObjects.Add(transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        posEnemy = characterStateController.positionOfCharacter;
        if (characterStateController.gridArray[posEnemy].GetComponent<MeshRenderer>().isVisible)
        {
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(true);
            }
        } else
        {
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(false);
            }
        }
    }
}
