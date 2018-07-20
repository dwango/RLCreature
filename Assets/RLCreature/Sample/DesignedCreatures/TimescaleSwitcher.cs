using UnityEngine;

public class TimescaleSwitcher : MonoBehaviour
{
	public float TimeScaleFastMode = 20;

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Time.timeScale = 1;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			Time.timeScale = TimeScaleFastMode;
		}
		Debug.Log(Time.time);
	}
}
