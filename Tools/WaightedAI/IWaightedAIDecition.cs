using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
	public interface IWaightedAIDesition {

		/// <summary>
		/// sets the AI that uses this Behavior
		/// </summary>
		/// <param name="aI">the parent AI</param>
		void Initialice(AIBehavior aI);

		/// <summary>
		/// evaluates the importance of the Behavior
		/// </summary>
		/// <returns>the importance as a float value</returns>
		float Evaluate();

		/// <summary>
		/// Call this when you start this Behavior
		/// </summary>
		void StartExecution();

		/// <summary>
		/// Executes one tick of the AI Behavior
		/// </summary>
		void Execute();

		/// <summary>
		/// Call this when you stop this Behavior
		/// </summary>
		void StopExecution();
	}
}