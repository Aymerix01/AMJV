using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selected : CharacterState
{
    private UnitSelections unitSelections;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        unitSelections = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>();
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (Input.GetMouseButtonDown(0))
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
        if (transform.gameObject.layer == 7 && !unitSelections.unitsSelected.Contains(transform.gameObject))
        {
            return Exit(new Idle());
        }
        return this;
    }
}
