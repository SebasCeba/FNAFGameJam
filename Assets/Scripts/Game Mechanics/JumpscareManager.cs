using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Search;

public class JumpscareManager : MonoBehaviour
{
    [Header("UI")]
    public Image blackoutImage; // Fullscreen black image for fade effect
    public GameObject optionsPanel; // Panel with "Play Again" and "Menu" buttons

    [Header("Audio")]
    public AudioSource audioSource;
    public AnimatronicVoice[] animaTronicVoices; // Assign in inspector 

    public void TriggerJumpscare(string animatronicName)
    {
        // Blacken screen 
        blackoutImage.color = new Color(0, 0, 0, 1);
        optionsPanel.SetActive(false);

        // Play voice line 
        var voice =System.Array.Find(animaTronicVoices, v => v.name == animatronicName);
        if(voice != null && voice.voiceLines.Length > 0)
        {
            AudioClip clip = GetWeightedVoiceLine(voice);
            audioSource.clip = clip;
            audioSource.Play();
            Invoke(nameof(ShowOptions), clip.length); // Show options after clip ends
        }
        else
        {
            ShowOptions(); 
        }
    }
    AudioClip GetWeightedVoiceLine(AnimatronicVoice voice)
    {
        float totalWeight = 0f;
        foreach (var v in voice.voiceLineWeights)
        {
            totalWeight += v;
        }
        float randomValue = Random.Range(0, totalWeight);
        float accum = 0f; 
        for (int i = 0; i < voice.voiceLines.Length; i++)
        {
            accum += voice.voiceLineWeights[i];
            if (randomValue <= accum)
            {
                return voice.voiceLines[i];
            }
        }
        return voice.voiceLines[0]; // Fallback 
    }
    void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
[System.Serializable]
public class AnimatronicVoice
{
    public string name;
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for random selection, Higher = more common 
}
