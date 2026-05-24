using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        if(state == States.Intro) { ChangeMainAnnounce("Ready For The Game!!!"); } 
        if(state == States.Playing) { ChangeMainAnnounce(""); }
        if(state == States.Outro) { }
    }
    
}
