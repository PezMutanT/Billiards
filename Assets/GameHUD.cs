using Messaging;
using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _player1ScoreText;
    [SerializeField] private TextMeshProUGUI _player2ScoreText;

    public void Init()
    {
        _player1ScoreText.text = "0";
        _player2ScoreText.text = "0";
        
        Messenger.AddListener<PlayerScoreChanged>(OnPlayerScoreChanged);
    }

    public void End()
    {
        Messenger.RemoveListener<PlayerScoreChanged>(OnPlayerScoreChanged);
    }

    private void OnPlayerScoreChanged(PlayerScoreChanged msg)
    {
        if (msg.Player.PlayerNumber == 1)
        {
            _player1ScoreText.text = msg.NewScore.ToString();
        }
        else if (msg.Player.PlayerNumber == 2)
        {
            _player2ScoreText.text = msg.NewScore.ToString();
        }
    }
}
