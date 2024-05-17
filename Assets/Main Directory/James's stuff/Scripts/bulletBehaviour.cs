using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : NetworkBehaviour
{
    public float bulletSpeed;
    public float bulletDrop;
    public float knockbackForce;
    public float dropOffAfter;
    private float currentBulletDrop = 0;

    public int teamID = 0;

    private float dropOffTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dropOffTimer += Time.deltaTime;
        // Current bullt drop accumulates over time by adding onto itself every frame (don't know if this is best way to do it)
        

        // Bullet will be instanciated in correct facing direciton so we just move it forward
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self);
        // Bullet moves down according to world space
        if (dropOffTimer > dropOffAfter)
        {
            transform.Translate(-Vector3.up * currentBulletDrop * Time.deltaTime, Space.World);
            currentBulletDrop += (bulletDrop * Time.deltaTime);
        }
    }

    // Bullet destroys when it hits something
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameObject seeIfWorks = col.transform.gameObject;

            int _damage = Random.Range(20, 25);

            if (seeIfWorks.name == "QuickRigCharacter_Head") _damage = 30;

            PlayerHealth playerHealth = seeIfWorks.GetComponent<PlayerHealth>();
            bool found = false;

            while (!found)
            {
                if(playerHealth == null)
                {
                    seeIfWorks = seeIfWorks.transform.parent.gameObject;
                    playerHealth = seeIfWorks.GetComponent<PlayerHealth>();
                }
                else
                {
                    DamagePlayer(_damage, teamID, playerHealth);
                    found = true;
                }
            }
        }
        Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
        if (rb != null) rb.AddForce(transform.forward * knockbackForce, ForceMode.Impulse);

        Destroy(gameObject);
    }

    
    void DamagePlayer(int _damage, int _teamID, PlayerHealth healthScript)
    {
        healthScript.DamagePlayer(_damage, _teamID);
    }
}