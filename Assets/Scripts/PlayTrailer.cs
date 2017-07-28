using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayTrailer : MonoBehaviour {

	string url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";

	void Start(){
		// Will attach a VideoPlayer to the main camera.
		// VideoPlayer automatically targets the camera backplane when it is added
		// to a camera object, no need to change videoPlayer.targetCamera.
		var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();

		// By default, VideoPlayers added to a camera will use the far plane.
		// Let's target the near plane instead.
		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

		// This will cause our scene to be visible through the video being played.
		videoPlayer.targetCameraAlpha = 1F;

		// Set the video to play. URL supports local absolute or relative paths.
		// Here, using absolute.
		videoPlayer.url = url;

		// Skip the first 100 frames.
		videoPlayer.frame = 100;

		// Restart from beginning when done.
		videoPlayer.isLooping = true;

		// Each time we reach the end, we slow down the playback by a factor of 10.
		videoPlayer.loopPointReached += EndReached;

		// Start playback. This means the VideoPlayer may have to prepare (reserve
		// resources, pre-load a few frames, etc.). To better control the delays
		// associated with this preparation one can use videoPlayer.Prepare() along with
		// its prepareCompleted event.
		videoPlayer.Play();
	}

	void EndReached(UnityEngine.Video.VideoPlayer vp){
		vp.playbackSpeed = vp.playbackSpeed / 10.0F;
	}

	string linkParser(string url) {
		//this is for parsing the url to the correct format to stream
		
		return "";	
	}
	
}
