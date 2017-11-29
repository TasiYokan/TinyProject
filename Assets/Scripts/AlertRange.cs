using UnityEngine;
using System.Collections;

public class AlertRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            //print("Plyer detected");
            other.transform.GetComponent<Player>().IsConfused = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //print("Plyer disappear");
            other.transform.GetComponent<Player>().IsConfused = false;
        }
    }
}
