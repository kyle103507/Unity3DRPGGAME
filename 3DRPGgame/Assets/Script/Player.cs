using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("速度"), Range(0, 1000)]
    public float speed = 1;
    [Header("旋轉速度"), Range(0, 1000)]
    public float turn = 1;


    [HideInInspector]       //在屬性面板上面隱藏

   
    public bool stop;        //停止不能移動


    [Header("傳送門:0 NPC ，1 怪蟲")]
    public Transform[] doors;
    [Header("介面區塊")]
    public Image barHp;
    public Image barMp;
    public Image barExp;

    private float attack = 10;
    private float hp = 100;
    private float maxHp = 100;
    private float mp = 50;
    private float exp;
    private int lv = 1;
    private Rigidbody rig;
    private Animator ani;
    private Transform cam;  //攝影機跟物件
    private NPC npc;
    #endregion

    #region 事件
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        cam = GameObject.Find("攝影機跟蹤").transform;

        npc = FindObjectOfType<NPC>();
    }

    private void FixedUpdate()
    {
        if (stop) return;       //如果 停止 跳出
        
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "骷顱頭") GetProp(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "傳送門-npc")
        {
            transform.position = doors[1].position;                         //傳送到 蟲子
            doors[0].GetComponent<CapsuleCollider>().enabled = false;       //關閉蟲子傳送門碰撞
            Invoke("OpenDoorZombie", 3);
        }

        if (other.name == "打怪傳送門")
        {
            transform.position = doors[1].position;                         //傳送到 NPC
            doors[1].GetComponent<CapsuleCollider>().enabled = false;       //關閉NPC 傳送門碰撞
            Invoke("OpenDoorNPC", 3);
        }
    }
    #endregion

    #region 方法

    private void OpenDoorNPC()
    {
        doors[0].GetComponent<CapsuleCollider>().enabled = true;

    }  private void OpenDoorZombie()
    {
        doors[1].GetComponent<CapsuleCollider>().enabled = true;
    }
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

    /// <summary>
    /// 取得道具
    /// </summary>
    /// <param name="prop">碰到的道具</param>
    private void GetProp(GameObject prop)
    {
        Destroy(prop);
        //播放音效 aud.PlayOneShot(soundProp);
        npc.UpdateTextMission();
            
    }

    /// <summary>
    /// 受傷:動畫、扣血、擊退
    /// </summary>
    /// <param name="damage">傷害值</param>
    public void Hit(float damage, Transform direction)                  //direction 方向
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");
        rig.AddForce(direction.forward * 100 + direction.up * 10);      //擊退朝怪物前與上方

        hp = Mathf.Clamp(hp, 0, 9999);                                  //夾住血量不要低於0
        barHp.fillAmount = hp / maxHp;                              //更新血條

        if (hp == 0) Dead();                                            //如果響亮等於零就死
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        ani.SetBool("死亡開關", true);                  //死亡動畫
        enabled = false;                                //關閉此腳本
    }

    private void Exp()
    {

    }


    #endregion
}
