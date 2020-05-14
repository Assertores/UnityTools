using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
	public interface ICameraNode {
		/// <summary>
		/// call this to set the camera with witch you want to make the tracking shot.
		/// </summary>
		/// <param name="cam">the transform of the camera. this must have a Camera component</param>
		void SetCamera(Transform cam);

		/// <summary>
		/// call this to calculate the next cameraposition in reference to last frame
		/// </summary>
		/// <returns>if the node tracking is finished</returns>
		bool NextTick();

		/// <summary>
		/// call this to calculate the next cameraposition
		/// </summary>
		/// <param name="timeDelta">colapst time since last call</param>
		/// <returns>if the node tracking is finished</returns>
		bool NextTick(float timeDelta);
	}
}
