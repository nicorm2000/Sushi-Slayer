using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [SerializeField] private WeaponOverheatUI weaponOverheatUI;
    public WeaponData weaponData;
    public float weaponOverheat;
    public float overheatIncreaseAmount;
    public float overheatDecreaseRate;
    public Image weaponBlack;
    public Image weaponRed;

    private float _currentOverheat = 0f;
    private float _timeBetweenShots;
    private bool _canShoot = true;
    private bool _overHeat = false;

    [SerializeField] private GameObject weaponUI;
    [SerializeField] private GameObject overHeatText;

    private void Start()
    {
        weaponOverheatUI.maxSliderOverheat = weaponOverheat;
    }

    private void Update()
    {
        if (!_overHeat && _canShoot && Input.GetMouseButton(0))
        {
            if (_currentOverheat < weaponOverheat)
            {
                ShootBullet();

                _currentOverheat += overheatIncreaseAmount;
                weaponOverheatUI.currentSliderOverheat -= 1;

                if (_currentOverheat >= weaponOverheat)
                {
                    StartCoroutine(ShootCooldown());
                }
            }
        }

        CheckShootWeapon();
        ManageOverheat();
        weaponOverheatUI.SetCurrentOverheat(_currentOverheat);
    }

    private void ShootBullet()
    {
        if (weaponData.isShootWeapon)
        {
            Instantiate(weaponData.bulletPrefab, transform.position, transform.rotation);
            _canShoot = false;
            _timeBetweenShots = 1f / weaponData.attackSpeed;
            Invoke(nameof(EnableShooting), _timeBetweenShots);
        }
    }

    private void EnableShooting()
    {
        _canShoot = true;
        weaponOverheatUI.currentSliderOverheat = weaponOverheatUI.maxSliderOverheat;
    }

    private void ManageOverheat()
    {
        if (_currentOverheat > 0f)
        {
            _currentOverheat -= overheatDecreaseRate * Time.deltaTime;

            if (_currentOverheat <= 0f)
            {
                _currentOverheat = 0f;
                _canShoot = true;
            }
        }
    }

    private IEnumerator ShootCooldown()
    {
        _overHeat = true;

        overHeatText.SetActive(true);

        while (_currentOverheat > 0f) 
        {
            yield return null;
        }

        overHeatText.SetActive(false);

        _overHeat = false;
    }

    public void CheckShootWeapon()
    {
        if (weaponData.isShootWeapon == true)
        {
            weaponOverheatUI.CheckTypeOfWeapon();
        }
    }
}