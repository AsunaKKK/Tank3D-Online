using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class TankShooting : NetworkBehaviour
{
    public NetworkObject bullet;
    public NetworkObject bulletPower;
    public int playerNumber = 1;
    public Rigidbody shellRb;
    public Rigidbody powerRb;
    public Transform fireTransform;
    public Slider aimSlider;
    public AudioSource shootingAudio;
    public AudioClip chargingClip;
    public AudioClip fireClip;
    public float minLaunchForce = 15f;
    public float maxLaunchForce = 30f;
    public float maxChargeTime = 0.75f;

    private string _fireButton;
    private float _currentLaunchForce;
    private float _chargeSpeed;

    [Networked]
    private bool _fired { get; set; }
    [Networked]
    private bool _isDown { get; set; }

    private void OnEnable()
    {
        _currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }

    private void Start()
    {
        _fireButton = "Fire" + playerNumber;
        _chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
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
        aimSlider.value = minLaunchForce;
        if (GetInput(out NetworkInputPrototype input))
        {
            if (_currentLaunchForce >= maxLaunchForce && !_fired)
            {
                Debug.Log("111");
                _currentLaunchForce = maxLaunchForce;
                Fire();
            }
            else if (input.IsDown(NetworkInputPrototype.BUTTON_JUMP) && !input.IsUp(NetworkInputPrototype.BUTTON_JUMP))
            {
                Debug.Log("222");
                _fired = false;
                _isDown = true;
                _currentLaunchForce += _chargeSpeed * Runner.DeltaTime;
                aimSlider.value = _currentLaunchForce;
                //shootingAudio.clip = chargingClip;
                //shootingAudio.Play();
            }
            else if (input.IsDown(NetworkInputPrototype.BUTTON_JUMP))
            {
                Debug.Log("333");
                _fired = false;
                _isDown = true;
                _currentLaunchForce = minLaunchForce;
                shootingAudio.clip = chargingClip;
                shootingAudio.Play();
            }
            else if (input.IsUp(NetworkInputPrototype.BUTTON_JUMP) && !_fired && _isDown)
            {
                _isDown = false;
                Debug.Log("444");
                Fire();
            }
        }

       
    }
    private void Fire()
    {
        _fired = true;
        //Rigidbody shellInstance = (Rigidbody)Instantiate(shellRb, fireTransform.position, fireTransform.rotation);
        //shellInstance.velocity = _currentLaunchForce * fireTransform.forward;

        Runner.Spawn(bullet, fireTransform.position, fireTransform.rotation, Object.InputAuthority, (runner, obj) =>
        {
            obj.GetComponent<ShellExplosion>().Inti(_currentLaunchForce * fireTransform.forward);
        });

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        _currentLaunchForce = minLaunchForce;
    }
}