﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmearEffect : MonoBehaviour
{
	Queue<Vector3> _recentPositions = new Queue<Vector3>();

	[SerializeField]
	int _frameLag = 0;

	public Material _smearMat = null;

    public Material smearMat
	{
		get
		{
            if (!_smearMat)
                //_smearMat = this.GetComponent<Material>(); ;

			if (!_smearMat.HasProperty("_PrevPosition"))
				_smearMat.shader = Shader.Find("Custom/Smear");

			return _smearMat;
		}
	}

	void LateUpdate()
	{
		if(_recentPositions.Count > _frameLag)
			smearMat.SetVector("_PrevPosition", _recentPositions.Dequeue());

		smearMat.SetVector("_Position", transform.position);
		_recentPositions.Enqueue(transform.position);
	}
}
