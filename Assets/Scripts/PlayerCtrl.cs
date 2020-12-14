using System.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField]
    private AudioSource myAudioSource;
    [SerializeField]
    private AudioSource shootAudioSource;
    [SerializeField]
    private AudioClip shootAudioClip;

    [SerializeField]
    private float speed = 1f; // fields per second (m/s)


    [SerializeField]
    private float shootTimeout = 1f; // second 

    private bool canShoot = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ControlsOverlord.Instance.MoveRight)
        {
            transform.localPosition += Vector3.right*speed*Time.fixedDeltaTime;
            if(!myAudioSource.isPlaying)
                myAudioSource.Play();
        }
        else if (ControlsOverlord.Instance.MoveLeft)
        {
            transform.localPosition -= Vector3.right*speed*Time.fixedDeltaTime;
            if (!myAudioSource.isPlaying)
                myAudioSource.Play();
        }
        else if (ControlsOverlord.Instance.Shoot)
            Shoot();
        else
        {
            myAudioSource.Stop();
        }


    }

    void Shoot()
    {
        if (!canShoot)
            return;
        shootAudioSource.PlayOneShot(shootAudioClip);
        canShoot = false;
        GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.one/2f, new Quaternion(), transform.parent);
        bullet.GetComponent<BulletCtrl>().Fire(Vector3.up, true);
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(shootTimeout);
        canShoot = true;
    }

}
