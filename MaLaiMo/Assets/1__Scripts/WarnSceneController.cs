using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WarnSceneController : MonoBehaviour
{
    public bool bLoadSceneAsync;
    public int iNextSceneID;

    [SerializeField] Button Btn_confirmGame;

    AsyncOperation async = null;
    public GameObject TineLineAniObj;
    public Animation aniSubtitle_1;
    public Animation aniSubtitle_2;

    [SerializeField] [Header("警告提示 UI")] GameObject WariningPanel;
    [SerializeField] [Header("前情提要 UI")] GameObject IntroducingPanel;
    [SerializeField] [Header("Hint text")] Text HintText;

    bool bIsShowWarning = true;
    bool bIsGameIntroducing = false;

    void Start()
    {
        bIsShowWarning = true;
        bIsGameIntroducing = false;
    }

    void Update()
    {
        if (!bIsShowWarning && bIsGameIntroducing && Input.GetKeyDown(KeyCode.Space))
            LoadScene(iNextSceneID);

        if (bIsShowWarning && Input.GetKeyDown(KeyCode.Space))
            ShowIntroducePanel();
    }

    void LoadScene(int r_iSceneIndex)
    {
        SceneManager.LoadScene(r_iSceneIndex);
    }

    void ShowIntroducePanel()
    {
        bIsShowWarning = false;
        bIsGameIntroducing = true;
        HintText.enabled = false;
        WariningPanel.SetActive(false);
        IntroducingPanel.SetActive(true);

        Invoke(nameof(DelayShowHint), 6.5f);
    }

    void DelayShowHint()
    {
        HintText.text = "按下 *空白鍵* 繼續";
        HintText.enabled = true;
    }
}