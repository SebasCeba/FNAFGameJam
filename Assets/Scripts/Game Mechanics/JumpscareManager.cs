using UnityEngine;
using UnityEngine.SceneManagement;

public enum AnimatronicType { Shane, Alera, Scourage, Oscar, Amelia }
public class JumpscareManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject optionsPanel; // Panel with "Play Again" and "Menu" buttons

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Voice Lines")]
    public AnimatronicVoice ShaneVoice;
    public AnimatronicVoice AleraVoice;
    public AnimatronicVoice ScourageVoice;
    public AnimatronicVoice OscarVoice;
    public AnimatronicVoice AmeliaVoice;

    public void TriggerJumpscare(AnimatronicType type)
    {
        if(GameManager.instance != null)
            GameManager.instance.GameOver();

        optionsPanel.SetActive(false);

        // Play voice line 
        AnimatronicVoice voice = GetVoiceByType(type);
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
    private AnimatronicVoice GetVoiceByType(AnimatronicType type)
    {
        switch (type)
        {
            case AnimatronicType.Shane:
                return ShaneVoice;
            case AnimatronicType.Alera:
                return AleraVoice;
            case AnimatronicType.Scourage:
                return ScourageVoice;
            case AnimatronicType.Oscar:
                return OscarVoice;
            case AnimatronicType.Amelia:
                return AmeliaVoice;
            default:
                return null;
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for random selection, Higher = more common 
}
