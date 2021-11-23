using cdb.virtualwalkthrough;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    CharacterController controller;
    CameraMovement cameraM;
    Vector3 presentVector;
    bool canRotate = false;
    float smoooth = 1;




    public float speed;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraM = transform.GetChild(0).GetComponent<CameraMovement>();
        VirtualwalkThrough.FireAssignvideoCamera(transform.GetChild(0).GetComponent<Camera>());
        // locator.localPosition = entLocation;
    }

    void Update()
    {
        if (GlobalConstants.isAdmin)
        {
            Vector2 movement = new Vector2(0, Input.GetAxis("Vertical")) + new Vector2(GlobalConstants.movement.Horizontal, GlobalConstants.movement.Vertical);
            Vector2 rotation = (new Vector2(Input.GetAxis("Horizontal"), 0) + new Vector2(GlobalConstants.rotation.Horizontal, GlobalConstants.rotation.Vertical)) * -speed * 10;


            if (transform.position.y != 1)
            {
                Vector3 pos = transform.position;
                pos.y = 1;
                transform.position = pos;
            }

            if (movement.x != 0 || movement.y != 0)
            { 
            Vector3 move = transform.forward * movement.y + transform.right * movement.x;

            controller.Move(move * speed * Time.deltaTime);
        }

            //transform.Rotate(Vector3.up * speed * 3 * movement);
            
            RotateCharacter(rotation.x, rotation.y);

#if UNITY_WEBGL

            if (Input.GetMouseButtonDown(0))
            {
                presentVector = Input.mousePosition;
                canRotate = true;

            }
            if (canRotate)
            {
                float Xvalue = presentVector.x - Input.mousePosition.x;
                float Yvalue = presentVector.y - Input.mousePosition.y;

                RotateCharacter(Xvalue, Yvalue);

                presentVector = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                presentVector = Input.mousePosition;
                canRotate = false;

            }
#endif
        }

    }

    private void RotateCharacter(float Xvalue, float Yvalue)
    {
        transform.Rotate(Vector3.down * smoooth * Xvalue * Time.deltaTime);
        float value = smoooth * Yvalue * Time.deltaTime;

        Vector3 rot = transform.GetChild(0).eulerAngles + Vector3.right * value;
        // Debug.Log(rot.x);
        rot.x = ClampAngle(rot.x, -30, 30);
        transform.GetChild(0).eulerAngles = rot;
    }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
   
    public void SetPoision(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger is" + other.name);
        VirtualwalkThrough.FireCollisisonTrigger(other.name);
    }

    //public void RelasePosition()
    //{
    //    cameraM.mLock = true;
    //}
}

