using UnityEngine;

public class JerboasEscape_FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1,10)]
    public float smoothFactor;

    //public AudioClip Runsound;

    void Start(){
        GetComponent<AudioSource>().Play();
        //AudioSource.PlayClipAtPoint(Runsound, transform.position);
    }
    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {

        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
