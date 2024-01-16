using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MageSpecial : MonoBehaviour
{
    [SerializeField] private float timeSpeAttack;
    [SerializeField] private float radiusSpeAttack;
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;

    private List<GameObject> playersDamaged;
    private CharacterStateController characterStateController;
    private bool waiting;
    private bool selected;
    

    private void Start()
    {
        waiting = false;
        playersDamaged= new List<GameObject>();
        characterStateController = GetComponent<CharacterStateController>();
    }
    private void Update()
    {
        if (selected || (!waiting && Input.GetKeyDown(KeyCode.R) && characterStateController.selected))
        {
            selected = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPlat;

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                selected = false;
            }

            if (Physics.Raycast(ray, out hitPlat))
            {
                if (hitPlat.collider.gameObject.layer == 6)
                {
                    Vector3 aiming = hitPlat.transform.position;
                    Debug.Log(aiming);
                    target.SetActive(true);
                    aiming.y += 0.4f;
                    target.transform.position = aiming;


                    if (Vector3.Distance(transform.position, aiming) <= maxDistance)
                    {
                        target.GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                    else
                    {
                        target.GetComponent<SpriteRenderer>().color = Color.red;
                    }


                    if (Input.GetKeyDown(KeyCode.Mouse0) && Vector3.Distance(transform.position, aiming) <= maxDistance)
                    {
                        Debug.Log("Over here");
                        StartCoroutine(WaitAttack());
                        RaycastHit[] hits;
                        hits = Physics.SphereCastAll(aiming,
                                                radiusSpeAttack,
                                                transform.forward,
                                                radiusSpeAttack);
                        foreach (RaycastHit hit in hits)
                        {
                            GameObject obj = hit.collider.gameObject;
                            if (obj.CompareTag("Enemy"))
                            {
                                playersDamaged.Add(obj);
                                obj.GetComponent<CharacterStateController>().pv -= damage;
                            }
                        }
                        selected = false;
                        target.SetActive(false);
                    }
                }
            }
        }
    }
    private IEnumerator WaitAttack()
    {
        waiting = true;
        yield return new WaitForSeconds(timeSpeAttack);
        waiting = false;

    }
}
