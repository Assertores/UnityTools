using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum axis {
	x,
	y,
	z,
}

public class SplitPointCloud : MonoBehaviour {

	[SerializeField] List<Transform> pointTransforms;
	[SerializeField] int palettSize;
	[SerializeField] GameObject prefab;

	private void Start() {
		Queue<Vector3[]> clounds = new Queue<Vector3[]>();
		{
			Vector3[] points = new Vector3[pointTransforms.Count];
			for(int i = 0; i < points.Length; i++) {
				points[i] = pointTransforms[i].position;
				
				pointTransforms[i].gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(points[i].x / 10, points[i].y / 10, points[i].z / 10));
			}
			clounds.Enqueue(points);
		}
		//*
		for(int i = 0; clounds.Count < palettSize; i++) {
			for(int j = clounds.Count; j > 0 && clounds.Count < palettSize; j--) {
				Vector3[] tmp1;
				Vector3[] tmp2;
				Split(clounds.Dequeue(), i, out tmp1, out tmp2);
				clounds.Enqueue(tmp1);
				clounds.Enqueue(tmp2);
			}
		}
		/*/
		for(int i = 0; clounds.Peek().Length > pointsPerCloud; i++) {
			Vector3[] tmp1;
			Vector3[] tmp2;
			Split(clounds.Dequeue(), i, out tmp1, out tmp2);
			clounds.Enqueue(tmp1);
			clounds.Enqueue(tmp2);
		}
		//*/
		for(int i = 0; clounds.Count > 0; i++) {
			var element = Instantiate(prefab);
			Vector3[] poses = clounds.Dequeue();
			element.transform.position = GetMedian(poses);
			element.GetComponent<Renderer>().material.SetColor("_Color", new Color(element.transform.position.x / 10, element.transform.position.y / 10, element.transform.position.z / 10));

			List<Vector3> tmp = new List<Vector3>(poses);
			var transes = pointTransforms.FindAll(x => tmp.Contains(x.position));
			foreach(var it in transes) {
				it.SetParent(element.transform);
			}
		}
	}

	void Split(Vector3[] original, int axie, out Vector3[] arr1, out Vector3[] arr2) {

		var tmpOrigin = new List<Vector3>(original);

		axie %= 3;

		switch(axie) {
		case 0:
			tmpOrigin.Sort((lhs, rhs) => lhs.x.CompareTo(rhs.x));
			break;
		case 1:
			tmpOrigin.Sort((lhs, rhs) => lhs.y.CompareTo(rhs.y));
			break;
		case 2:
			tmpOrigin.Sort((lhs, rhs) => lhs.z.CompareTo(rhs.z));
			break;
		}

		arr1 = tmpOrigin.GetRange(0, (int)(tmpOrigin.Count * 0.5f)).ToArray();
		arr2 = tmpOrigin.GetRange((int)(tmpOrigin.Count * 0.5f), tmpOrigin.Count - (int)(tmpOrigin.Count * 0.5f)).ToArray();
	}

	Vector3 GetMedian(Vector3[] origin) {
		Vector3 value = Vector3.zero;

		foreach(var it in origin) {
			value += it;
		}
		return value / origin.Length;
	}
}
