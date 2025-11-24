using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //Singleton
    
    private PlayerController player;
    private GameObject pauseMenuUI;
    private bool _paused;
    public bool IsPaused => _paused;
    private bool _isMenuScene;
    [SerializeField] private string menuScene = "MainMenu";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isMenuScene = scene.name == menuScene;
        player = FindFirstObjectByType<PlayerController>();
        pauseMenuUI = GameObject.Find("PauseMenu")
                      ?? FindInactive("PauseMenu");

        _paused = false;
        Time.timeScale = 1f;
        SetCursorState(_isMenuScene);
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
    private GameObject FindInactive(string name)
    {
        var all = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in all)
        {
            if (go.name == name && go.hideFlags == HideFlags.None)
                return go;
        }
        return null;
    }
    
    void Update()
    {
        if (_isMenuScene) return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_paused) Instance.Resume();
            else Instance.Pause();
        }
    }
    
    public void Pause()
    {
        if (_paused) return;
        
        _paused = true;
        Time.timeScale = 0f;
        SetCursorState(true);
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        
        if (player != null)
            player.Paused = true;
    }

    public void Resume()
    {
        if (!_paused) return;
        
        _paused = false;
        Time.timeScale = 1f;
        SetCursorState(false);
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (player != null)
            player.Paused = false;
    }
    
    public void LoadScene(string sceneName)
    {
        Debug.Log("LoadScene");
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
