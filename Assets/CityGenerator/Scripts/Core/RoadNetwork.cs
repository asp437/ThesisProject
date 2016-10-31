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
}

public class RoadSegment {
    public Crossroad start;
    public Crossroad end;
    public int type; // TODO: Create enum with road types
    public float width;

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
