using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler; // call the notification
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration; //in minutes for now

    private int energy;
    private const string EnergyKey = "Energy"; //create key 
    private const string EnergyReadyKey = "EnergyReady";

    
    private void Start()
    {
        OnApplicationFocus(true); // call method to run
    }
    
    //Start only get called when you change scenes or when you open up the app
    private void OnApplicationFocus(bool hasFocus) //whenever we load the main menu scene
    {
        if(!hasFocus) { return; }

        CancelInvoke();

        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = $"High Score: {highScore}";

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if(energy == 0) //when player run out off energy
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if(energyReadyString == string.Empty) { return; } //check for error
            DateTime energyReady = DateTime.Parse(energyReadyString); //convert string to datetime, to access the built in comparing ability 
            
            if(DateTime.Now > energyReady) //Now is after energy ready time
            {
                energy = maxEnergy; //, therefore, restore energy to max
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else
            {
                playButton.interactable = false; //turn play botton off when energy is not ready
                Invoke(nameof(EnergyRecharged),(energyReady - DateTime.Now).Seconds);
            }
        }

        energyText.text = energy.ToString(); 
    }

    private void EnergyRecharged()
    {
        playButton.interactable = true; //interactable
        energy = maxEnergy; //, therefore, restore energy to max
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = energy.ToString(); 
    }
    public void Play()  
    {
        if (energy < 1){ return; } //if there's currently no energy, player can't play

        energy--; //cost one energy to play each round

        PlayerPrefs.SetInt(EnergyKey, energy); //Reset energy key with new number

        if(energy == 0) //when player run out off energy
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());

#if UNITY_ANDROID //Not Compiled unless it's android
            androidNotificationHandler.ScheduleNotification(energyReady);
#endif
        }  

        SceneManager.LoadScene(1);
    }
}
