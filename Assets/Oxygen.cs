using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Oxygen : MonoBehaviour
{
	public event EventHandler OnOxygenChanged;

	[SerializeField]
    public float _currentOxygen;
	[SerializeField]
	private float _maxOxygen;
	[SerializeField]
	private float _minOxygen;
	[SerializeField]
	private Health _playerHealthScript;
	[SerializeField]
	private float _breathTime;
	[SerializeField]
	private float _breathAmmount;
	[SerializeField]
	private int _sufficationDamage;
	[SerializeField]
	private AudioSource _breathingAudioSource;

	[SerializeField]
	private GameObject _ChokeEffect;



	private void Start()
	{
		_playerHealthScript = this.gameObject.GetComponent<Health>();
		InvokeRepeating("Breathing", 4f, _breathTime);
	}

	private void Update()
	{
		//Breathing();
	}

	private void Breathing()
	{
		_breathingAudioSource.Stop();
		if (_currentOxygen > _minOxygen)
		{	
			
			_breathingAudioSource.Play();
			_currentOxygen -= _breathAmmount;
			Mathf.Clamp(_currentOxygen, _minOxygen, _maxOxygen);
			if (_currentOxygen <= _minOxygen)
			{
				_breathingAudioSource.Stop();
				_playerHealthScript.TakeDamage((int)_sufficationDamage);
				Instantiate(_ChokeEffect, _playerHealthScript.gameObject.transform);
			} 
			if (OnOxygenChanged != null) OnOxygenChanged(this, EventArgs.Empty);
			
		}
		else
		{
			_playerHealthScript.TakeDamage((int)_sufficationDamage);
			Instantiate(_ChokeEffect,_playerHealthScript.gameObject.transform);
		}
		
	}

	public void AddOxygen(float oxygenAmount)
	{
		_currentOxygen += oxygenAmount;
		if (_currentOxygen > _maxOxygen) _currentOxygen = _maxOxygen;
		if (OnOxygenChanged != null) OnOxygenChanged(this, EventArgs.Empty);
	}

}
