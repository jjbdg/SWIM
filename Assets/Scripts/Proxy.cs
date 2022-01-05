using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proxy : MonoBehaviour
{
    // Start is called before the first frame update
    public Shader shader;
    public GameObject target;
    public int targetpos;
    public WIMWindow window;

    private Vector3 lastPos;
    private Vector3 lastScale;

    private Vector3 mylastPos;
    private Vector3 mylastScale;

    public bool grabbed = false;
    void Start()
    {
        lastPos = target.transform.position;
        lastScale = target.transform.lossyScale;
        //if (!gameObject.GetComponent<Material>())
        shader = Shader.Find("MiniObject");
        gameObject.GetComponent<Renderer>().material.shader = shader;
    }

    // Update is called once per frame
    void Update()
    {
        float rad = window.transform.localScale.x * .5f;
        gameObject.GetComponent<Renderer>().material.SetVector("_WIMPos", window.transform.position);
        gameObject.GetComponent<Renderer>().material.SetFloat("_WIMRad", rad);
        //transform.localPosition = target.transform.localPosition;
        //transform.localRotation = target.transform.localRotation;
        Vector3 dst = transform.position - window.transform.position;
        
        //transform.localScale = target.transform.localScale;
        if (grabbed)
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_WIMRad", 100f);
            target.transform.position = (transform.position - window.transform.position) / window.scale + window.focus;
            target.transform.rotation = transform.rotation;
            target.transform.localScale *= transform.lossyScale.magnitude / mylastScale.magnitude;
        }
        else
        {
            
            transform.position = (target.transform.position - (window.focus)) * window.scale + window.transform.position;
            transform.rotation = target.transform.rotation;
            transform.localScale *= target.transform.lossyScale.magnitude / lastScale.magnitude;
        }

        lastPos = target.transform.position;
        lastScale = target.transform.lossyScale;

        mylastPos = transform.position;
        mylastScale = transform.lossyScale;
    }

    public void Grab()
    {
        grabbed = true;
        if (target.GetComponent<Rigidbody>())
        {
            target.GetComponent<Rigidbody>().isKinematic = true;
            target.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void UnGrab()
    {
        grabbed = false;
        if (target.GetComponent<Rigidbody>())
        {
            target.GetComponent<Rigidbody>().isKinematic = false;
            target.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public bool Grabbable(Vector3 pos)
    {
        if (grabbed) return false;
        float rad = window.transform.localScale.x * .5f;
        Vector3 dst = pos - window.transform.position;
        return (Mathf.Abs(dst.x) > rad || Mathf.Abs(dst.y) > rad || Mathf.Abs(dst.z) > rad);
    }
}
