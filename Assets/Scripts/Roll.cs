using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    public GameObject diceObj;
    public ForceMode m_forceMode;
    public ScoreManager sManager;

    List<Transform> m_dice;
    List<Rigidbody> m_diceRigidbody;
    List<Dice> m_diceProperties;
    Vector3 m_throwDirection;
    public bool m_rolled;

    // Start is called before the first frame update
    void Start()
    {
        m_throwDirection = new Vector3(-1, 1, 0);   m_rolled = false;
        m_dice = new List<Transform>();     m_diceRigidbody = new List<Rigidbody>();    m_diceProperties = new List<Dice>();
        for (int i = 0; i < 5; i++)
        {
            m_dice.Add(diceObj.transform.GetChild(i));
            m_diceRigidbody.Add(m_dice[i].GetComponent<Rigidbody>());
            m_diceProperties.Add(m_dice[i].GetComponent<Dice>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Left Click to Roll
        if(Input.GetKeyDown(KeyCode.Space) && !m_rolled) RollDice();
    }

    void RollDice()
    {
        Debug.Log("RollDice() Called");
        for(int i = 0; i < 5; i++)
        {
            if(!m_diceProperties[i].m_chosen)
            {
                m_diceRigidbody[i].useGravity = true;
                m_diceRigidbody[i].AddForce(m_throwDirection * 12f, m_forceMode);
                m_diceProperties[i].m_state = 1;
            }
        }
        m_rolled = true;        // Change this back to false after next action starts
        StartCoroutine(WaitUntilStatic());
    }

    IEnumerator WaitUntilStatic()
    {
        yield return new WaitForSeconds(2.5f);
        Debug.Log("Wait Finished");
        sManager.DiceStatic();
    }
}
