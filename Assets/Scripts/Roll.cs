using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    public GameObject diceObj;
    public ForceMode m_forceMode;

    List<Transform> m_dice;
    List<Rigidbody> m_diceRigidbody;
    Vector3 m_throwDirection;
    // Start is called before the first frame update
    void Start()
    {
        m_throwDirection = new Vector3(0, 1, -1);
        m_dice = new List<Transform>();     m_diceRigidbody = new List<Rigidbody>();
        for (int i = 0; i < 5; i++)
        {
            m_dice.Add(diceObj.transform.GetChild(i));
            m_diceRigidbody.Add(m_dice[i].GetComponent<Rigidbody>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Left Click to Roll
        if(Input.GetMouseButton(0)) RollDice();
    }

    void RollDice()
    {
        Debug.Log("RollDice() Called");
        for(int i = 0; i < 5; i++)
        {
            m_diceRigidbody[i].useGravity = true;
            m_diceRigidbody[i].AddForce(m_throwDirection * .5f, m_forceMode);
        }
    }
}
