using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spindle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftHand;
    public GameObject rightHand;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = (leftHand.transform.position + rightHand.transform.position) / 2;
        this.transform.up = leftHand.transform.position - this.transform.position;
        float scaleFactor = (leftHand.transform.position - this.transform.position).magnitude;
        this.transform.localScale = new Vector3(.2f * scaleFactor, .2f * scaleFactor, .2f * scaleFactor);
        //this.transform.up = Vector3.Cross(leftHand.transform.position - this.transform.position, rightHand.transform.position - this.transform.position);
    }
}
