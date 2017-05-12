using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {
    private static GameObject instance;

    void Awake()
    {
        if (instance != null && instance != this || SceneManager.GetActiveScene().name == this.name)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
