using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameObject diceObj;

    List<Transform> m_dice;
    // Start is called before the first frame update
    void Start()
    {
        m_dice = new List<Transform>();
        for (int i = 0; i < 5; i++) m_dice.Add(diceObj.transform.GetChild(i));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    void IdentifyNumber(GameObject die)
    {
        
    }
}
