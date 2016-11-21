using UnityEngine;
using System.Collections;

public class Point {
    public int x, y;
    public Point ()
    {
        this.x = this.y = 0;
    }
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
	}
	public Point(float x, float y)
	{
		this.x = (int)x;
		this.y = (int)y;
	}
    public Point(Vector2 Position)
    {
        this.x = (int)Position.x;
        this.y = (int)Position.y;
    }
}