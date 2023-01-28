using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GateState { add,multi, subtrac}

public class Gate : MonoBehaviour
{

    [SerializeField] private GateState _gateState;
    public GateState GateState { get => _gateState; }

    [SerializeField] private int gateCount;
    public int GateCount { get => gateCount; }

    [SerializeField] private GameObject otherGate;

    [SerializeField] private Text gateText;


    

    public void gateClose()
    {

        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        if(otherGate != null)
        {
            otherGate.GetComponent<BoxCollider>().enabled = false;
        }
        


    }

    private void Awake()
    {
        gateText = transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>();

        if(_gateState == GateState.multi)
        {
            gateText.text = "X" + gateCount.ToString();
        }
        else if(_gateState == GateState.subtrac)
        {
            gateText.text = "-" + gateCount.ToString();
        }
        else
        {
            gateText.text = "+" + gateCount.ToString();
        }

    }




}
