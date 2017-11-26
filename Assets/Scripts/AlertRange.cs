using UnityEngine;
using System.Collections;

public class AlertRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.CompareTag("Player"))
        {
            //print("Plyer detected");
            other.transform.parent.GetComponent<Player>().IsConfused = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            //print("Plyer disappear");
            other.transform.parent.GetComponent<Player>().IsConfused = false;
        }
    }
}
