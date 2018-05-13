using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    GameObject pickUpSound;

    private void Start()
    {
        pickUpSound = this.transform.Find("Sound").gameObject;
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            pickUpSound.GetComponent<AudioSource>().Play();
            pickUpSound.transform.parent = null;

            Destroy(this.gameObject);
        }
    }
}
