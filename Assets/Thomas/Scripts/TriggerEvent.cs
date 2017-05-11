using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour {
    public bool useScreenShake = false;
    public float eventTriggerTime = 0.2f;
    public GameObject[] monsterActivatables;
    public GameObject[] sceneActivatables;
    private CameraEffects playerCamEffect;
    private AudioSource[] sounds;
    private bool isActivated = false;

    // Use this for initialization
    void Start () {
        sounds = GetComponents<AudioSource>();
        playerCamEffect = Camera.main.GetComponent<CameraEffects>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter()
    {
        if (isActivated)
        {
            return;
        }
        isActivated = true;
        sounds[0].Play(); //lever click
        StartCoroutine(WaitToActivate());
    }

    IEnumerator WaitToActivate()
    {
        if (!useScreenShake)
        {
            yield return new WaitForSeconds(eventTriggerTime);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
            playerCamEffect.StartShake(0.1f, 1.0f);
            yield return new WaitForSeconds(8.0f);
        }
        for (int i = 0; i < monsterActivatables.Length; i++)
        {
            monsterActivatables[i].SetActive(true);
        }

        for (int i = 0; i < sceneActivatables.Length; i++)
        {
            sceneActivatables[i].GetComponent<Activatable>().activate();
        }
    }
}
