using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [Header("Collect Sound")]
    [SerializeField] private AudioClip collectSound;
    private int DataPads = 0;

    [SerializeField] private Text Count;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("DataPad"))
        {
            Destroy(collision.gameObject);
            DataPads++;
            Count.text = "" + DataPads;
            SoundManager.instance.PlaySound(collectSound);
        }
    }
}
