using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks; 

public class UIManager : MonoBehaviour
{
    public WaveManager waveManager;
    public GameManager gameManager;
    public TextMeshProUGUI mainAnnounce;
    private void OnEnable()
    {
        gameManager.OnChangedGameState += ClassifyState;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ChangeMainAnnounce(string text)
    {
        mainAnnounce.text = text;   
    }
    public void ClassifyState(States state)
    {
        if(state == States.Intro) { ChangeMainAnnounce("Ready For The Game!!!"); ClearAnnounce(4000); } 
        if(state == States.Playing) { ChangeMainAnnounce("Fighting!!!"); ClearAnnounce(4000); }
        if(state == States.Outro) { ChangeMainAnnounce("End!!!"); ClearAnnounce(3000); }
    }
    public async void ClearAnnounce(int timeDelay)
    {
        await Task.Delay(timeDelay);
        ChangeMainAnnounce("");
    }
}
