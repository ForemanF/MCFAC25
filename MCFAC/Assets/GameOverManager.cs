using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    GameObject game_over_menu;

    [SerializeField]
    GameObject you_win_text;

    [SerializeField]
    GameObject you_lose_text;

    Subscription<GameOverEvent> game_over_sub;

    // Start is called before the first frame update
    void Start()
    {
        game_over_sub = EventBus.Subscribe<GameOverEvent>(_OnGameOver);
        
    }

    void _OnGameOver(GameOverEvent e) {
        game_over_menu.SetActive(true);

        if(e.did_win) {
            you_win_text.SetActive(true);
            you_lose_text.SetActive(false);
        }
        else { 
            you_win_text.SetActive(false);
            you_lose_text.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
