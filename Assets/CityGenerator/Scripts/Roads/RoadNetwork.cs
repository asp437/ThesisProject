using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crossroad {
    public List<RoadSegment> adjacentSegemnts;
    public float x;
    public float y;

    public Crossroad() {
        adjacentSegemnts = new List<RoadSegment>();
    }

    public Crossroad(float x, float y)
    {
        adjacentSegemnts = new List<RoadSegment>();
        this.x = x;
        this.y = y;
    }
}

public class RoadSegment {
    public Crossroad start;
    public Crossroad end;
    public int type; // TODO: Create enum with road types
    public float width;

    public RoadSegment(Crossroad start, Crossroad end)
    {
        this.start = start;
        this.end = end;
        width = 0.5f;
    }

    public RoadSegment()
    {
        width = 0.5f;
    }

    public void setStart(Crossroad cr) {
        if (start != null)
            start.adjacentSegemnts.Remove(this);
        start = cr;
        start.adjacentSegemnts.Add(this);
    }

    public void setEnd(Crossroad cr) {
        if (end != null)
            end.adjacentSegemnts.Remove(this);
        end = cr;
        end.adjacentSegemnts.Add(this);
    }
}

public class RoadNetwork {
    public List<RoadSegment> roadSegments;
    public List<Crossroad> crossroads;

    public RoadNetwork() {
        roadSegments = new List<RoadSegment>();
        crossroads = new List<Crossroad>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
