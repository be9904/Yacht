using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameObject diceObj;
    public GameObject diceUpclose;
    public GameObject diceChosen;
    public GameObject scoreUI;
    public Text totalScore;

    List<Transform> m_dice;
    List<Dice> m_diceProperties;
    List<Transform> m_diceUpclose;
    List<Transform> m_diceChosen;
    
    List<int> m_scores;
    List<Text> m_userScore;

    bool m_diceStatic;
    // Start is called before the first frame update
    void Start()
    {
        m_diceStatic = false;
        m_dice = new List<Transform>();     m_diceProperties = new List<Dice>();    m_diceUpclose = new List<Transform>();  m_diceChosen = new List<Transform>();   m_userScore = new List<Text>(); m_scores = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            m_dice.Add(diceObj.transform.GetChild(i));
            m_diceProperties.Add(m_dice[i].GetComponent<Dice>());
            m_diceUpclose.Add(diceUpclose.transform.GetChild(i));
            m_diceChosen.Add(diceChosen.transform.GetChild(i));
            m_scores.Add(0);
        }
        for (int i = 0; i < 12; i++) m_userScore.Add(scoreUI.transform.GetChild(i).GetComponent<Text>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (m_diceStatic) BringUpclose(other.gameObject);
    }

    public void DiceStatic()
    {
        m_diceStatic = true;
    }
    // Identifies Rolled Number
    int IdentifyNumber(GameObject die)
    {
        int retVal = -1;
        switch (die.name)
        {
            case "1":
                retVal = 6;
                break;
            case "2":
                retVal = 5;
                break;
            case "3":
                retVal = 4;
                break;
            case "4":
                retVal = 3;
                break;
            case "5":
                retVal = 2;
                break;
            case "6":
                retVal = 1;
                break;
            default:
                break;
        }
        return retVal;
    }

    public void BringUpclose(GameObject dieNum)
    {
        Debug.Log(dieNum.name);
        Rigidbody dieRigidBody;
        // Stop Die from Moving
        if (dieNum.transform.parent.gameObject.GetComponent<Rigidbody>() != null)
        {
            dieRigidBody = dieNum.transform.parent.gameObject.GetComponent<Rigidbody>();
            dieRigidBody.velocity = Vector3.zero;
            dieRigidBody.useGravity = false;
        }

        int _dieNum = IdentifyNumber(dieNum);
        Debug.Log("Number is: " + _dieNum.ToString());
        // Change to Appropriate Rotation
        switch (_dieNum)
        {
            case 1:
                dieNum.transform.parent.transform.eulerAngles = new Vector3(Dice.sideOne.x, Dice.sideOne.y, Dice.sideOne.z);
                break;
            case 2:
                dieNum.transform.parent.transform.eulerAngles = new Vector3(Dice.sideTwo.x, Dice.sideTwo.y, Dice.sideTwo.z);
                break;
            case 3:
                dieNum.transform.parent.transform.eulerAngles = new Vector3(Dice.sideThree.x, Dice.sideThree.y, Dice.sideThree.z);
                break;
            case 4:
                dieNum.transform.parent.transform.eulerAngles = new Vector3(Dice.sideFour.x, Dice.sideFour.y, Dice.sideFour.z);
                break;
            case 5:
                dieNum.transform.parent.transform.eulerAngles = new Vector3(Dice.sideFive.x, Dice.sideFive.y, Dice.sideFive.z);
                break;
            case 6:
                dieNum.transform.parent.transform.eulerAngles = new Vector3(Dice.sideSix.x, Dice.sideSix.y, Dice.sideSix.z);
                break;
            default:
                break;
        }

        // Move Die Upclose
        switch (dieNum.transform.parent.gameObject.name)
        {
            case "white":
                dieNum.transform.parent.transform.position = m_diceUpclose[0].position;
                break;
            case "black":
                dieNum.transform.parent.transform.position = m_diceUpclose[1].position;
                break;
            case "red":
                dieNum.transform.parent.transform.position = m_diceUpclose[2].position;
                break;
            case "blue":
                dieNum.transform.parent.transform.position = m_diceUpclose[3].position;
                break;
            case "green":
                dieNum.transform.parent.transform.position = m_diceUpclose[4].position;
                break;
            default:
                break;
        }
    }

    // 0: Aces          1: Deuces           2: Threes
    // 3: Fours         4: Fives            5: Sixes
    // 6: Choice        7: 4 of a Kind      8: Full House
    // 9: S. Straight   10: L. Straight     11: Yacht
    void YachtScore()
    {

    }
}
