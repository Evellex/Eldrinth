using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ExamplesController : MonoBehaviour {

	[Serializable]
	public class Example
	{
		public string Name;
		[Multiline]
		public string Description;
		public Transform Prefab;
	}

	public Example[] m_Examples;
	public GameObject m_Canvas = null;
	public Slider m_ExamplesSlider = null;

	private Text m_Decription = null;
	private GameObject m_CurrentPrefab = null;
	private int m_CurrentExample = 0;

	void Start()
	{
		//Instantiate (m_Canvas, Vector3.zero, Quaternion.identity);
		m_Decription = GameObject.Find ("ExampleDescription").GetComponent<Text>();
		activateExample (0);

		if (m_ExamplesSlider != null) {
			m_ExamplesSlider.maxValue = (int)(m_Examples.Length -1);
		}
	}

	//void Update() {
		/*
		if (Input.GetButtonDown("Next"))
		    NextExample();

		if (Input.GetButtonDown ("Previous"))
			PreviousExample ();*/
	//}

	public void NextExample() {
		if (m_Examples[m_CurrentExample].Prefab != m_CurrentPrefab && m_CurrentPrefab != null)
			Destroy (m_CurrentPrefab);
		m_CurrentExample ++;
		ClampExampleCount ();
		m_CurrentPrefab = Instantiate(m_Examples[m_CurrentExample].Prefab, Vector3.zero, Quaternion.identity) as GameObject;

		if (m_Decription != null)
			m_Decription.text = m_Examples [m_CurrentExample].Description;
	}

	public void PreviousExample() {
		if (m_Examples[m_CurrentExample].Prefab != m_CurrentPrefab && m_CurrentPrefab != null)
			Destroy (m_CurrentPrefab);
		m_CurrentExample --;
		ClampExampleCount ();
		m_CurrentPrefab = Instantiate(m_Examples[m_CurrentExample].Prefab, Vector3.zero, Quaternion.identity) as GameObject;

		if (m_Decription != null)
			m_Decription.text = m_Examples [m_CurrentExample].Description;
	}

	private void ClampExampleCount() {
		if (m_CurrentExample < 0)
			m_CurrentExample = m_Examples.Length - 1;
		if (m_CurrentExample > m_Examples.Length - 1)
			m_CurrentExample = 0;
	}

	public void activateExampleFromSlider () {
		if (m_ExamplesSlider != null) {
			activateExample((int)(m_ExamplesSlider.value));
		}
	}

	public void activateExample(int index)
	{
		if (m_Examples[index].Prefab != m_CurrentPrefab && m_CurrentPrefab != null)
			Destroy (m_CurrentPrefab);

		m_CurrentPrefab = Instantiate(m_Examples[index].Prefab, Vector3.zero, Quaternion.identity) as GameObject;

		if (m_Decription != null)
			m_Decription.text = m_Examples [index].Description;

		/*
		index --;
		foreach (GameObject example in examples)
		{
			if (example == examples[index])
				example.SetActive(true);
			else
				example.SetActive(false);
		}

		text.text = examplesDesc [index];
		*/
	}


}
