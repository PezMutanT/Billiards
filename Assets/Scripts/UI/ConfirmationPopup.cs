using UnityEngine;

public class ConfirmationPopup : MonoBehaviour
{
    private GameManager _gameManager;
    
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnYesButtonClicked()
    {
        _gameManager.ResetGame();
        Destroy(gameObject);
    }

    public void OnNoButtonClicked()
    {
        Destroy(gameObject);
    }
}
