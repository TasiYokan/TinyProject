using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 m_motion;
    public float speed;
    public Transform headTrans;
    private Vector3 m_lookPos;

    private Vector3 leftDown;
    private Vector3 rightUp;
    public float boundaryThickness = 0.3f;
    [SerializeField]
    private bool m_isConfused = false;

    public bool IsConfused
    {
        get
        {
            return m_isConfused;
        }

        set
        {
            m_isConfused = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        FindRoomCornerPoints(out leftDown, out rightUp);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseHitPos();
        MovementTypeB();

        ClampPlayerPos();

        AttackCheck();
    }

    private void MovementTypeB()
    {
        m_motion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += m_motion * speed;

        if (m_isConfused == false)
            headTrans.forward = (m_lookPos - headTrans.position).SetY(0);
        else
            headTrans.forward = Quaternion.AngleAxis(10, Vector3.up) * headTrans.forward;
    }

    private void MovementTypeA()
    {
        m_motion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (m_isConfused == false)
            headTrans.forward = (m_lookPos - headTrans.position).SetY(0);
        else
            headTrans.forward = Quaternion.AngleAxis(10, Vector3.up) * headTrans.forward;

        Vector3 faceDir = (m_lookPos - headTrans.position).SetY(0);

        if (faceDir.magnitude > 0.5f)
        {
            transform.position += headTrans.forward * m_motion.z * speed;
        }
        else if (faceDir.magnitude > 0.1f)
        {
            transform.position += headTrans.forward * m_motion.z * speed * faceDir.magnitude;
        }
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
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

    public void FindRoomCornerPoints(out Vector3 leftDown, out Vector3 rightUp)
    {
        RaycastHit hitInfoLeft;
        Physics.Raycast(transform.position, Vector3.left, out hitInfoLeft, 100, 1 << LayerMask.NameToLayer("Wall"));
        Debug.DrawLine(transform.position, hitInfoLeft.transform.position, Color.red);

        RaycastHit hitInfoRight;
        Physics.Raycast(transform.position, Vector3.right, out hitInfoRight, 100, 1 << LayerMask.NameToLayer("Wall"));
        Debug.DrawLine(transform.position, hitInfoRight.transform.position, Color.red);

        RaycastHit hitInfoFoward;
        Physics.Raycast(transform.position, Vector3.forward, out hitInfoFoward, 100, 1 << LayerMask.NameToLayer("Wall"));
        Debug.DrawLine(transform.position, hitInfoFoward.transform.position, Color.red);

        RaycastHit hitInfoBackward;
        Physics.Raycast(transform.position, Vector3.back, out hitInfoBackward, 100, 1 << LayerMask.NameToLayer("Wall"));
        Debug.DrawLine(transform.position, hitInfoBackward.transform.position, Color.red);

        leftDown = new Vector3(hitInfoLeft.transform.position.x + boundaryThickness, hitInfoBackward.transform.position.z + boundaryThickness);
        rightUp = new Vector3(hitInfoRight.transform.position.x - boundaryThickness, hitInfoFoward.transform.position.z - boundaryThickness);
        return;
    }

    private void ClampPlayerPos()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftDown.x, rightUp.x),
            0,
            Mathf.Clamp(transform.position.z, leftDown.y, rightUp.y));
    }
}
