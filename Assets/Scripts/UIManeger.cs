using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour {
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Text scoreText;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //ScoreのUpdate
    void UpdateScore(int score) {
        scoreText.text = "Score : " + score.ToString();

    }

    //PlayerのHP/MaxHPを代入
    void UpdateHealthBar(int playerHealth,int maxHealth) {
        healthSlider.value = playerHealth / maxHealth;
    }

}
