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
    public Alera alera;
    public Amelia amelia;
    public Oscar oscar;
    public Scourage scourage;
    public Shane shane;

    public void TriggerJumpscare(AnimatronicType type)
    {
        if(GameManager.instance != null)
            GameManager.instance.GameOver();

        optionsPanel.SetActive(false);
        ShowOptions();

        // Play voice line 
        AudioClip clip = GetVoiceByType(type);
        if(clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            //Invoke(nameof(ShowOptions), clip.length); // Show options after clip ends
        }
        else
        {
            ShowOptions();
        }
    }
    private AudioClip GetVoiceByType(AnimatronicType type)
    {
        switch (type)
        {
            case AnimatronicType.Alera: return alera?.GetRandomVoiceLine();
            case AnimatronicType.Amelia: return amelia?.GetRandomVoiceLine();
            case AnimatronicType.Oscar: return oscar?.GetRandomVoiceLine();
            case AnimatronicType.Scourage: return scourage?.GetRandomVoiceLine();
            case AnimatronicType.Shane: return shane?.GetRandomVoiceLine();
            default: return null;
        }
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
