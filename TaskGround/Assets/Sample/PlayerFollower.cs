using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerFollower : MonoBehaviour
{
    public GameObject lookAt;

    public Transform cameraTransform;   // 操作するカメラ
    public float mouseSensitivity = 300.0f;  // マウス感度
    public float defaultDistance = 3.0f;
    public float defaultAngle = 30.0f;//degree
    public float defaultY = 0.0f;

    public float maxAngle = 80.0f;
    public float minAngle = 5.0f;

	[Tooltip("マウスや右スティックでカメラが動かせるかどうか")]
	public bool isConrtollable = true;

	[Space]
    public float y;    // カメラのY軸成分
	public float angle;
	public float distance;

    private Vector3 cameraOffset;

    private bool cursorIsLocked
    {
        get
        {
            return !Cursor.visible;
        }
        set
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            Cursor.visible = !value;
        }
    }

    public void ResetCamera()
    {
        y = 0;
        angle = 0;
    }

    void Start()
    {
        cursorIsLocked = true;
        y = defaultY;
        angle = defaultAngle;
        distance = defaultDistance;
    }

    void Awake()
    {
		
    }

    // 全ての処理が終わったとにカメラの位置を調整するためにLateUpdateにする
    void LateUpdate()
    {
        //Debug.Log("wieweiweiwewei");
        if (!Application.isEditor || Application.isPlaying) {
			//Debug.Log ("あれあれ");
			if (Input.GetButtonDown ("Fire1")) {
				cursorIsLocked = true;
			}

			if (Input.GetKeyDown ("escape")) {
				cursorIsLocked = false;
			}
		}

		//Debug.Log ("hoge");

		if (cursorIsLocked || (Application.isEditor && !Application.isPlaying)) {
			if (isConrtollable && (!Application.isEditor || Application.isPlaying)) {
				y += Input.GetAxis ("Mouse X") * Time.deltaTime * mouseSensitivity;
				angle -= Input.GetAxis ("Mouse Y") * Time.deltaTime * mouseSensitivity;
				float scroll = Input.GetAxis("Mouse ScrollWheel");
				distance += scroll * 4;
			}

			if (angle > maxAngle)
				angle = maxAngle;
			if (angle < minAngle)
				angle = minAngle;

			cameraOffset = new Vector3 (0, 0, -distance);
			cameraOffset = Quaternion.Euler (angle, y, 0) * cameraOffset;
			//Debug.Log ("foo");
		}

        Transform lookAtTransform = lookAt.GetComponent<Transform>();
        cameraTransform.position = lookAtTransform.position + cameraOffset;
        cameraTransform.LookAt(lookAt.GetComponent<Transform>().position);
		cameraTransform.RotateAround (cameraTransform.position, cameraTransform.right, -6);
    }

}