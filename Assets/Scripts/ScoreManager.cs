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
    public Text bonusScore;
    public Text totalScore;
    public Canvas rerollButton;

    List<Transform> m_dice;
    List<Dice> m_diceProperties;
    List<Transform> m_diceUpclose;
    List<Transform> m_diceChosen;
    
    List<int> m_scores;
    List<Text> m_userScore;

    public bool m_diceStatic;
    int m_numChosen;
    int m_numRolls;
    int total;
    int turns;
    bool m_choosingDice;
    bool m_bonusAdded;
    Ray m_ray;

    bool m_aces;
    bool m_deuces;
    bool m_threes;
    bool m_fours;
    bool m_fives;
    bool m_sixes;
    bool m_choice;
    bool m_fourofakind;
    bool m_fullhouse;
    bool m_sstraight;
    bool m_lstraight;
    bool m_yacht;
    // Start is called before the first frame update
    void Start()
    {
        m_aces = false; m_deuces = false; m_threes = false; m_fours = false; m_fives = false; m_sixes = false;
        m_choice = false; m_fourofakind = false; m_fullhouse = false; m_sstraight = false; m_lstraight = false; m_yacht = false;
        m_bonusAdded = false;   totalScore.text = "0";  bonusScore.text = "0";

        m_diceStatic = false;   m_choosingDice = false;     m_numChosen = 0;    m_numRolls = 0;     total = 0;      turns = 0;
        m_dice = new List<Transform>();     m_diceProperties = new List<Dice>();    m_diceUpclose = new List<Transform>();  m_diceChosen = new List<Transform>();   m_userScore = new List<Text>(); m_scores = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            m_dice.Add(diceObj.transform.GetChild(i));
            m_diceProperties.Add(m_dice[i].GetComponent<Dice>());
            m_diceUpclose.Add(diceUpclose.transform.GetChild(i));
            m_diceChosen.Add(diceChosen.transform.GetChild(i));
            m_scores.Add(0);
        }
        for (int i = 0; i < 12; i++) m_userScore.Add(scoreUI.transform.GetChild(i).GetChild(0).GetComponent<Text>());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_choosingDice) ChooseDice();
        if (m_choosingDice && m_diceStatic) ClickField();
        if (turns == 12) { }                                                        // End Game
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (m_diceStatic)
        {
            BringUpclose(other.gameObject);
            m_choosingDice = true;
        }
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
        rerollButton.gameObject.SetActive(true);
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
                m_scores[0] = _dieNum;
                break;
            case "black":
                dieNum.transform.parent.transform.position = m_diceUpclose[1].position;
                m_scores[1] = _dieNum;
                break;
            case "red":
                dieNum.transform.parent.transform.position = m_diceUpclose[2].position;
                m_scores[2] = _dieNum;
                break;
            case "blue":
                dieNum.transform.parent.transform.position = m_diceUpclose[3].position;
                m_scores[3] = _dieNum;
                break;
            case "green":
                dieNum.transform.parent.transform.position = m_diceUpclose[4].position;
                m_scores[4] = _dieNum;
                break;
            default:
                break;
        }

        CheckScore();
    }

    void ChooseDice()
    {
        bool chosen = false;

        if (Input.GetMouseButtonDown(0))
        {
            // Reset ray with new mouse position
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(m_ray);

            for(int i = 0; i < hits.Length; i++)
            {
                
                switch (hits[i].collider.gameObject.tag)
                {
                    case "white":
                        m_diceProperties[0].m_chosen = !m_diceProperties[0].m_chosen;
                        if (m_diceProperties[0].m_chosen)
                        {
                            hits[i].collider.gameObject.transform.position = m_diceChosen[0].position;
                            m_numChosen++;
                        }
                        else
                        {
                            hits[i].collider.gameObject.transform.position = m_diceUpclose[0].position;
                            m_numChosen--;
                        }
                        chosen = true;
                        break;
                    case "black":
                        m_diceProperties[1].m_chosen = !m_diceProperties[1].m_chosen;
                        if (m_diceProperties[1].m_chosen)
                        {
                            hits[i].collider.gameObject.transform.position = m_diceChosen[1].position;
                            m_numChosen++;
                        }
                        else
                        {
                            hits[i].collider.gameObject.transform.position = m_diceUpclose[1].position;
                            m_numChosen--;
                        }
                        chosen = true;
                        break;
                    case "red":
                        m_diceProperties[2].m_chosen = !m_diceProperties[2].m_chosen;
                        if (m_diceProperties[2].m_chosen)
                        {
                            hits[i].collider.gameObject.transform.position = m_diceChosen[2].position;
                            m_numChosen++;
                        }
                        else
                        {
                            hits[i].collider.gameObject.transform.position = m_diceUpclose[2].position;
                            m_numChosen--;
                        }
                        chosen = true;
                        break;
                    case "blue":
                        m_diceProperties[3].m_chosen = !m_diceProperties[3].m_chosen;
                        if (m_diceProperties[3].m_chosen)
                        {
                            hits[i].collider.gameObject.transform.position = m_diceChosen[3].position;
                            m_numChosen++;
                        }
                        else
                        {
                            hits[i].collider.gameObject.transform.position = m_diceUpclose[3].position;
                            m_numChosen--;
                        }
                        chosen = true;
                        break;
                    case "green":
                        m_diceProperties[4].m_chosen = !m_diceProperties[4].m_chosen;
                        if (m_diceProperties[4].m_chosen)
                        {
                            hits[i].collider.gameObject.transform.position = m_diceChosen[4].position;
                            m_numChosen++;
                        }
                        else
                        {
                            hits[i].collider.gameObject.transform.position = m_diceUpclose[4].position;
                            m_numChosen--;
                        }
                        chosen = true;
                        break;
                    case "reroll":
                        if (m_numChosen != 5 && m_numRolls < 2)
                        {
                            for (int j = 0; j < 5; j++) m_diceProperties[j].ResetDie();
                            m_numRolls++;
                            rerollButton.gameObject.SetActive(false);
                        }
                        break;
                    default:
                        break;
                }
                if (chosen) break;
            }
        }
    }

    // 0: Aces          1: Deuces           2: Threes
    // 3: Fours         4: Fives            5: Sixes
    // 6: Choice        7: 4 of a Kind      8: Full House
    // 9: S. Straight   10: L. Straight     11: Yacht

    int CheckAces()
    {
        int score = 0;
        for(int i = 0; i < 5; i++)
        {
            if (m_scores[i] == 1) score++;
        }

        m_userScore[0].text = score.ToString();

        return score;
    }

    int CheckDeuces()
    {
        int score = 0;
        for (int i = 0; i < 5; i++)
        {
            if (m_scores[i] == 2) score += 2;
        }

        m_userScore[1].text = score.ToString();

        return score;
    }

    int CheckThrees()
    {
        int score = 0;
        for (int i = 0; i < 5; i++)
        {
            if (m_scores[i] == 3) score += 3;
        }

        m_userScore[2].text = score.ToString();

        return score;
    }

    int CheckFours()
    {
        int score = 0;
        for (int i = 0; i < 5; i++)
        {
            if (m_scores[i] == 4) score += 4;
        }

        m_userScore[3].text = score.ToString();
        
        return score;
    }

    int CheckFives()
    {
        int score = 0;
        for (int i = 0; i < 5; i++)
        {
            if (m_scores[i] == 5) score += 5;
        }

        m_userScore[4].text = score.ToString();

        return score;
    }

    int CheckSixes()
    {
        int score = 0;
        for (int i = 0; i < 5; i++)
        {
            if (m_scores[i] == 6) score += 6;
        }

        m_userScore[5].text = score.ToString();

        return score;
    }

    int CheckChoice()
    {
        int score = 0;
        for (int i = 0; i < 5; i++) score += m_scores[i];

        m_userScore[6].text = score.ToString();

        return score;
    }

    int CheckFourOfAKind()
    {
        int score = 0;  int[] repeated = new int[] { 0, 0, 0, 0, 0, 0 }; bool isTrue = false;
        for (int i = 0; i < 5; i++){ repeated[m_scores[i] - 1]++;    score += m_scores[i]; }
        for (int i = 0; i < 6; i++){ if (repeated[i] >= 4) isTrue = true; }

        if (isTrue) m_userScore[7].text = score.ToString();
        else m_userScore[7].text = "0";

        return score;
    }

    int CheckFullHouse()
    {
        int score = 0; int[] repeated = new int[] { 0, 0, 0, 0, 0, 0 }; bool threeNum = false; bool twoNum = false;
        for (int i = 0; i < 5; i++) { repeated[m_scores[i] - 1]++; score += m_scores[i]; }
        for (int i = 0; i < 6; i++) 
        { 
            if (repeated[i] == 3) threeNum = true;
            if (repeated[i] == 2) twoNum = true;
        }

        if(threeNum && twoNum) m_userScore[8].text = score.ToString();
        else m_userScore[8].text = "0";

        return score;
    }

    int CheckSmallStraight()
    {
        int score = 15; int[] repeated = new int[] { 0, 0, 0, 0, 0, 0 }; bool isTrue = false;
        for (int i = 0; i < 5; i++) { repeated[m_scores[i] - 1]++; }
        string debug = "";
        for (int i = 0; i < 6; i++) { debug += repeated[i].ToString() + " "; }
        Debug.Log("REPEATED: " + debug);
        if (repeated[0] >= 1 && repeated[1] >= 1 && repeated[2] >= 1 && repeated[3] >= 1) isTrue = true;
        if (repeated[1] >= 1 && repeated[2] >= 1 && repeated[3] >= 1 && repeated[4] >= 1) isTrue = true;
        if (repeated[2] >= 1 && repeated[3] >= 1 && repeated[4] >= 1 && repeated[5] >= 1) isTrue = true;

        if (isTrue) m_userScore[9].text = "15";
        else m_userScore[9].text = "0";

        return score;
    }

    int CheckLargeStraight()
    {
        int score = 30; int[] repeated = new int[] { 0, 0, 0, 0, 0, 0 }; bool isTrue = false;
        for (int i = 0; i < 5; i++) { repeated[m_scores[i] - 1]++; }
        if (repeated[0] == 1 && repeated[1] == 1 && repeated[2] == 1 && repeated[3] == 1 && repeated[4] == 1) isTrue = true;
        if (repeated[1] == 1 && repeated[2] == 1 && repeated[3] == 1 && repeated[4] == 1 && repeated[5] == 1) isTrue = true;

        if (isTrue) m_userScore[10].text = "30";
        else m_userScore[10].text = "0";

        return score;
    }

    int CheckYacht()
    {
        int score = 50;  bool isTrue = false;
        if (m_scores[0] == m_scores[1] && m_scores[1] == m_scores[2] && m_scores[2] == m_scores[3] && m_scores[3] == m_scores[4]) isTrue = true;

        if(isTrue) m_userScore[11].text = "50";
        else m_userScore[11].text = "0";

        return score;
    }

    void CheckScore()
    {
        if (!m_aces) CheckAces();
        if (!m_deuces) CheckDeuces();
        if (!m_threes) CheckThrees();
        if (!m_fours) CheckFours();
        if (!m_fives) CheckFives();
        if (!m_sixes) CheckSixes();
        if (!m_choice) CheckChoice();
        if (!m_fourofakind) CheckFourOfAKind();
        if (!m_fullhouse) CheckFullHouse();
        if (!m_sstraight) CheckSmallStraight();
        if (!m_lstraight) CheckLargeStraight();
        if (!m_yacht) CheckYacht();
    }

    void ClickField()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Reset ray with new mouse position
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(m_ray);

            for (int i = 0; i < hits.Length; i++)
            {
                switch(hits[i].collider.gameObject.name)
                {
                    case "Aces":
                        if (!m_aces) { total += int.Parse(m_userScore[0].text); CheckBonus(); ChooseField(0); m_aces = true; turns++; }
                        break;
                    case "Deuces":
                        if (!m_deuces) { total += int.Parse(m_userScore[1].text); CheckBonus(); ChooseField(1); m_deuces = true; turns++; }
                        break;
                    case "Threes":
                        if (!m_threes) { total += int.Parse(m_userScore[2].text); CheckBonus(); ChooseField(2); m_threes = true; turns++; }
                        break;
                    case "Fours":
                        if (!m_fours) { total += int.Parse(m_userScore[3].text); CheckBonus(); ChooseField(3); m_fours = true; turns++; }
                        break;
                    case "Fives":
                        if (!m_fives) { total += int.Parse(m_userScore[4].text); CheckBonus(); ChooseField(4); m_fives = true; turns++; }
                        break;
                    case "Sixes":
                        if (!m_sixes) { total += int.Parse(m_userScore[5].text); CheckBonus(); ChooseField(5); m_sixes = true; turns++; }
                        break;
                    case "Choice":
                        if (!m_choice) { total += int.Parse(m_userScore[6].text); CheckBonus(); ChooseField(6); m_choice = true; turns++; }
                        break;
                    case "4 of a Kind":
                        if (!m_fourofakind) { total += int.Parse(m_userScore[7].text); CheckBonus(); ChooseField(7); m_fourofakind = true; turns++; }
                        break;
                    case "Full House":
                        if (!m_fullhouse) { total += int.Parse(m_userScore[8].text); CheckBonus(); ChooseField(8); m_fullhouse = true; turns++; }
                        break;
                    case "S. Straight":
                        if (!m_sstraight) { total += int.Parse(m_userScore[9].text); CheckBonus(); ChooseField(9); m_sstraight = true; turns++; }
                        break;
                    case "L. Straight":
                        if (!m_lstraight) { total += int.Parse(m_userScore[10].text); CheckBonus(); ChooseField(10); m_lstraight = true; turns++; }
                        break;
                    case "Yacht":
                        if (!m_yacht) { total += int.Parse(m_userScore[11].text); CheckBonus(); ChooseField(11); m_yacht = true; turns++; }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void ChooseField(int index)
    {
        for(int i = 0; i < 12; i++) 
        {
            if (i != index)
            {
                switch(i)
                {
                    case 0:
                        if(!m_aces) m_userScore[i].text = "";
                        break;
                    case 1:
                        if (!m_deuces) m_userScore[i].text = "";
                        break;
                    case 2:
                        if(!m_threes) m_userScore[i].text = "";
                        break;
                    case 3:
                        if(!m_fours) m_userScore[i].text = "";
                        break;
                    case 4:
                        if(!m_fives) m_userScore[i].text = "";
                        break;
                    case 5:
                        if(!m_sixes) m_userScore[i].text = "";
                        break;
                    case 6:
                        if(!m_choice) m_userScore[i].text = "";
                        break;
                    case 7:
                        if(!m_fourofakind) m_userScore[i].text = "";
                        break;
                    case 8:
                        if(!m_fullhouse) m_userScore[i].text = "";
                        break;
                    case 9:
                        if(!m_sstraight) m_userScore[i].text = "";
                        break;
                    case 10:
                        if(!m_lstraight) m_userScore[i].text = "";
                        break;
                    case 11:
                        if(!m_yacht) m_userScore[i].text = "";
                        break;
                    default:
                        break;
                }
            }
        }
        // Show Highlight
        scoreUI.transform.GetChild(index).GetComponent<Image>().enabled = true;
    }

    void CheckBonus()
    {
        if(!m_bonusAdded)
        {
            int bonus = 0;

            if (m_aces) bonus += int.Parse(m_userScore[0].text);
            if (m_deuces) bonus += int.Parse(m_userScore[1].text);
            if (m_threes) bonus += int.Parse(m_userScore[2].text);
            if (m_fours) bonus += int.Parse(m_userScore[3].text);
            if (m_fives) bonus += int.Parse(m_userScore[4].text);
            if (m_sixes) bonus += int.Parse(m_userScore[5].text);

            if (bonus >= 63) { total += 35; bonusScore.text = "35"; m_bonusAdded = true; }
            
        }
        NextTurn();
    }

    void NextTurn()
    {
        for(int i = 0; i < 5; i++)
        {
            m_diceProperties[i].m_chosen = false;
            m_diceProperties[i].ResetDie();
        }
        m_choosingDice = false;     m_diceStatic = false;   m_numChosen = 0;    m_numRolls = 0;
        rerollButton.gameObject.SetActive(false);
        totalScore.text = total.ToString();
    }
}
