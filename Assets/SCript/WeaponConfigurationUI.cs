using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponConfigurationUI : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI nameText;
    public int curIndex = 0;
    public BaseStat curStat;

    private void OnEnable()
    {
        GameEvents.OnShipChanged += getData;
    }

    private void OnDisable()
    {
        GameEvents.OnShipChanged -= getData;
    }

    void Start()
    {
        if (WeaponConfigurator.Instance != null && WeaponConfigurator.Instance.stat != null)
        {
            getData(WeaponConfigurator.Instance.stat);
        }
    }

    public void RefreshUI()
    {
        if (curStat == null) return;
        Image.sprite = curStat.sprite;
        nameText.text = curStat.name;
    }

    public void NextData()
    {
        var list = WeaponConfigurator.Instance.inventorySpaceShip;
        if (list == null || list.Count == 0) return;

        curIndex = (curIndex + 1) % list.Count;
        curStat = list[curIndex]; // Gán vào biến tạm trên UI

        RefreshUI(); // Vẽ lại giao diện xem trước
    }

    // Gán hàm này vào Event onClick của NÚT APPLY trên UI
    public void ApplySelection()
    {
        if (curStat == null) return;

        // Bấm Apply thì mới chính thức chốt hạ gửi xuống Core Logic
        //WeaponConfigurator.Instance.ChangeBaseStat(curStat);
        GameEvents.RequestChangeShip(curStat);
        Debug.Log($"Đã Apply con tàu: {curStat.name}");
    }
    public void getData(BaseStat x)
    {
        curStat = x;
        RefreshUI();
    }
    public void TurnOffPanel()
    {
        GameEvents.RequestChangeGameStates(States.Intro);
        GameEvents.RequestSpawnPlayer();
        this.gameObject.SetActive(false);  
    }
    public void TurnOnPanel()
    {
        this.gameObject.SetActive(true);
    }
}