using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("NPC資料")]
    public NPCData data;
    [Header("對話區域")]
    public GameObject paneIDialog;
    [Header("名稱")]
    public Text textName;
    [Header("內容")]
    public Text textContent;
    [Header("打字速度"),Range(0.1f , 1)]
    public float printSpeed = 0.2f;
    [Header("打字音效")]
    public AudioClip soundPrint;
    [Header("任務區塊")]
    public RectTransform panelMission;
    [Header("任務數量")]
    public Text textMission;

    private AudioSource aud;
    private Animator ani;
    private Player player;

    public int count;

    /// <summary>
    /// 更新任務文字介面
    /// </summary>
    public void UpdateTextMission()
    {
        count++;
        textMission.text = count + " /" + data.count;
    }

    /// <summary>
    /// 對話系統
    /// </summary>
    public void Dialog()
    {
        paneIDialog.SetActive(true);
        textName.text = name;
        StartCoroutine(Print());
    }
    
    /// <summary>
    /// 取消對話
    /// </summary>
    private void CancleDialog()
    {
        paneIDialog.SetActive(false);
        ani.SetBool("說話開關", false);
    }
    /// <summary>
    /// 打字效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator Print()
    {
        AnimationControl();
        Missioning();

        player.stop = true;                             //不能動

        string dialog = data.dialogs[(int)data._NPCState];                //對話 = NPC 資料.對話第一段
        textContent.text = "";                          //清空

        for (int i = 0; i < dialog.Length; i++)         //跑對話第一個到最後一個字
        {
            textContent.text += dialog[i];              //對話內容.文字 += 對話[]
            aud.PlayOneShot(soundPrint, 0.5f);
            yield return new WaitForSeconds(printSpeed);
        }

        player.stop = false;                             //可以動     

        NoMission();
    }

    /// <summary>
    /// 位階任務狀態切換為任務進行中:對話後執行
    /// </summary>
    private void NoMission()
    {
        if (data._NPCState == NPCState.NoMission) data._NPCState = NPCState.Missioning;
        StartCoroutine(ShowMission());
    }

    private IEnumerator ShowMission()
    {
        //當任務區域.X 大於 280 就插植跑到 280
        while (panelMission.anchoredPosition.x > 280)
        {
            panelMission.anchoredPosition = Vector3.Lerp(panelMission.anchoredPosition, new Vector3(280, 160, 0), 10 * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// 任務進行中切換為任務完成:對話開始執行
    /// </summary>
    private void Missioning()
    {
        if (count >= data.count) data._NPCState = NPCState.Finish;
    }

    /// <summary>
    /// 動畫控制
    /// </summary>
    private void AnimationControl()
    {
        if (data._NPCState == NPCState.NoMission || data._NPCState == NPCState.Missioning)
            ani.SetBool("說話開關", true);
        else
            ani.SetTrigger("感謝開關");
    }

    private void Awake()
    {
        data._NPCState = NPCState.NoMission;                //遊戲開始時狀態為未接任務

        aud = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();

        player = FindObjectOfType<Player>();               //透過類型尋找物件 PS.僅現場景只有一個類型

    }

    // Enter 進入
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "U醬") Dialog();

    }

    //Exit 離開
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "U醬") CancleDialog();
        
    }
}
