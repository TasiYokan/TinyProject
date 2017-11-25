using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 m_motion;
    public float speed;
    public Transform headTrans;
    private Vector3 m_lookPos;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseHitPos();

        m_motion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //transform.position += m_motion * speed;

        headTrans.forward = (m_lookPos - headTrans.position).SetY(0);
        Vector3 faceDir = (m_lookPos - headTrans.position).SetY(0);

        if (faceDir.magnitude > 0.1f)
        {
            //float angle = Vector3.SignedAngle(transform.forward, faceDir, Vector3.up) * 1f;
            //transform.Rotate(Vector3.up, angle, Space.World);
        }

        if (faceDir.magnitude > 0.5f)
        {
            transform.position += headTrans.forward * m_motion.z * speed;
        }
        else if (faceDir.magnitude > 0.1f)
        {
            transform.position += headTrans.forward * m_motion.z * speed * faceDir.magnitude;
        }

        AttackCheck();
    }

    private void UpdateMouseHitPos()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
            if (hit.transform.CompareTag("Ground"))
                m_lookPos = hit.point;
    }

    private void AttackCheck()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            WaterBall waterBall = GameObject.Instantiate(
                Resources.Load("Prefabs/WaterBall") as GameObject,
                headTrans.position,
                Quaternion.identity,
                transform
                ).GetComponent<WaterBall>();
            //waterBall.transform.position = transform.position;
            //waterBall.transform.forward = headTrans.forward;
            waterBall.StartCoroutine(waterBall.Launch(0.5f, headTrans.forward));
        }
    }
}
