using UnityEngine;

public class Amelia : MonoBehaviour
{
    public Material[] moodMaterials; // If any case the modeler doesn't include the materials/variants for happy, warning and danger moods.
    public SkinnedMeshRenderer meshRenderer; // Reference to the SkinnedMeshRenderer component
    public Animator animator; // Reference to the Animator component
    public float meterMax = 100f; // Maximum value of the meter
    public float meterValue = 100f; // Current value of the meter
    public float meterDecayRate = 5f; // Rate at which the meter decays per second

    private void Update()
    {
        meterValue -= meterDecayRate * Time.deltaTime; // Decrease the meter value over time
        meterValue = Mathf.Clamp(meterValue, 0, meterMax); // Clamp the meter value between 0 and max
        UpdateMood();

        // Once they gave the modeler the materials, variants and animations, we can implement the mood changes based on meterValue.
        if (meterValue > meterMax * 0.5f)
            animator.Play("IdleHappy");
        else if(meterValue > meterMax * 0.2f)
            animator.Play("IdleWarning");
        else
            animator.Play("IdleDanger");
    }
    void UpdateMood()
    {
        if(meterValue > meterMax * 0.5f)
            meshRenderer.material = moodMaterials[0]; // Happy
        else if (meterValue > meterMax * 0.2f)
            meshRenderer.material = moodMaterials[1]; // Warning
        else
            meshRenderer.material = moodMaterials[2]; // Danger
    }
}
