using Messaging;
using UnityEngine;
using UnityEngine.UI;

public class ShootGauge : MonoBehaviour
{
    [SerializeField] private GlobalConfiguration _globalConfiguration;
    [SerializeField] private Image _progressBarMask;
    
    private bool _isCharging;

    public void Init()
    {
        _isCharging = false;
        
        Messenger.AddListener<ShootForceMagnitudeChanged>(OnShootForceMagnitudeChanged);
    }

    public void End()
    {
        Messenger.RemoveListener<ShootForceMagnitudeChanged>(OnShootForceMagnitudeChanged);
    }

    private void OnShootForceMagnitudeChanged(ShootForceMagnitudeChanged msg)
    {
        var progressBarFill = msg.NewValue / _globalConfiguration.MaxCueForceMagnitude;
        _progressBarMask.fillAmount = progressBarFill;
    }

    public void OnShootButtonClick()
    {
        if (_isCharging)
        {
            Shoot();
        }
        else
        {
            Charge();
        }
    }

    private void Charge()
    {
        _isCharging = true;
        Messenger.Send(new ShootChargingStarted());
    }

    private void Shoot()
    {
        _isCharging = false;
        Messenger.Send(new ShootChargingFinished());
    }
}
