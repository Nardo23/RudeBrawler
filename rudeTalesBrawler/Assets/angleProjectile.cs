using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angleProjectile : MonoBehaviour
{
    public float speed;
    public GameObject projectile, shadow;
    bool landed = false;
    public bool isPlayer = true;
    public bool hitOnce = true;
    Vector3 direction;
    public AudioSource Sor;
    [SerializeField]
    AudioClip[] impactSound;
    public Vector2 pitchRange = new Vector2(.9f,1.1f);
    // Start is called before the first frame update
    void Start()
    {
        direction = projectile.transform.right;
        //Debug.Log("projectile direction: " + direction);
    }

    public void SetProjectile(Transform projectileStart, Transform shadowStart, float zRotation)
    {
        projectile.transform.position = projectileStart.position;
        shadow.transform.position = shadowStart.position;
        Vector3 rotationVector = new Vector3(0f, 0f, zRotation);
        projectile.transform.localRotation =  Quaternion.Euler (rotationVector);
        direction = projectile.transform.right;

    }


    // Update is called once per frame
    void Update()
    {
        if (!landed)
        {
            projectile.transform.position += direction * speed * Time.deltaTime;
            shadow.transform.position = new Vector3(projectile.transform.position.x, shadow.transform.position.y, 0);
            //Debug.Log("Projectile position: " + projectile.transform.position);

        }
        if(projectile.transform.position.y <= shadow.transform.position.y)
        {
            projectile.transform.position = new Vector3 (projectile.transform.position.x,shadow.transform.position.y,0);
            if (!landed)
            {
                if (Sor != null)
                {
                    Sor.pitch= Random.Range(pitchRange.x, pitchRange.y);
                    Sor.PlayOneShot(impactSound[Random.Range(0, impactSound.Length)]);

                }
            }
            landed = true;
            GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer && collision.tag == "EnemyHurt")
        {
            if (hitOnce)
            {
                Destroy(this.gameObject);
            }
        }
        if (!isPlayer && collision.tag == "PlayerHurt")
        {
            if (hitOnce)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
