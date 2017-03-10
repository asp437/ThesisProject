using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictCell
{
    public float x;
    public float y;
    public bool edgeLeft;
    public bool edgeRight;
    public bool edgeUp;
    public bool edgeBottom;

    public DistrictCell(float x, float y, bool edgeLeft, bool edgeRight, bool edgeUp, bool edgeBottom)
    {
        this.x = x;
        this.y = y;
        this.edgeLeft = edgeLeft;
        this.edgeRight = edgeRight;
        this.edgeUp = edgeUp;
        this.edgeBottom = edgeBottom;
    }

    public DistrictCell(float x, float y) : this(x, y, false, false, false, false) { }

    public DistrictCell(Vector2 position) : this(position.x, position.y) { }

    public DistrictCell(int x, int y) : this((float)x, (float)y) { }

    public DistrictCell() : this(0, 0) { }
}

public enum DistrictType
{
    UNKNOWN,        // For non-initialized districts
    INDUSTRIAL,     // Industrial zone
    RESIDENTIAL,    // Residential zone. Can be combined with commercial at edges
    RECREATIONAL,   // Recreational zone. Shouldn't be placed near industrial zone.
    COMMERCIAL,     // Shops, malls and other commercial buildings
    COUNT
}

public class District
{
    public List<DistrictCell> cells;
    public DistrictType type;
    public DistrictType edgeTypeLeft;
    public DistrictType edgeTypeRight;
    public DistrictType edgeTypeUp;
    public DistrictType edgeTypeBottom;
    private static System.Random rand;

    public District()
    {
        if (rand == null)
            rand = new System.Random();

        cells = new List<DistrictCell>();
        type = DistrictType.RESIDENTIAL;
        edgeTypeLeft = DistrictType.UNKNOWN;
        edgeTypeRight = DistrictType.UNKNOWN;
        edgeTypeUp = DistrictType.UNKNOWN;
        edgeTypeBottom = DistrictType.UNKNOWN;
    }

    public float getDistanceTo(District another, RoadNetwork roadNetwork)
    {
        // TODO: Distance via roads
        Vector2 thisPosition = new Vector2();
        foreach (DistrictCell cell in cells)
        {
            thisPosition.x += cell.x;
            thisPosition.y += cell.y;
        }
        thisPosition.x /= cells.Count;
        thisPosition.y /= cells.Count;

        Vector2 anotherPosition = new Vector2();
        foreach (DistrictCell cell in another.cells)
        {
            anotherPosition.x += cell.x;
            anotherPosition.y += cell.y;
        }
        anotherPosition.x /= cells.Count;
        anotherPosition.y /= cells.Count;

        return Vector2.Distance(thisPosition, anotherPosition);
    }
}