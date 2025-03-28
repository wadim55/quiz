using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioSource audio;
    public static GameManager Inst;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource fon;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        audio = GetComponent<AudioSource>();
        if (Inst == null)
            Inst = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        fon.Play();
    }
    public void ChangeScene(int numScene)
    {
        PlayClip(1);
        SceneManager.LoadScene(numScene);
        
    }

    public void MuteSound(bool mute)
    {
        fon.mute = mute;
        audio.mute = mute;
    }

    public void PlayClip(int num)
    {
        audio.clip = clips[num];
        audio.Play();
    }

    public void Quit()
    {
        Application.Quit();  
    }
}
