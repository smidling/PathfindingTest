using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField] private float speed = 1f; // fields per second (m/s)
    private List<PathfindingNode> myPath = new List<PathfindingNode>();
    private bool startMoving = false;
    private int nextNodeIter = 0;
    private float distancePonder = 0.02f;


    [SerializeField]
    private float shootTimeout = 1f; // second 

    private bool canShoot = true;

    public void SetPath(List<PathfindingNode> path)
    {
        myPath = path;
        startMoving = true;
        // two iteration moves
        distancePonder = speed*Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        // shooting part
        int raycastMask = LayerMask.GetMask("Enemy");
        raycastMask = ~raycastMask;
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.one/2f, Vector2.down, 100, raycastMask);
        if (hit.collider != null)
            if (hit.collider.tag == "Player" || hit.collider.tag == "Base")
                if (canShoot)
                {
                    canShoot = false;
                    StartCoroutine(Shoot(Vector3.down));
                }
        // Cast a ray straight right.
        hit = Physics2D.Raycast(transform.position + Vector3.one/2f, Vector2.right, 100, raycastMask);
        if (hit.collider != null)
            if (hit.collider.tag == "Player" || hit.collider.tag == "Base")
                if (canShoot)
                {
                    canShoot = false;
                    StartCoroutine(Shoot(Vector3.right));
                }
        // Cast a ray straight left.
        hit = Physics2D.Raycast(transform.position + Vector3.one/2f, Vector2.left, 100, raycastMask);
        if (hit.collider != null)
            if (hit.collider.tag == "Player" || hit.collider.tag == "Base")
                if (canShoot)
                {
                    canShoot = false;
                    StartCoroutine(Shoot(Vector3.left));
                }
        
        // moving part
        if (!startMoving)
            return;

        if (nextNodeIter >= myPath.Count)
            return;

        Vector3 direction = myPath[nextNodeIter].GetWorldPos() - transform.localPosition;
        if (direction.magnitude < distancePonder)
        {
            //you are there, snap to pos and move to next point
            transform.localPosition = myPath[nextNodeIter].GetWorldPos();
            nextNodeIter++;
        }
        else
            transform.localPosition += direction.normalized*speed*Time.fixedDeltaTime;
    }


    IEnumerator Shoot(Vector3 direction)
    {
        yield return new WaitForSeconds(shootTimeout / 2);


        float oldSpeed = speed; // fields per second (m/s)
        speed = 0;
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.one / 2f, new Quaternion(), transform.parent);
        bullet.GetComponent<BulletCtrl>().Fire(direction, false);
        StartCoroutine(Reload());

        yield return new WaitForSeconds(shootTimeout / 2);
        speed = oldSpeed;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(shootTimeout);
        canShoot = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
            // end game
            Time.timeScale = 0;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Base")
            // end game
            Time.timeScale = 0;
    }


}
