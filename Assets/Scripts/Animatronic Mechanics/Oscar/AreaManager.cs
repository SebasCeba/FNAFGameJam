using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public Oscar oscar;
    public Collider areaCollider;
    [Tooltip("If true, Oscar will crouch in t his area.")]
    public bool isCrouchArea = false; // Set in inspector for each area

    [Tooltip("If true, Oscar will look towards the player when entering this area.")]
    public Vector3 lookEulerAngles = Vector3.zero; // Set in inspector for each area 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oscar"))
        {
            oscar.currentArea = this; 
            oscar.SetLookDirection(lookEulerAngles);
        }
    }
    public void OnShockButttonPress()
    {
        oscar.TryShock(this); 
    }
}
