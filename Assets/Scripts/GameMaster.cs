using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Transform[] _terrains;

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
}
