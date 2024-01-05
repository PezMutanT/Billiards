using System.Collections.Generic;
using Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _player1ScoreText;
    [SerializeField] private TextMeshProUGUI _player2ScoreText;
    [SerializeField] private AnimatedScoreText _player1AnimatedScoreText;
    [SerializeField] private AnimatedScoreText _player2AnimatedScoreText;
    [SerializeField] private CurrentPlayerMarker _player1CurrentPlayerMarker;
    [SerializeField] private CurrentPlayerMarker _player2CurrentPlayerMarker;
    [SerializeField] private ShootGauge _shootGauge;
    [SerializeField] private Image _nextBallOnSingleColorSprite;
    [SerializeField] private Image _nextBallOnMulticolorSprite;
    [SerializeField] private GameObject _confirmationPopupPrefab;
    
    public void Init(List<BallType> allowedBallTypes)
    {
        _shootGauge.Init();
        
        _player1CurrentPlayerMarker.Activate();
        _player2CurrentPlayerMarker.Deactivate();

        _player1AnimatedScoreText.Init();
        _player2AnimatedScoreText.Init();
        
        _player1ScoreText.text = "0";
        _player2ScoreText.text = "0";
        
        Messenger.AddListener<PlayerChanged>(OnPlayerChanged);
        Messenger.AddListener<PlayerScoreChanged>(OnPlayerScoreChanged);
        
        RefreshBallOnSprite(allowedBallTypes);
    }

    public void End()
    {
        Messenger.RemoveListener<PlayerScoreChanged>(OnPlayerScoreChanged);
        Messenger.RemoveListener<PlayerChanged>(OnPlayerChanged);

        _shootGauge.End();
    }

    private void OnPlayerChanged(PlayerChanged msg)
    {
        var isPlayer1Turn = msg.CurrentPlayer.PlayerNumber == 1;
        if (isPlayer1Turn)
        {
            _player1CurrentPlayerMarker.Activate();
            _player2CurrentPlayerMarker.Deactivate();
        }
        else
        {
            _player1CurrentPlayerMarker.Deactivate();
            _player2CurrentPlayerMarker.Activate();   
        }
    }

    private void RefreshBallOnSprite(List<BallType> allowedBallTypes)
    {
        if (allowedBallTypes.Count == 0)
        {
            Debug.LogError($"Allowed ball types list is empty.");
            return;
        }
        
        if (allowedBallTypes.Count > 1)
        {
            _nextBallOnSingleColorSprite.gameObject.SetActive(false);
            _nextBallOnMulticolorSprite.gameObject.SetActive(true);
            return;
        }
        
        _nextBallOnSingleColorSprite.gameObject.SetActive(true);
        _nextBallOnMulticolorSprite.gameObject.SetActive(false);

        var nextBallOnType = allowedBallTypes[0];
        Color spriteColor = Color.magenta;
        switch (nextBallOnType)
        {
            case BallType.Red:
                spriteColor = Color.red;
                break;
            case BallType.Yellow:
                spriteColor = Color.yellow;
                break;
            case BallType.Green:
                spriteColor = Color.green;
                break;
            case BallType.Brown:
                spriteColor = new Color(0.4716981f, 0.2413695f, 0.09122463f);
                break;
            case BallType.Blue:
                spriteColor = Color.blue;
                break;
            case BallType.Pink:
                spriteColor = new Color(0.990566f, 0.630785f, 0.8885943f);
                break;
            case BallType.Black:
                spriteColor = Color.black;
                break;
            default:
                Debug.LogError($"BallType not supported by sprite.");
                break;
        }

        _nextBallOnSingleColorSprite.color = spriteColor;
    }

    private void OnPlayerScoreChanged(PlayerScoreChanged msg)
    {
        if (msg.Player.PlayerNumber == 1)
        {
            _player1AnimatedScoreText.AnimateScoreText(msg.NewScore - msg.OldScore);
            _player1ScoreText.text = msg.NewScore.ToString();
        }
        else if (msg.Player.PlayerNumber == 2)
        {
            _player2AnimatedScoreText.AnimateScoreText(msg.NewScore - msg.OldScore);
            _player2ScoreText.text = msg.NewScore.ToString();
        }
    }

    public void StartNewTurn(List<BallType> allowedBallTypes)
    {
        RefreshBallOnSprite(allowedBallTypes);
        _shootGauge.StartNewTurn();
    }

    public void OnSettingsButtonClicked()
    {
        var popupGameObject = Instantiate(_confirmationPopupPrefab, transform);
        var confirmationPopup = popupGameObject.GetComponent<ConfirmationPopup>();
        confirmationPopup.Init(_gameManager);
    }
}
