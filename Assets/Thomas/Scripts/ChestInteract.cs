using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteract : MonoBehaviour {
    public GameObject gabePortrait;
    public GameObject gabeConcern;
    public GameObject[] sceneActivatables;
    private AudioSource trapNoise;

    // Use this for initialization
    void Start () {
        trapNoise = GetComponent<AudioSource>();
	}

    public void openChest()
    {
        gabePortrait.SetActive(false);
        gabeConcern.SetActive(true);
        trapNoise.Play();
        gameObject.transform.FindChild("chest_epic_gems").gameObject.SetActive(false);
        for (int i = 0; i < sceneActivatables.Length; i++)
        {
            sceneActivatables[i].GetComponent<Activatable>().activate();
        }
    }
}
