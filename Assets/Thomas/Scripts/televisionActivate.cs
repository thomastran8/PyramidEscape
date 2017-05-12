using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class televisionActivate : Activatable {
    VideoPlayer tv;
    
    override public void activate()
    {
        tv = transform.FindChild("TVScreen").GetComponent<VideoPlayer>();
        tv.Play();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
