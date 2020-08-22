using UnityEngine;

public class CameraContal : MonoBehaviour
{
    [Header("目標")]
    public Transform target;
    [Header("速度"), Range(0, 100)]
    public float speed = 1;
    [Header("旋轉速度"), Range(0, 100)]
    public float turn;
    [Header("上下角度限制")]
    public Vector2 limit = new Vector2(-30, 30);



    /// <summary>
    /// 滑鼠控制旋轉角度
    /// </summary>
    private Quaternion rot;





    /// <summary>
    /// 追蹤
    /// </summary>
    private void Track()
    {
        Vector3 posA = transform.position;                              //A點
        Vector3 posB = target.position;                                 //B點
        posA = Vector3.Lerp(posA, posB, Time.deltaTime * speed);        //A點 = Vector3.插值(A 點 ，B點，Time.deltaTime * speed);
        transform.position = posA;                                      //攝影機.座標 = A 點

        if (Input.GetMouseButton(1))
        {
            //旋轉
            rot.x += Input.GetAxis("Mouse Y") * turn;                          //取得滑鼠上下來控制 X  角度
            rot.y += -Input.GetAxis("Mouse X") * turn;                         //取得滑鼠左右來控制 Y 角度

            rot.x = Mathf.Clamp(rot.x, limit.x, limit.y);                       //限制 X 在 -30 與 30內

            transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);             //攝影機跟物件.角度 = 歐拉(X,Y,Z)    
        }
    }

    private void Awake()
    {
        Cursor.visible = false;      //指標.可視 = 否
    }


    private void LateUpdate()
    {

        Track();

    }

}


