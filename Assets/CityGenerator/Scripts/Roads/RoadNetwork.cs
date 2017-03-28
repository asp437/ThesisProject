using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crossroad
{
    public List<RoadSegment> adjacentSegemnts;
    public float x;
    public float y;
    public Dictionary<Crossroad, float> minDistances;

    public Crossroad()
    {
        adjacentSegemnts = new List<RoadSegment>();
    }

    public Crossroad(float x, float y)
    {
        adjacentSegemnts = new List<RoadSegment>();
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Crossroad that = (Crossroad)obj;
        return that.x == this.x && that.y == this.y && that.adjacentSegemnts.Equals(this.adjacentSegemnts);
    }

    public override int GetHashCode()
    {
        int value = (int)x;
        value = value * 1337 + (int)y;
        return value * 1337; // + adjacentSegemnts.GetHashCode();
    }
}

public class RoadSegment
{
    private Crossroad start;
    private Crossroad end;
    public int type; // TODO: Create enum with road types
    public float width;

    public RoadSegment(Crossroad start, Crossroad end)
    {
        setStart(start);
        setEnd(end);
        width = 0.5f;
    }

    public RoadSegment()
    {
        width = 0.5f;
    }

    ~RoadSegment()
    {
        setStart(null);
        setEnd(null);
    }

    public float getLength()
    {
        float result = Vector2.Distance(new Vector2(start.x, start.y), new Vector2(end.x, end.y));
        return result;
    }

    public void setStart(Crossroad cr)
    {
        if (start != null)
            start.adjacentSegemnts.Remove(this);
        start = cr;
        if (start != null)
            start.adjacentSegemnts.Add(this);
    }

    public void setEnd(Crossroad cr)
    {
        if (end != null)
            end.adjacentSegemnts.Remove(this);
        end = cr;
        if (end != null)
            end.adjacentSegemnts.Add(this);
    }

    public Crossroad getStart()
    {
        return start;
    }

    public Crossroad getEnd()
    {
        return end;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        RoadSegment that = (RoadSegment)obj;
        return this.end == that.end && this.start == that.start && this.type.Equals(that.type) && this.width.Equals(that.width);
    }

    public override int GetHashCode()
    {
        int value = start.GetHashCode();
        value = value * 1337 + end.GetHashCode();
        value = value * 1337 + type;
        return (int)(value * 1337 + width);
    }
}

public class RoadNetwork
{
    public List<RoadSegment> roadSegments;
    public List<Crossroad> crossroads;

    public RoadNetwork()
    {
        roadSegments = new List<RoadSegment>();
        crossroads = new List<Crossroad>();
    }
}
