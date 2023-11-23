using Messaging;
using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _player1ScoreText;
    [SerializeField] private TextMeshProUGUI _player2ScoreText;
    [SerializeField] private GameObject _player1CurrentPlayerMarker;
    [SerializeField] private GameObject _player2CurrentPlayerMarker;
    [SerializeField] private ShootGauge _shootGauge;
    
    public void Init()
    {
        _shootGauge.Init();
        
        _player1CurrentPlayerMarker.SetActive(false);
        _player2CurrentPlayerMarker.SetActive(false);
        
        _player1ScoreText.text = "0";
        _player2ScoreText.text = "0";
        
        Messenger.AddListener<PlayerChanged>(OnPlayerChanged);
        Messenger.AddListener<PlayerScoreChanged>(OnPlayerScoreChanged);
    }

    public void End()
    {
        Messenger.RemoveListener<PlayerScoreChanged>(OnPlayerScoreChanged);
        Messenger.RemoveListener<PlayerChanged>(OnPlayerChanged);

        _shootGauge.End();
    }

    private void OnPlayerChanged(PlayerChanged msg)
    {
        _player1CurrentPlayerMarker.SetActive(msg.CurrentPlayer.PlayerNumber == 1);
        _player2CurrentPlayerMarker.SetActive(msg.CurrentPlayer.PlayerNumber == 2);
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
