using UnityEngine;

public class SpwnManger : MonoBehaviour
{
    [Header("怪物")]
    public Transform enemy;
    [Header("生成點")]
    public GameObject[] points;
    [Header("間隔時間"),Range(0f,5f)]
    public float interval = 2f;


    private void Start()
    {
        points = GameObject.FindGameObjectsWithTag("生存點");      //透過標籤尋找物件們

        InvokeRepeating("Spawn", 0, interval);                      //重複呼叫("方法名稱" ,延遲時間 , 重複頻率)
    }

    private void Spawn()
    {
        int r = Random.Range(0, points.Length);                     //隨機
        Transform point = points[r].transform;                      //儲存生存點
        Instantiate(enemy, point.position, point.rotation);         //生成(物件,座標,角度)
    }
}
