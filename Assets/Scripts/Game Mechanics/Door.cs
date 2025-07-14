using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float OpenY = 7f;
    [SerializeField] private float ClosedY = 0f;

    [SerializeField] private float doorSpeed; 

    public bool IsOpen;
    private Vector3 targetPos; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.position; 
        targetPos.y = OpenY; // Set the target position to the open position
        transform.position = targetPos; // Initialize the door position to open

        IsOpen = true; // Initialize the door as closed
    }

    // Update is called once per frame
    void Update()
    {
        float desiredY = IsOpen ? OpenY : ClosedY; // Determine the desired Y position based on the door state
        if(Mathf.Abs(transform.position.y - desiredY) > 0.01f) // Check if the door needs to move
        {
            Vector3 newPos = transform.position; // Get the current position of the door
            newPos.y = Mathf.MoveTowards(transform.position.y, desiredY, doorSpeed * Time.deltaTime); // Move towards the desired Y position
            transform.position = newPos; // Update the door position
        }
        //if(IsOpen)
        //{
        //    if(transform.position != OpenPos)
        //    {
        //        if (Vector3.Distance(transform.position, OpenPos) < 0.5f)
        //        {
        //            // If the door is open, move it to the open position
        //            transform.position = OpenPos;
        //        }
        //        else
        //        {
        //            transform.position = Vector3.Lerp(transform.position, OpenPos, doorSpeed * Time.deltaTime);
        //        }
        //    }
        //}
        //else
        //{
        //    // If the door is closed, move it to the closed position
        //    if (transform.position != ClosedPos)
        //    {
        //        if (Vector3.Distance(transform.position, ClosedPos) < 0.5f)
        //        {
        //            // If the door is open, move it to the open position
        //            transform.position = ClosedPos;

        //        }
        //        else
        //        {
        //            transform.position = Vector3.Lerp(transform.position, ClosedPos, doorSpeed * Time.deltaTime);
        //        }
        //    }
        //}
    }
}
