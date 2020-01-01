using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree2D
{
#pragma warning disable 0649
    public Rect             boundary;
    private int             capacity;
    private List<Vector2>   points;
    private QuadTree2D      northEast;
    private QuadTree2D      southEast;
    private QuadTree2D      southWest;
    private QuadTree2D      northWest;
    private bool            divided;
#pragma warning restore 0649

    public QuadTree2D(Rect boundary, int capacity)
    {
        this.boundary = boundary;
        this.capacity = capacity;
        points = new List<Vector2>();
        divided = false;
    }   

    public void Insert(Vector2 point)
    {
        if (!Contains(point))
        {
            return;
        }
             
        if (points.Count < capacity)
        {
            points.Add(point);
            return;
        }
        else if(!this.divided)
        {
            Subdivide();
        }

        northEast.Insert(point);
        southEast.Insert(point);
        southWest.Insert(point);
        northWest.Insert(point);
    }

    private bool Contains(Vector2 point)
    {
        return point.x <= boundary.x + boundary.width &&
               point.x >= boundary.x && 
               point.y <= boundary.y + boundary.height && 
               point.y >= boundary.y;   
    }

    private void Subdivide()
    {
        this.northEast = new QuadTree2D(new Rect(boundary.x + boundary.width * 0.5f,     boundary.y,                             boundary.width / 2f,    boundary.height / 2f), capacity);
        this.southEast = new QuadTree2D(new Rect(boundary.center.x,                      boundary.center.y,                      boundary.width / 2f,    boundary.height / 2f), capacity);
        this.southWest = new QuadTree2D(new Rect(boundary.x,                             boundary.y + boundary.height * 0.5f,    boundary.width / 2f,    boundary.height / 2f), capacity);
        this.northWest = new QuadTree2D(new Rect(boundary.x,                             boundary.y,                             boundary.width / 2f,    boundary.height / 2f), capacity);
        divided = true;
    }

    public void ShowData()
    {
        if (this.divided)
        {
            northEast.ShowData();
            southEast.ShowData();
            southWest.ShowData();
            northWest.ShowData();
        }
        else
        {
            for (int i = 0; i < points.Count; i++)
            {
                Debug.Log(points[i]);
            }
        }
    }

    public void GraphDebug()
    {
        if (this.divided)
        {
            this.northEast.GraphDebug();
            this.southEast.GraphDebug();
            this.southWest.GraphDebug();
            this.northWest.GraphDebug();
        }
        else
        {
            var nw = new Vector3(boundary.x, boundary.y);
            var ne = new Vector3(boundary.x + boundary.width, boundary.y);
            var se = new Vector3(boundary.x + boundary.width, boundary.y + boundary.height);
            var sw = new Vector3(boundary.x, boundary.y + boundary.height);

            Gizmos.DrawLine(nw, ne);
            Gizmos.DrawLine(ne, se);
            Gizmos.DrawLine(se, sw);
            Gizmos.DrawLine(sw, nw);
        }
    }


    /// <summary>
    /// returns a point from tree
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public QuadTree2D Find(Vector2 point)
    {
        if (!this.Contains(point))
        {
            return null;
        }
        if (!this.divided)
        {
            return this;
        }
        QuadTree2D result;

        result = northEast.Find(point);
        if (!(result is null))
            return result;

        result = southEast.Find(point);
        if (!(result is null))
            return result;

        result = southWest.Find(point);
        if (!(result is null))
            return result;

        return northWest.Find(point);
    }


    /// <summary>
    /// returns all points in a rect
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public List<Vector2> AreaSearch(Rect rect)
    {
        List<Vector2> points = new List<Vector2>();
        
        if (!this.boundary.Intersects(rect))
        {
            return new List<Vector2>();
        }
        else
        {
            foreach (var item in this.points)
            {
                if (Contains(item))
                {
                    points.Add(item);
                }
            }
        }

        if (this.divided)
        {
            points = points.Concat(northEast.AreaSearch(rect)).ToList();
            points = points.Concat(southEast.AreaSearch(rect)).ToList();
            points = points.Concat(southWest.AreaSearch(rect)).ToList();
            points = points.Concat(northWest.AreaSearch(rect)).ToList();
        }

        return points;
    }
}

public static class RectTools
{
    public static bool Intersects(this Rect rect, Rect other)
    {
        return   !(other.x - other.width > rect.x + rect.width ||
                    other.x + rect.width < rect.x - rect.width ||
                 other.y - other.height > rect.y + rect.height ||
                  other.y + rect.height < rect.y - rect.height);
    }
}