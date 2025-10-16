using System.Collections.Generic;
using UnityEngine;
using Characters.PlayerComponents;

namespace Weapons.Handlers
{
    public class WeaponHandler : MonoBehaviour
    {
        private const int SHIFT_INDEX = 1;

        [SerializeField] private List<Weapon> _weapons = new List<Weapon>();
        [SerializeField] private PlayerInput _playerInput;

        private int _currentWeaponIndex = 0;

        private void OnEnable()
        {
            _playerInput.ShootPerformed += OnShootPerformed;
            _playerInput.SwitchWeaponNext += NextWeapon;
            _playerInput.SwitchWeaponPrevious += PreviousWeapon;

            InitializeWeapons();
        }

        private void OnDisable()
        {
            _playerInput.ShootPerformed -= OnShootPerformed;
            _playerInput.SwitchWeaponNext -= NextWeapon;
            _playerInput.SwitchWeaponPrevious -= PreviousWeapon;
        }

        private void InitializeWeapons()
        {
            for (int i = 0; i < _weapons.Count; i++)
                if (_weapons[i] != null)
                    _weapons[i].gameObject.SetActive(i == _currentWeaponIndex);

            if (_weapons.Count > 0 && _weapons[_currentWeaponIndex] != null)
                _weapons[_currentWeaponIndex].gameObject.SetActive(true);
        }

        private void ActivateWeapon(int index)
        {
            if (index < 0 || index >= _weapons.Count)
                return;

            if (_currentWeaponIndex >= 0 && _currentWeaponIndex < _weapons.Count)
                _weapons[_currentWeaponIndex].gameObject.SetActive(false);

            _currentWeaponIndex = index;
            _weapons[_currentWeaponIndex].gameObject.SetActive(true);
        }

        private void OnShootPerformed()
        {
            if (_weapons.Count > 0 && _weapons[_currentWeaponIndex] != null)
            {
                Vector3 fireDirection = _weapons[_currentWeaponIndex].transform.forward;
                _weapons[_currentWeaponIndex].Fire(fireDirection);
            }
        }

        private void NextWeapon()
        {
            int nextIndex = (_currentWeaponIndex + SHIFT_INDEX) % _weapons.Count;
            ActivateWeapon(nextIndex);
        }

        private void PreviousWeapon()
        {
            int prevIndex = (_currentWeaponIndex - SHIFT_INDEX + _weapons.Count) % _weapons.Count;
            ActivateWeapon(prevIndex);
        }
    }
}