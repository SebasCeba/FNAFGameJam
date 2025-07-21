using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject spotLight; 
    [SerializeField] private float OpenY = 7f;
    [SerializeField] private float ClosedY = 0f;

    [SerializeField] private float doorSpeed; 

    public bool IsOpen;
    public bool IsOn; 
    private Vector3 targetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.localPosition; 
        targetPos.y = OpenY; // Set the target position to the open position
        transform.localPosition = targetPos; // Initialize the door position to open

        IsOpen = true; // Initialize the door as closed
    }

    // Update is called once per frame
    void Update()
    {
        float desiredY = IsOpen ? OpenY : ClosedY; // Determine the desired Y position based on the door state
        if (Mathf.Abs(transform.localPosition.y - desiredY) > 0.01f) // Check if the door needs to move
        {
            Vector3 newPos = transform.localPosition; // Get the current position of the door
            newPos.y = Mathf.MoveTowards(transform.localPosition.y, desiredY, doorSpeed * Time.deltaTime); // Move towards the desired Y position
            transform.localPosition = newPos; // Update the door position
        }
    }

    public void ChangeLight()
    {
        IsOn = !IsOn; // Toggle the light state

        if (IsOn)
        {
            spotLight.SetActive(true); // Turn on the light
        }
        else
        {
            spotLight.SetActive(false); // Turn off the light
        }
    }
}
