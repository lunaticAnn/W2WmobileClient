﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedQueue<T> {
	public int size { get; private set; }
	Queue<T> myQueue;
	public LimitedQueue(int s){
		size = s;
		myQueue = new Queue<T>();
	}

/// <summary>
/// return true if queue reaches limited size;
/// </summary>
/// <param name="obj"></param>
/// <returns></returns>
	public bool Enqueue(T obj) {
		myQueue.Enqueue(obj);
		if (myQueue.Count > size){
			myQueue.Dequeue();
			return true;
		}
		return false;		
	}

	public void Dequeue() {
		myQueue.Dequeue();
	}
}
