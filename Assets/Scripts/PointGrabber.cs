using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointGrabber : Grabber
{
    public LineRenderer laserPointer;
    public Material grabbablePointerMaterial;

    public InputActionProperty touchAction;
    public InputActionProperty grabAction;
    public InputActionProperty scrollAction;
    public InputActionProperty windowAction;

    public WIMWindow window;
    public Transform WIMBox;
    public Transform otherHand;

    private Vector3 pos;
    private float scale;
    private bool selecting = false;

    Material lineRendererMaterial;
    Transform grabPoint;
    Grabbable grabbedObject;
    Transform initialParent;

    // Start is called before the first frame update
    void Start()
    {
        laserPointer.enabled = false;
        lineRendererMaterial = laserPointer.material;

        grabPoint = new GameObject().transform;
        grabPoint.name = "Grab Point";
        grabPoint.parent = this.transform;
        grabbedObject = null;
        initialParent = null;

        grabAction.action.performed += Grab;
        grabAction.action.canceled += Release;

        touchAction.action.performed += TouchDown;
        touchAction.action.canceled += TouchUp;

        scrollAction.action.performed += Scroll;

        windowAction.action.performed += SetCenter;
        windowAction.action.canceled += ChangeWIM;
    }

    private void OnDestroy()
    {
        grabAction.action.performed -= Grab;
        grabAction.action.canceled -= Release;

        touchAction.action.performed -= TouchDown;
        touchAction.action.canceled -= TouchUp;

        scrollAction.action.performed -= Scroll;

        windowAction.action.performed -= SetCenter;
        windowAction.action.canceled -= ChangeWIM;
    }

    // Update is called once per frame
    void Update()
    {
        if (laserPointer.enabled)
        {
            laserPointer.SetPosition(1, new Vector3(0, 0, 100));
            laserPointer.material = lineRendererMaterial; 
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
            System.Array.Sort(hits, new myReverserClass());
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<Proxy>() && hit.collider.GetComponent<Proxy>().Grabbable(hit.point)) continue;
                laserPointer.SetPosition(1, new Vector3(0, 0, hit.distance));

                if (hit.collider.GetComponent<Grabbable>())
                {
                    laserPointer.material = grabbablePointerMaterial;
                }
                else
                {
                    laserPointer.material = lineRendererMaterial;
                }

                if (selecting)
                {
                    pos = hit.point;
                    scale = Mathf.Pow(2.0f * (transform.position - otherHand.position).magnitude, 3.0f);
                    WIMBox.position = pos;
                    WIMBox.localScale = new Vector3(scale, scale, scale);
                }
                
                break;
            }
        }
    }

    public void SetCenter(InputAction.CallbackContext context)
    {
        laserPointer.enabled = true;
        /*
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
        System.Array.Sort(hits, new myReverserClass());
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Proxy>() && hit.collider.GetComponent<Proxy>().Grabbable(hit.point)) continue;
            pos = hit.point;
            scale = Mathf.Pow(2.0f * (transform.position - otherHand.position).magnitude,3.0f);
            WIMBox.position = pos;
            WIMBox.localScale = new Vector3(scale, scale, scale);
            return;
        }
        */
        if (!WIMBox.gameObject.active)
        {
            WIMBox.gameObject.SetActive(true);
            selecting = true;
        }
        

    }

    public void ChangeWIM(InputAction.CallbackContext context)
    {
        if (selecting)
        {
            window.focus = pos;
            window.SetScale(1.0f / scale);
            selecting = false;
            WIMBox.gameObject.SetActive(false);
        }
        

    }

    public class myReverserClass : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit x, RaycastHit y)
        {
            return (int)(10000f * (x.distance - y.distance));
        }

        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
    }

    public override void Grab(InputAction.CallbackContext context)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
        System.Array.Sort(hits,new myReverserClass());
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Proxy>() && hit.collider.GetComponent<Proxy>().Grabbable(hit.point)) continue;
            if (hit.collider.GetComponent<Grabbable>())
            {
                grabPoint.localPosition = new Vector3(0, 0, hit.distance);

                if (hit.collider.GetComponent<Grabbable>().GetCurrentGrabber() != null)
                {
                    hit.collider.GetComponent<Grabbable>().GetCurrentGrabber().Release(new InputAction.CallbackContext());
                }
                if (hit.collider.GetComponent<Proxy>())
                {
                    hit.collider.GetComponent<Proxy>().Grab();
                }

                grabbedObject = hit.collider.GetComponent<Grabbable>();
                grabbedObject.SetCurrentGrabber(this);

                if (grabbedObject.GetComponent<Rigidbody>())
                {
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                }

                initialParent = grabbedObject.transform.parent;
                grabbedObject.transform.parent = grabPoint;
            }
            return;
        }
    }

    public override void Release(InputAction.CallbackContext context)
    {
        if (grabbedObject)
        {
            if (grabbedObject.GetComponent<Rigidbody>() && !grabbedObject.GetComponent<Proxy>())
            {
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            }

            if (grabbedObject.GetComponent<Proxy>())
            {
                grabbedObject.GetComponent<Proxy>().UnGrab();
            }

            grabbedObject.transform.parent = initialParent;
            grabbedObject = null;
        }
    }

    void TouchDown(InputAction.CallbackContext context)
    {
        laserPointer.enabled = true;
    }

    void TouchUp(InputAction.CallbackContext context)
    {
        if (!selecting) laserPointer.enabled = false;
    }

    void Scroll(InputAction.CallbackContext context)
    {
        if (grabbedObject)
        {
            float dist = grabPoint.localPosition.z;
            Vector2 joystick = context.ReadValue<Vector2>();
            dist += 3 * joystick.y * Time.deltaTime;
            if (dist < 0) dist = 0;
            grabPoint.localPosition = new Vector3(0, 0, dist);
        }
    }
}
