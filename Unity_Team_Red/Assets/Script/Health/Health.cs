using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;

    [Header("Hurt Sound")]
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if (currentHealth > 0)
        {       
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");

                //player
                if(GetComponent<PlayerMovement>() !=null)
                GetComponent<PlayerMovement>().enabled = false;

                //enemy
                if (GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                if (GetComponent<MeleeEnemy>() != null)
                    GetComponent<MeleeEnemy>().enabled = false;

                if (GetComponent<BossMeleeEnemy>() != null)
                    GetComponent<BossMeleeEnemy>().enabled = false;
               
            

                dead = true;

                if (GetComponent<BossMeleeEnemy>() == dead)
                {
                    SceneManager.LoadScene(4);
                }

                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }
    public void addHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Respawn()
    {
        dead = false;
        addHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Player_idle");
        StartCoroutine(Invunerability());

        if (GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = true;
    }
    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(6, 9, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

        }
        Physics2D.IgnoreLayerCollision(6, 9, false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

   
}
