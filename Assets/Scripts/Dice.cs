using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public static Vector3 sideOne = new Vector3(-90, 0, 0);
    public static Vector3 sideTwo = new Vector3(0, 0, -90);
    public static Vector3 sideThree = new Vector3(0, 0, 0);
    public static Vector3 sideFour = new Vector3(0, 0, -180);
    public static Vector3 sideFive = new Vector3(0, 0, 90);
    public static Vector3 sideSix = new Vector3(90, 0, 0);

    public int m_state;     // 0: Waiting, 1: Rolled
    Vector3 m_initialPos;
    Vector3 m_initialRot;
    Rigidbody diceRigidbody;

    public Roll roll;
    // Start is called before the first frame update
    void Start()
    {
        m_state = 0;
        diceRigidbody = GetComponent<Rigidbody>();

        m_initialPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        m_initialRot = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Keep Dice Rotating for Random Results
        if (m_state == 0)
        {
            gameObject.transform.Rotate(0, 0, 120 * Time.deltaTime);
            Debug.Log("Waiting...");
        }

        if (Input.GetKeyDown(KeyCode.R)) ResetDice();
    }

    void ResetDice()
    {
        gameObject.transform.position = m_initialPos;
        gameObject.transform.eulerAngles = m_initialRot;
        diceRigidbody.useGravity = false;
        m_state = 0;
        roll.m_rolled = false;
    }
}
