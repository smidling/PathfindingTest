using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public enum firingOrigin
    {
        invalid = -1,
       Player = 0,
       Enemy
    }

    [SerializeField] private firingOrigin myOrigin = firingOrigin.invalid;


    [SerializeField]
    private float speed = 1f; // fields per second (m/s)

    private bool fired = false;
    private Vector3 direction = new Vector3();
    public void Fire( Vector3 newDir, bool playerFire)
    {
        myOrigin = playerFire ? firingOrigin.Player : firingOrigin.Enemy;
        direction = newDir;
        fired = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(fired)
            transform.localPosition += direction.normalized * speed * Time.fixedDeltaTime;

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player" || col.collider.tag == "Base")
        {

            if (myOrigin == firingOrigin.Enemy)
                Time.timeScale = 0;
        }
        else if (col.collider.tag == "Enemy")
        {
            if (myOrigin == firingOrigin.Player)
            {
                // hit enemy, selfdestroy & destroy enemy
                Destroy(gameObject);
                Destroy(col.collider.gameObject);
            }
        }
        else
        {
            // hit wall, selfdestroy
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (myOrigin == firingOrigin.Enemy)
            if (col.tag == "Base")
                // end game
                Time.timeScale = 0;
    }


}
