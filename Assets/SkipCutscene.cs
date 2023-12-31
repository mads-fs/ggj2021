﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipCutscene : MonoBehaviour {
    public PlayableDirector[] directors;
    
    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Jump")) {
            foreach (var director in directors)
                if (director.playableGraph.IsValid())
                    if (director.playableGraph.IsPlaying()) {
                        Time.timeScale = 50;
                        return;
                    }
        }
    }

    public void ResetTimescale() {
        print("reset");
        Time.timeScale = 1;
    }
}
