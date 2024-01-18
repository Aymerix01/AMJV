using System.Collections.Generic;
using UnityEngine;

public class Selected : CharacterState
{
    private List<GameObject> unitsSelected;
    private CharacterStateController characterStateController;
    private Camera camera;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GridStat[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        characterStateController = transform.GetComponent<CharacterStateController>();
        camera = Camera.main;
        characterStateController.selected = true;
        unitsSelected = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>().unitsSelected;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (characterStateController.pv <= 0)
        {
            return Exit(new Death());
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int layer = hit.collider.gameObject.layer;
                if (layer == 6 && !hit.collider.CompareTag("Hole") && !hit.collider.gameObject.GetComponent<GridStat>().hasEntityOnIt)
                {
                    return Exit(new Walk());
                }
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    return Exit(new Follow());
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
