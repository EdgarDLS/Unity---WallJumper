using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMaster : MonoBehaviour
{
    public static GameMaster _GM;
    public AudioSource flickeringSound;
    public GameObject optionsMenu;

    [Space]
    public int nextLevel = 0;

    [SerializeField] private Transform[] _terrains;

    public Player player;
    Animator cameraAnimator;
    AsyncOperation asyncLoad;
    float animFlickeringLength = 4.3f;

    private void Awake()
    {
        if (_GM != null && _GM != this)
            Destroy(this.gameObject);
        else
            _GM = this;
    }

    private void Start()
    {
        cameraAnimator = Camera.main.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();

        optionsMenu.SetActive(false);
    }

    void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.RightControl))
  //      {
  //          foreach (Transform t in _terrains)
  //          {
  //              t.GetComponent<TerrainEasing>().Begin();
  //          }
  //      }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            FlickeringEffect();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            if (optionsMenu.activeSelf)
                optionsMenu.SetActive(false);
        }

        // Numpad level laod
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(animFlickeringLength);
        asyncLoad.allowSceneActivation = true;
    }

    public void GlitchEffect()
    {
        cameraAnimator.Play("Glitch");
    }

    public void FlickeringEffect()
    {
        asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
        asyncLoad.allowSceneActivation = false;

        flickeringSound.Play();
        cameraAnimator.Play("Flickering");

        StartCoroutine(LoadLevel());
    }

    public void PrepareNextLevel()
    {
        FlickeringEffect();
        player.PrepareNextLevel();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void PlayerDead()
    {
        player.Die();
        optionsMenu.SetActive(true);
    }

    // Trigger used to check if the player is out of bounds and kill him
    // ONLY FOR THE PROTOTYPE
    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerDead();
        }
    }
}
