using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    public bool isPaused;

    public GameObject player;


    public PlayerMovement playerScript;

    float originalTimeScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();

        originalTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (activeMenu == null)
            {
                Pause();
                activeMenu = pauseMenu;
                activeMenu.SetActive(true);
            }
            else if (activeMenu == pauseMenu)
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;

        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        isPaused = false;

        Time.timeScale = originalTimeScale;
        Cursor.lockState = CursorLockMode.Locked;

        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void YouWon()
    {
        if (activeMenu != null)
            return;

        Pause();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }

    public void YouLose()
    {
        if (activeMenu != null)
            return;

        Pause();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
}
