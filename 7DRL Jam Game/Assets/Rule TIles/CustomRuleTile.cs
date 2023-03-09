using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor> {
    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int OtherNotNull = 3;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.OtherNotNull: return tile != null && !(tile is CustomRuleTile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}