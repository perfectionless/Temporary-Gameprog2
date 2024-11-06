using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject myHands; // Position for holding the object
    public bool canpickup; // Whether the player can pick up the item
    private GameObject ObjectIwantToPickUp; // The gameobject currently being looked at
    public bool hasItem; // Whether the player has an item
    public float interactionRange = 5f;
    public Camera playerCamera;
    public Material highlightMaterial; // Material for highlighting
    private Material originalMaterial; // Original material of the object

    public bool isLookingAtDoor; // Whether the player is looking at a door
    private bool isDoorOpen; // Whether the door is currently open
    private GameObject doorObject; // The door object the player is looking at
    public float doorOpenAngle = 90f; // The angle to open the door
    public float doorCloseAngle = 0f; // The angle to close the door
    public float doorSmooth = 2f; // Speed of door rotation

    void Update()
    {
        // Check if the player is looking at an object within range
        CheckForInteraction();

        if (canpickup && Input.GetKeyDown("f") && !hasItem) // Press 'f' to pick up
        {
            PickUp();
        }

        if (hasItem && Input.GetKeyDown("g")) // Press 'g' to drop the item
        {
            Drop();
        }

        if (isLookingAtDoor && Input.GetKeyDown(KeyCode.E)) // Press 'E' to open/close the door
        {
            ToggleDoor();
        }
    }

    // Checks if the player is looking at an object in pickup range
    void CheckForInteraction()
    {
        // Create a ray from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Draw the ray in the Scene view for visualization
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.green);
        if (Physics.Raycast(ray, out hit, interactionRange))
        {

            if (hit.collider.gameObject.CompareTag("PickUp"))
            {
                Debug.Log(hit.collider.gameObject.tag);
                HandlePickupObject(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Door"))
            {
                Debug.Log(hit.collider.gameObject.tag);
                HandleDoorObject(hit.collider.gameObject);
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }

    // Handles interaction with pickup objects
    void HandlePickupObject(GameObject pickupObject)
    {
        if (ObjectIwantToPickUp != pickupObject)
        {
            ResetHighlight();
            ObjectIwantToPickUp = pickupObject;
            originalMaterial = ObjectIwantToPickUp.GetComponent<Renderer>().material;
            ObjectIwantToPickUp.GetComponent<Renderer>().material = highlightMaterial;
        }

        canpickup = true;
        isLookingAtDoor = false; // Ensure we're not interacting with a door
    }

    // Handles interaction with door objects
    void HandleDoorObject(GameObject door)
    {
        ResetHighlight();

        isLookingAtDoor = true;
        doorObject = door;
        canpickup = false; // Ensure we're not interacting with a pickup item
    }

    // Resets the interaction state
    void ResetInteraction()
    {
        ResetHighlight();
        canpickup = false;
        ObjectIwantToPickUp = null;
        isLookingAtDoor = false;
        doorObject = null;
    }

    // Resets the material of the currently highlighted object
    void ResetHighlight()
    {
        if (ObjectIwantToPickUp != null && originalMaterial != null)
        {
            ObjectIwantToPickUp.GetComponent<Renderer>().material = originalMaterial;
        }
    }

    // Picks up the object
    void PickUp()
    {
        hasItem = true;
        ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true; // Disable physics on the object
        ObjectIwantToPickUp.transform.position = myHands.transform.position; // Move object to hands
        ObjectIwantToPickUp.transform.parent = myHands.transform; // Parent the object to hands
        ResetHighlight(); // Reset highlight after picking up
    }

    // Drops the object
    void Drop()
    {
        hasItem = false;
        ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // Enable physics on the object
        ObjectIwantToPickUp.transform.parent = null; // Unparent the object
        ObjectIwantToPickUp = null;
    }

    // Toggles the door open/close state
    void ToggleDoor()
    {
        isDoorOpen = !isDoorOpen; // Toggle door state
        float targetAngle = isDoorOpen ? doorOpenAngle : doorCloseAngle;
        StartCoroutine(RotateDoor(targetAngle));
    }

    // Coroutine to smoothly rotate the door
    IEnumerator RotateDoor(float targetAngle)
    {
        Quaternion initialRotation = doorObject.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0) * initialRotation;

        while (Quaternion.Angle(doorObject.transform.localRotation, targetRotation) > 0.1f)
        {
            doorObject.transform.localRotation = Quaternion.Slerp(doorObject.transform.localRotation, targetRotation, Time.deltaTime * doorSmooth);
            yield return null;
        }
        
        doorObject.transform.localRotation = targetRotation;
    }
}
