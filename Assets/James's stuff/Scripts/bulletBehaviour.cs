using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDrop;
    private float currentBulletDrop = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Current bullt drop accumulates over time by adding onto itself every frame (don't know if this is best way to do it)
        currentBulletDrop += (bulletDrop * Time.deltaTime);

        // Bullet will be instanciated in correct facing direciton so we just move it forward
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self);
        // Bullet moves down according to world space
        transform.Translate(-Vector3.up * currentBulletDrop * Time.deltaTime, Space.World);
    }

    // Bullet destroys when it hits something
    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
