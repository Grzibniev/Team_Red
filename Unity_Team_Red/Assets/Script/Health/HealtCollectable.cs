using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtCollectable : MonoBehaviour
{
    [SerializeField] private float healtValue;
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(collectSound);
            collision.GetComponent<Health>().addHealth(healtValue);
            gameObject.SetActive(false);
        }
    }
}
