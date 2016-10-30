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

        /*
        Crossroad cr0 = new Crossroad(), cr1 = new Crossroad(), cr2 = new Crossroad(), cr3 = new Crossroad(), cr4 = new Crossroad();
        RoadSegment segment0 = new RoadSegment(), segment1 = new RoadSegment(), segment2 = new RoadSegment(), segment3 = new RoadSegment();
        cr0.x = 32; cr0.y = 32;
        cr1.x = 0; cr1.y = 0;
        cr2.x = 32; cr2.y = 64;
        cr3.x = 0; cr3.y = 32;
        cr4.x = 64; cr4.y = 64;

        segment0.setStart(cr1);
        segment0.setEnd(cr0);
        segment1.setStart(cr2);
        segment1.setEnd(cr0);
        segment2.setStart(cr3);
        segment2.setEnd(cr0);
        segment3.setStart(cr4);
        segment3.setEnd(cr0);

        roadSegments.Add(segment0);
        roadSegments.Add(segment1);
        roadSegments.Add(segment2);
        roadSegments.Add(segment3);
        crossroads.Add(cr0);
        crossroads.Add(cr1);
        crossroads.Add(cr2);
        crossroads.Add(cr3);
        crossroads.Add(cr4);
        */
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
