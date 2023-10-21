using Messaging;

public class Player
{
    private readonly int _playerNumber;
    public int PlayerNumber => _playerNumber;
    
    private int _score;

    public Player(int playerNumber)
    {
        _playerNumber = playerNumber;
    }

    public int Score
    {
        get => _score;

        private set
        {
            var oldScore = _score;
            _score = value;
            Messenger.Send(new PlayerScoreChanged(this, oldScore, _score));
        }
    }
    
    public void Init()
    {
        _score = 0;
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }
}