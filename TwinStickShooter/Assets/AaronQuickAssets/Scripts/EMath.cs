using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EMath {
    
    public static float Round(this float val)
    {
        return Mathf.Round(val);
    }

    public static bool CloseTo(this float val, float val2, float margin = 1)
    {
        return (val >= val2 - margin && val <= val2 + margin);
    }
}
