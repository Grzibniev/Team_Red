using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("FireTrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip trapSound;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
        private  IEnumerator ActivateFiretrap()
    {
            //zmienia kolor pu³apki na czerwony 
            triggered = true;
            spriteRend.color = Color.red;

            //czeka na opóznienie, aktywuje pu³apke,aktywuje animacje,zmienia kolor na podstawowy 
            yield return new WaitForSeconds(activationDelay);
            SoundManager.instance.PlaySound(trapSound);
            spriteRend.color = Color.white;
            active = true;
            anim.SetBool("activated", true);

            //czeka x sekund, wy³¹cza pu³ake i resetuje wartosci
            yield return new WaitForSeconds(activeTime);
            active = false;
            triggered = false;
        anim.SetBool("activated", false);
    }
    

    
}
