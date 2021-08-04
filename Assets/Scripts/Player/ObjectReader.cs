using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReader : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject target;
    private bool is_interacting;

    public bool Interacting
    {
        get { return is_interacting; }
    }

    public bool CanInteract
    {
        get { return (target != null); }
    }

    private void Update()
    {
        if (target != null && !is_interacting)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                IInteractable interactable = target.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    is_interacting = true;
                    interactable.Interact(player, Interactable_EndEvent);
                }
            }
        }
    }

    public void SetInteracting(bool value)
    {
        is_interacting = value;
    }

    private void Interactable_EndEvent()
    {
        is_interacting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable") && target == null)
        {
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            target = null;
        }
    }
}
