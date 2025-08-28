using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public Oscar oscar;
    public Collider areaCollider;
    public bool lookLeft = true; // Set in the inspector for each area 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oscar"))
        {
            oscar.currentArea = this; 
            oscar.SetLookDirection(lookLeft);
        }
    }
    public void OnShockButttonPress()
    {
        oscar.TryShock(this); 
    }
}
