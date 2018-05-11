using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster _GM;

    [SerializeField] private Transform[] _terrains;

    Animator cameraAnimator;


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
	}

    public void GlitchEffect()
    {
        cameraAnimator.Play("Glitch");
    }
}
