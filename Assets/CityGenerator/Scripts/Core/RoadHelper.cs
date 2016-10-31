using UnityEngine;
using System.Collections;

public class RoadHelper {
    // Line segment intersection algorithm by Bryce Boe
    // More info: http://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
    protected static bool ccw(Crossroad A, Crossroad B, Crossroad C) {
        return (C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x);
    }
    
    public static bool intersects(Crossroad cr00, Crossroad cr01, Crossroad cr10, Crossroad cr11) {
        return ccw(cr00, cr10, cr11) != ccw(cr01, cr10, cr11) && ccw(cr00, cr01, cr10) != ccw(cr00, cr01, cr11);
    }
}
