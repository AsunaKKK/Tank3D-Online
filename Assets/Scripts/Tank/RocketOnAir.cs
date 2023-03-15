using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class RocketOnAir : NetworkBehaviour
{
    public float currenAmmo { get; set; }

    public NetworkObject bulletPower;

    public int playerNumber = 1;
    public Rigidbody powerRb;
    public Transform fireTransform;
    public Slider aimSlider;
    public AudioSource shootingAudio;
    public float minLaunchForce = 15f;
    public float maxLaunchForce = 30f;
    public float maxChargeTime = 0.75f;

    private string _fireButton;
    private float _currentLaunchForce;
    private float _chargeSpeed;

    private int maxAmmo = 1;
    //public float currenAmmo;

    [Networked]
    private bool _fired { get; set; }
    [Networked]
    private bool _isDown { get; set; }

    private void OnEnable()
    {
        _currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
        //currenAmmo = maxAmmo;
    }

    private void Start()
    {
        _fireButton = "Fire" + playerNumber;
        _chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        currenAmmo = maxAmmo;

    }
    /* // void Update //
    private void Update()
    {
        aimSlider.value = minLaunchForce;

        if (_currentLaunchForce >= maxLaunchForce && !_fired)
        {
            Debug.Log("111");
            _currentLaunchForce = maxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(_fireButton))
        {
            Debug.Log("222");
            _fired = false;
            _currentLaunchForce = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if (Input.GetButton(_fireButton) && !_fired)
        {
            Debug.Log("333");
            _currentLaunchForce += _chargeSpeed * Time.deltaTime;
            aimSlider.value = _currentLaunchForce;
        }
        else if (Input.GetButtonUp(_fireButton) && !_fired)
        {
            Debug.Log("444");
            Fire();
        }
    }
    */

    public override void FixedUpdateNetwork()
    {
        //messageText.text = "Big Shll use R x " + currenAmmo;
        if (currenAmmo <= 0f)
        {
            _fired = true;
            _isDown = false;
        }

        aimSlider.value = minLaunchForce;
        if (GetInput(out NetworkInputPrototype input))
        {
            if (_currentLaunchForce >= maxLaunchForce && !_fired)
            {
                Debug.Log("11s1ss");
                _currentLaunchForce = maxLaunchForce;
                FirePowerUp();
            }
            else if (input.IsDown(NetworkInputPrototype.BUTTON_ACTION1))
            {
                Debug.Log("33s3ss");
                _fired = false;
                _isDown = true;
                _currentLaunchForce = 20;
;
            }
            else if (input.IsUp(NetworkInputPrototype.BUTTON_ACTION1) && !_fired && _isDown)
            {
                _isDown = false;
                Debug.Log("44s4ss");
                FirePowerUp();
            }
        }
    }

    private void FirePowerUp()
    {
        _fired = true;
        //Rigidbody shellInstance = (Rigidbody)Instantiate(shellRb, fireTransform.position, fireTransform.rotation);
        //shellInstance.velocity = _currentLaunchForce * fireTransform.forward;

        Runner.Spawn(bulletPower, fireTransform.position, fireTransform.rotation, Object.InputAuthority, (runner, obj) =>
        {
            obj.GetComponent<ShellOnAir>().Inti(_currentLaunchForce * fireTransform.right);
        });
        currenAmmo--;
    }

    public void AddAmmoda(int Ammo)
    {
        if (currenAmmo + Ammo >= maxAmmo)
        {
            currenAmmo = maxAmmo;
        }
        else
        {
            currenAmmo += Ammo;
        }
    }

}