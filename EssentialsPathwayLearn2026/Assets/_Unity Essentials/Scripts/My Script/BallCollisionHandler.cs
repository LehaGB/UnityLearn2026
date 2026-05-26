using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    private AudioSource ballSource;

    public AudioClip ballClip;

    private void Start()
    {
        ballSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || 
            collision.gameObject.CompareTag("Ramp"))
        {
            PlayHitSound();
        }
    }

    private void PlayHitSound()
    {
        if(ballClip != null && ballSource != null)
        {
            ballSource.PlayOneShot(ballClip);
        }
        else
        {
            Debug.Log("Аудио неназнаечен");
        }
    }
}
