using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button mPlayButton = null;
    [SerializeField] Button mQuitButton = null;

    [SerializeField] string mMainLevel = "";

    private void Awake()
    {
        mPlayButton.onClick.AddListener(Play);
        mQuitButton.onClick.AddListener(Quit);
    }

    void Play()
    {
        SceneManager.LoadScene(mMainLevel);
    }

    void Quit()
    {
        Application.Quit();
    }
}
