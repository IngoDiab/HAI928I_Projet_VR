using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button mIntroButton = null;
    [SerializeField] Button mPlayButton = null;
    [SerializeField] Button mQuitButton = null;

    [SerializeField] string mIntroLevel = "";
    [SerializeField] string mMainLevel = "";

    private void Awake()
    {
        mIntroButton.onClick.AddListener(Intro);
        mPlayButton.onClick.AddListener(Play);
        mQuitButton.onClick.AddListener(Quit);

    }

    void Intro()
    {
        SceneManager.LoadScene(mIntroLevel);
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
