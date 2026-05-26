using UnityEngine;

public class DoorClose : MonoBehaviour
{
    private Animator doorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLayer"))
        {
            if(doorAnimator != null)
            {
                doorAnimator.SetTrigger("Door_Close");
            }
        }
    }
}
