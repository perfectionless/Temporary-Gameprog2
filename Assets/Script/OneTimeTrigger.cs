using UnityEngine;

public class OneTimeTrigger : MonoBehaviour
{
    public GameObject targetObject; // The object to activate/deactivate

    private bool hasTriggered = false; // Ensures the trigger can only activate once

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true); // Activate the target object
            }

            hasTriggered = true; // Mark as triggered
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (hasTriggered)
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false); // Deactivate the target object
            }

            gameObject.SetActive(false); // Deactivate the trigger itself
        }
    }
}
