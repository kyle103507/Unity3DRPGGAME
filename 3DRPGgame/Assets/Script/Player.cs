using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("速度"), Range(0, 1000)]
    public float speed = 1;
    [Header("旋轉速度"), Range(0, 1000)]
    public float turn = 1;

    private float attack = 10;
    private float hp = 100;
    private float mp = 50;
    private float exp;
    private int lv = 1;
    private Rigidbody rig;
    private Animator ani;
    private Transform cam;  //攝影機跟物件
    #endregion

    #region 事件
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        cam = GameObject.Find("攝影機跟蹤").transform;

    }

    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 移動方法:前後左右移動與動畫
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxis("Vertical");                                            //前後:w.S上下
        float h = Input.GetAxis("Horizontal");                                          //左右:AD上下 
        Vector3 pos = cam.forward * v + cam.right * h;                                  //移動座標(左右,0，前後)
        rig.MovePosition(transform.position + pos * speed);                            //移動座標(原本座標 +移動座標 + 速度)

        ani.SetFloat("移動", Mathf.Abs(v) + Mathf.Abs(h));                             //設定浮點數(絕對值V 與 H)

        if (v != 0 || h != 0)                                                          //如果控制中                 
        {
            pos.y = 0;
            Quaternion angle = Quaternion.LookRotation(pos);                            //B 角度 = 面向(移動座標)
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, turn);     //A 角度 = 腳次.插值(A 角度 , B角度 , 旋轉速度)
        }
    }

    private void Attack()
    {

    }

    private void Skill()
    {

    }

    private void GetProp()
    {

    }

    private void Hit()
    {

    }

    private void Dead()
    {

    }

    private void Exp()
    {

    }


    #endregion
}
