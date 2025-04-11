using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Globals
{

    /// <summary>
    /// Contains all tags that are used in game.
    /// When referencing tag strings, only use this class.
    /// </summary>
    public static class StaticTagStrings
    {
        // This is the tag that will be used to declare an object is a small block
        public const string SMALL_BLOCK = "SmallBlock";

        // Blocks that require moving into to move or magic to pickup
        public const string LARGE_BLOCK = "LargeBlock";

        // Player Tag
        public const string PLAYER = "Player";

        // Enemy Tag
        public const string ENEMY = "Enemy";

        // Environment Triggers
        public const string TRIGGER = "Trigger";

        // For objects that the player can move into and it'll push them
        public const string SHOVEABLE = "Shoveable";
    }

}
