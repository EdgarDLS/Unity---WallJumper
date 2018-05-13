using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider objectTrigger)
    {
        if (objectTrigger.transform.tag.Equals("Player"))
            GameMaster._GM.FlickeringEffect();
    }
}
