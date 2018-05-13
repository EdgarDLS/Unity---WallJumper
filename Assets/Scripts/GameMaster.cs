using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMaster : MonoBehaviour
{
    public static GameMaster _GM;
    public AudioSource flickeringSound;

    [Space]
    public int nextLevel = 0;

    [SerializeField] private Transform[] _terrains;

    Animator cameraAnimator;
    AsyncOperation asyncLoad;
    float animFlickeringLength = 4.5f;

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
    }

    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.RightControl))
        {
            foreach (Transform t in _terrains)
            {
                t.GetComponent<TerrainEasing>().Begin();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            FlickeringEffect();
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
}
