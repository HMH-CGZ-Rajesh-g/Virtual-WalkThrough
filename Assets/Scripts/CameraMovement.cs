using UnityEngine;

public class CameraMovement : MonoBehaviour {
  Vector2 mouseLook;
  Vector2 smoothV;
  public float sensitivity = 5.0f;
  public float smoothing = 2.0f;
  public bool mLock = true;
  float minFov = 15f;
  float maxFov = 90f;
  float sensitivityZoom = 10f;

  GameObject character;
  // Start is called before the first frame update
  void Start () {
    //Cursor.lockState = CursorLockMode.Locked;
    character = this.transform.parent.gameObject;
  }

  public void LinkSelection () {
    //Cursor.lockState = CursorLockMode.None;
    //Cursor.visible = true;
    //mLock = false;
  }

  // Update is called once per frame
  void Update () {
    if (Input.GetKeyDown (KeyCode.Escape)) {
      mLock = false;
      //Cursor.lockState = CursorLockMode.None;
      //Cursor.visible = true;
    } else if (Input.GetKey (KeyCode.Mouse1)) {
      mLock = true;
      //Cursor.lockState = CursorLockMode.Locked;
      //Cursor.visible = false;
    }


    if (Input.GetMouseButton(0)) {
      var md = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"))+ (Vector2)transform.parent.transform.rotation.eulerAngles;

            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;

            // Camera clamping
            transform.localRotation = Quaternion.AngleAxis (Mathf.Clamp (-mouseLook.y, -30f, 30f), Vector3.right);
      character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up);

            #region Scroll to Zoom
            float fov = Camera.main.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivityZoom;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
            #endregion
        }
  }
}