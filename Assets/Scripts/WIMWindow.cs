using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIMWindow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject worldParent;
    private GameObject children;

    public float scale = .1f;
    public Vector3 focus = Vector3.zero;

    public float lastScale;
    private Vector3 lastFocus;

    private List<GameObject> refs = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in worldParent.GetComponentsInChildren<Transform>())
        {
            Proxy script = child.gameObject.AddComponent<Proxy>();
            script.targetpos = refs.Count;
            script.window = this;
            refs.Add(child.gameObject);

        }
        children = GameObject.Instantiate(worldParent);
        foreach (Transform child in worldParent.GetComponentsInChildren<Transform>())
        {
            Destroy(child.gameObject.GetComponent<Proxy>());
        }
        Destroy(children.GetComponent<Proxy>());
        children.transform.parent = transform;
        children.transform.localPosition = (children.transform.localPosition - focus) * scale;
        children.transform.localScale *= scale;
        foreach (Transform child in children.GetComponentsInChildren<Transform>()) {
            Rigidbody body = child.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.useGravity = false;
                body.isKinematic = true;
            }
            Proxy script = child.GetComponent<Proxy>();
            script.target = refs[script.targetpos];
            foreach (Collider col in child.GetComponents<Collider>())
            {
                col.isTrigger = true;
            }
            
        }
        //Rigidbody body = children.GetComponent<Rigidbody>();
        //body.useGravity = false;
        //body.isKinematic = true;

        lastScale = scale;
        lastFocus = focus;
    }

    // Update is called once per frame
    void Update()
    {
        if (scale == 0) return;
        children.transform.localScale *= scale/lastScale;
        children.transform.localPosition *= scale / lastScale;
        children.transform.localPosition += (lastFocus - focus) * scale;

        lastScale = scale;
        lastFocus = focus;
    }

    public void SetScale(float s)
    {
        scale = s;
        foreach (Proxy child in GameObject.FindObjectsOfType<Proxy>())
        {
            if (child.grabbed)
            {
                child.GetComponent<Proxy>().target.transform.localScale *=  lastScale / scale;
            }
        }
    }
}
