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

public class District
{
    public List<DistrictCell> cells;

    public District()
    {
        cells = new List<DistrictCell>();
    }
}