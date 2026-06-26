using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] StageManager stageManager;
    [SerializeField] PlayerStatus player;
    [SerializeField] Transform playerPosition;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text expText;
    

    private void Update()
    {
        

        

        hpText.text = $"HP : {player.CurrentHp}/ {player.MaxHp}";
        levelText.text = $"Level:{player.Level}";
        expText.text = $"Exp: {player.Exp}/{player.ExpToNext}";
        
        

    }
}