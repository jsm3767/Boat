using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health;
    
   public void ApplyDamage(int amount)
    {
        health -= amount;

        if(health < 0)
        {
            //gameObject.SendMessage("DeathFunctionName")
            //Destroy(gameObject);
        }
    }
}
