using UnityEngine;

public class TurretTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] bullets;
    private float cooldownTimer;
  
   

    private void Attack()
    {
        cooldownTimer = 0;
 

        bullets[FindBullets()].transform.position = firePoint.position;
        bullets[FindBullets()].GetComponent<EnemyProjectile>().ActivateProjectile();
     }

    private int FindBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
            Attack();
    }

}
