using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("NPC資料")]
    public NPCData Data;
    [Header("對話區域")]
    public GameObject paneIDialog;
    [Header("名稱")]
    public Text textName;
    [Header("內容")]
    public Text textContent;

    /// <summary>
    /// 對話系統
    /// </summary>
    public void Dialog()
    {
        paneIDialog.SetActive(true);
        textName.text = name;
        textContent.text = Data.dialogs[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "U醬") Dialog();
        
    }
}
