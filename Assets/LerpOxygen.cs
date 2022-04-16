using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LerpOxygen : MonoBehaviour
{

	public Oxygen playerOxygenScript;
	[SerializeField]
	private float _newOxygen;
	[SerializeField]
	private float _currentOxygen;
	[SerializeField]
	private float _previousOxygen;
	[SerializeField]
	private float _maxOxygen = 100;
	[Tooltip("The slider for health")]

	[SerializeField]
	private Slider _oxygenSlider;
	[SerializeField]
	private TMP_Text _oxygenNumberText;

	[SerializeField]
	private bool Animating = false;
	[SerializeField]
	private float lerpSpeed = 2f;

	


	private void Start()
	{
		if (playerOxygenScript != null)
		{
			_maxOxygen = _currentOxygen = _newOxygen = _oxygenSlider.maxValue = _oxygenSlider.value = playerOxygenScript._currentOxygen;
			playerOxygenScript.OnOxygenChanged += PlayerOxygenScript_OnOxygenChanged;
		}
	}

	private void PlayerOxygenScript_OnOxygenChanged(object sender, System.EventArgs e)
	{
		_previousOxygen = _currentOxygen;
		_newOxygen = playerOxygenScript._currentOxygen;
		_newOxygen = Mathf.Clamp(_newOxygen, 0, _maxOxygen);
		Animating = true;
	}

	private void Update()
	{
		if (Animating)
		{
			LerpNumber();
		}
	}
	
	float t = 0;

	void LerpNumber()
	{
		t += lerpSpeed * Time.deltaTime;

		_currentOxygen = Mathf.Lerp(_previousOxygen, _newOxygen, t);

		_oxygenSlider.value = _currentOxygen;

		_oxygenNumberText.text = _currentOxygen.ToString("F0");

		if (_currentOxygen == _newOxygen)
		{
			Animating = false;
			t = 0;
		}
	}
}
