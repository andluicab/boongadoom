using UnityEngine;
using System.Collections;

public class MusicPlay : MonoBehaviour {

	public MusicNames musicToPlay;
	public bool playOnStart = true;
	public bool restartTheMusic = true;

	void Start () {
		if(playOnStart){
			PlayTheMusic();
		}
	}
	
	public void PlayTheMusic () {
		if (restartTheMusic || !AudioManager.getIsPlaying(musicToPlay.ToString ())) {
			AudioManager.StopAllMusic ();
			AudioManager.PlayMusic (musicToPlay.ToString (), transform.position);
		}
	}
}
