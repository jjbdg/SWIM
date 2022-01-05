using UnityEngine;
using UnityEngine.InputSystem;

public class GraspGrabber : Grabber
{
    public InputActionProperty grabAction;
    public InputActionProperty toggleGogo;

    public static int gogo = 2;
    public float gogoThreshold = 0.5f;

    public GameObject spindle;

    Grabbable currentObject;
    Grabbable grabbedObject;

    GameObject hmd;

    static Grabber both;
    public GraspGrabber otherHand;
    private Transform initialParent;

    // Start is called before the first frame update
    void Start()
    {
        grabbedObject = null;
        currentObject = null;

        grabAction.action.performed += Grab;
        grabAction.action.canceled += Release;

        toggleGogo.action.performed += ToggleGogo;

        //spindle.SetActive(false);

        hmd = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnDestroy()
    {
        grabAction.action.performed -= Grab;
        grabAction.action.canceled -= Release;

        toggleGogo.action.performed -= ToggleGogo;
    }

    // Update is called once per frame
    void Update()
    {
        if (gogo == 0)
        {
            Vector3 pos = hmd.transform.position - this.gameObject.transform.parent.position;
            if (pos.magnitude > gogoThreshold)
            {
                float distance = pos.magnitude - gogoThreshold;
                this.gameObject.transform.position = this.gameObject.transform.parent.position - pos.normalized * distance * distance * 100;
            }
            else
            {
                this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        } 
        else
        {
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void ToggleGogo(InputAction.CallbackContext context) // do nothing, lock to one mode only
    {
        //gogo = (gogo+1)%3;
        //spindle.SetActive(gogo!=0);
    }

    public override void Grab(InputAction.CallbackContext context)
    {
        if (currentObject && grabbedObject == null)
        {
            grabbedObject = currentObject;

            if (gogo == 0)
            {
                if (currentObject.GetCurrentGrabber() != null)
                {
                    currentObject.GetCurrentGrabber().Release(new InputAction.CallbackContext());
                }
                if (currentObject.GetComponent<Proxy>())
                {
                    currentObject.GetComponent<Proxy>().Grab();
                }
                if (grabbedObject.GetComponent<Rigidbody>())
                {
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                }
                grabbedObject.SetCurrentGrabber(this);
                initialParent = grabbedObject.transform.parent;
                grabbedObject.transform.parent = this.transform;
            }
            else if (gogo == 1)
            {
                if (currentObject.GetCurrentGrabber() != null)
                {
                    currentObject.GetCurrentGrabber().Release(new InputAction.CallbackContext());
                }
                if (currentObject.GetComponent<Proxy>())
                {
                    currentObject.GetComponent<Proxy>().Grab();
                }
                if (grabbedObject.GetComponent<Rigidbody>())
                {
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                }
                grabbedObject.SetCurrentGrabber(this);
                initialParent = grabbedObject.transform.parent;
                grabbedObject.transform.parent = spindle.transform;
                grabbedObject.transform.localPosition = new Vector3(0, 0, 0);
            }
            else // gogo is 2
            {
                if (grabbedObject.GetCurrentGrabber() != otherHand)
                {
                    if (currentObject.GetCurrentGrabber() != null)
                    {
                        currentObject.GetCurrentGrabber().Release(new InputAction.CallbackContext());
                    }
                    if (currentObject.GetComponent<Proxy>())
                    {
                        currentObject.GetComponent<Proxy>().Grab();
                    }
                    if (grabbedObject.GetComponent<Rigidbody>())
                    {
                        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                        grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                    }
                    grabbedObject.SetCurrentGrabber(this);
                    initialParent = grabbedObject.transform.parent;
                    grabbedObject.transform.parent = this.transform;
                }
                else
                {
                    grabbedObject.SetCurrentGrabber(both);
                    grabbedObject.transform.parent = spindle.transform;
                }
            }
        }
    }

    public override void Release(InputAction.CallbackContext context)
    {
        if (grabbedObject)
        {
            if (grabbedObject.GetCurrentGrabber() == both)
            {
                grabbedObject.SetCurrentGrabber(otherHand);
                grabbedObject.transform.parent = otherHand.transform;
                grabbedObject = null;
            }
            else
            {
                if (grabbedObject.GetComponent<Rigidbody>() && !currentObject.GetComponent<Proxy>())
                {
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                    grabbedObject.GetComponent<Rigidbody>().useGravity = true;
                }

                grabbedObject.SetCurrentGrabber(null);
                grabbedObject.transform.parent = initialParent;
                if (currentObject.GetComponent<Proxy>())
                {
                    currentObject.GetComponent<Proxy>().UnGrab();
                }
                grabbedObject = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentObject == null && other.GetComponent<Grabbable>())
        {
            if (other.GetComponent<Proxy>() && other.GetComponent<Proxy>().Grabbable(transform.position)) return;
            currentObject = other.gameObject.GetComponent<Grabbable>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentObject)
        {
            if (other.GetComponent<Grabbable>() && currentObject.GetInstanceID() == other.GetComponent<Grabbable>().GetInstanceID())
            {
                currentObject = null;
            }
        }
    }
}
