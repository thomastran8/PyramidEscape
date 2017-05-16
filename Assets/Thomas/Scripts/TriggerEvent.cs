using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour {
    public bool useScreenShake = false;
    public bool killEnemiesToRevert = false;
    public float eventTriggerTime = 0.2f;
    private bool revertScene = false;
    public GameObject[] monsterActivatables;
    public GameObject[] sceneActivatables;
    private CameraEffects playerCamEffect;
    private AudioSource[] sounds;
    private bool isActivated = false;
    private int monsterDead = 0;
    private int numMonsters;

    // Use this for initialization
    void Start () {
        sounds = GetComponents<AudioSource>();
        playerCamEffect = Camera.main.GetComponent<CameraEffects>();
        numMonsters = monsterActivatables.Length;
    }
	
	// Update is called once per frame
	void Update () {
        if (isActivated && killEnemiesToRevert && !revertScene)
        {
            for (int i = 0; i < monsterActivatables.Length; i++)
            {
                if (!monsterActivatables[i])
                {
                    Debug.Log("Something died");
                    monsterDead++;
                }
            }

            if (monsterDead == numMonsters)
            {
                revertScene = true;
                sounds[0].Play(); //lever click
                for (int i = 0; i < sceneActivatables.Length; i++)
                {
                    Debug.Log("Reverring");
                    sceneActivatables[i].GetComponent<Activatable>().deActivate();
                }
            }
            else
            {
                monsterDead = 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActivated)
        {
            return;
        }

        if (other.gameObject.transform.parent.tag == "Player")
        {
            isActivated = true;
            sounds[0].Play(); //lever click
            StartCoroutine(WaitToActivate());
        }
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
            playerCamEffect.StartShake(1f, 1.0f);
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
