using UnityEngine;
using UnityEngine.UI;

public class TeamController : MonoBehaviour
{
    [SerializeField] [Header("返回按鈕")] Button ReturnBtn;

    [SerializeField] [Header("製作團隊 - 按鈕")] Button GroupMarkBtn;
    [SerializeField] [Header("製作團隊 - 物件")] GameObject GroupPage;
    [SerializeField] [Header("製作團隊 - Text")] Text GroupMarkText;

    [SerializeField] [Header("贊助商 - 按鈕")] Button SponsorMarkBtn;
    [SerializeField] [Header("贊助商 - 物件")] GameObject SponsorPage;
    [SerializeField] [Header("贊助商 - Text")] Text SponsorMarkText;

    [SerializeField] [Header("FunctionMenuCtrlr")] FunctionMenuCtrlr MenuCtrlr;

    void Start()
    {
        ReturnBtn.onClick.AddListener(SettingReturn);
        GroupMarkBtn.onClick.AddListener(() => { ShowSelectPage(0); });
        SponsorMarkBtn.onClick.AddListener(() => { ShowSelectPage(1); });

        ShowSelectPage(0);
    }

    void SettingReturn()
    {
        GameObject TeamView = gameObject.transform.GetChild(1).gameObject;
        TeamView.SetActive(false);

        if (MenuCtrlr != null)
        {
            MenuCtrlr.ShowAllBtn();
        }
    }

    void ShowSelectPage(int r_iPage)
    {
        HideAllPage();
        DefaultFontStyleAndSize();

        switch (r_iPage)
        {
            case 0:
                GroupPage.SetActive(true);
                GroupMarkText.fontStyle = FontStyle.Bold;
                GroupMarkText.fontSize = 48;
                break;
            case 1:
                SponsorPage.SetActive(true);
                SponsorMarkText.fontStyle = FontStyle.Bold;
                SponsorMarkText.fontSize = 48;
                break;
            default:
                break;
        }
    }

    void HideAllPage()
    {
        GroupPage.SetActive(false);
        SponsorPage.SetActive(false);
    }

    void DefaultFontStyleAndSize()
    {
        GroupMarkText.fontStyle = FontStyle.Normal;
        GroupMarkText.fontSize = 36;

        SponsorMarkText.fontStyle = FontStyle.Normal;
        SponsorMarkText.fontSize = 36;
    }
}