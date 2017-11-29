using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    private float m_speed = 20;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(Launch(0.5f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Launch(float _lifeTime, Vector3 _dir)
    {
        float elapseTime = 0;
        while ((_lifeTime - elapseTime).Sgn() > 0)
        {
            elapseTime += Time.deltaTime;
            //transform.Translate(_dir * m_speed * Time.deltaTime);
            transform.position += _dir * m_speed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            //print("hit enemey " + other.name);
            other.transform.parent.GetComponent<Enemy>().IsFrozen = true;
        }
    }
}
