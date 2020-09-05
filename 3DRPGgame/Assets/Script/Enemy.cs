using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("怪物速度"), Range(0.1f, 3)]
    public float speed = 2.5f;
    [Header("攻擊力"), Range(35f, 50f)]
    public float attack = 40f;
    [Header("血量"), Range(30, 300)]
    public float hp = 200;
    [Header("怪物的經驗值"), Range(30, 100)]
    public float exp = 30;
    [Header("怪物攻擊停止距離"), Range(0.1f, 5)]
    public float distanceAttack = 1.5f;
    [Header("攻擊冷卻時間"), Range(0.1f, 5f)]
    public float cd = 2.5f;
    [Header("面向玩家的速度"), Range(0.1f, 5f)]
    public float turn = 5f;

    private NavMeshAgent nav;       //導覽代理器
    private Transform player;       //玩家
    private Animator ani;           //動畫控制器
    private float timer;             //計時器



    private void Awake()
    {
        ani = GetComponent<Animator>();                 //取得導覽代理器
        nav = GetComponent<NavMeshAgent>();             //取得元件
        nav.speed = speed;                              //設定速度
        nav.stoppingDistance = distanceAttack ;

        player = GameObject.Find("U醬").transform;       //取得玩家

        nav.SetDestination(player.position);                //避免一開始就偷打
    }

    private void Update()
    {
        ani.SetBool("Walk Forward", false);

        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.35f);
        Gizmos.DrawSphere(transform.position, distanceAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);

        if (other.name == "U醬")
        {
            float range = Random.Range(-10f, 10f);                          //隨機攻擊力 +-10
            other.GetComponent<Player>().Hit(attack + range,transform);     //對玩家造成傷害(攻擊力+隨機，變形)
        }
    }

    private void Move()
    {
        nav.SetDestination(player.position);                        //追蹤玩家座標

        if (nav.remainingDistance < distanceAttack) Attack();       //如果 剩餘距離 < 攻擊停止距離  攻擊
        else ani.SetBool("Walk Forward", true);                      // 設定移動動畫,導覽器,加速度,數值
    }

    private void Attack()
    {
        Quaternion look = Quaternion.LookRotation(player.position - transform.position);            //面相角度 看向角度(玩家座標 -自己座標)
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * turn);     //角度 = 插植 (角度,面相角度,速度)

        timer += Time.deltaTime;                                          //計時器累加

        if (timer >= cd)        
        {
            timer = 0;
            ani.SetTrigger("Stab Attack");

        }
        ani.SetBool("Walk Forward", false);                             //停止撥放走路動畫
    }
}
