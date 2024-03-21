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
        currentBulletDrop = currentBulletDrop + (bulletDrop * Time.deltaTime);

        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self);
        transform.Translate(-Vector3.up * currentBulletDrop * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
