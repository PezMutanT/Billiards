using TMPro;
using UnityEngine;

public class EndGamePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _player1ScoreText;
    [SerializeField] private TextMeshProUGUI _player2ScoreText;
    
    private GameManager _gameManager;
    
    public void Init(GameManager gameManager, int player1Score, int player2Score)
    {
        _gameManager = gameManager;

        if (player1Score > player2Score)
        {
            _titleText.text = "Player 1 won!";
        }
        else if (player2Score > player1Score)
        {
            _titleText.text = "Player 2 won!";
        }
        else
        {
            _titleText.text = "It's a tie. Well played.";
        }

        _player1ScoreText.text = $"{player1Score}";
        _player2ScoreText.text = $"{player2Score}";
    }

    public void OnPlayAgainButtonClicked()
    {
        _gameManager.ResetGame();
        Destroy(gameObject);
    }
}
