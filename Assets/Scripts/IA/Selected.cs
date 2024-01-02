using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selected : CharacterState
{
    private List<GameObject> unitsSelected;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        unitsSelected = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>().unitsSelected;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (transform.gameObject.GetComponent<CharacterStateController>().pv <= 0)
        {
            return Exit(new Death());
        }
        if (SeeEnemy())
        {
            GameObject[] enemiesSeen = GetEnemiesGameObject();
            foreach (GameObject enemy in enemiesSeen)
            {
                int childCount = enemy.transform.childCount;
                for (int i = 1; i < childCount; i++)
                {
                    enemy.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                int layer = hit.collider.gameObject.layer;
                if (layer == 6)
                {
                    return Exit(new Walk());
                }
            }
        }
        if (transform.gameObject.layer == 7 && !unitsSelected.Contains(transform.gameObject))
        {
            return Exit(new Idle());
        }
        return this;
    }
}
