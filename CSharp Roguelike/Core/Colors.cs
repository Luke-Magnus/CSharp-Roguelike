﻿using RLNET;
 
namespace CSharp_Roguelike.Core
{
    public class Colors
    {
        public static RLColor FloorBackground = RLColor.Black;
        public static RLColor Floor = Palette.AlternateDarkest;
        public static RLColor FloorBackgroundFov = Palette.DbDark;
        public static RLColor FloorFov = Palette.Alternate;

        public static RLColor WallBackground = Palette.SecondaryDarkest;
        public static RLColor Wall = Palette.Secondary;
        public static RLColor WallBackgroundFov = Palette.SecondaryDarker;
        public static RLColor WallFov = Palette.SecondaryLighter;

        public static RLColor TextHeading = Palette.DbLight;

        public static RLColor Player = Palette.DbLight;

        public static RLColor Text = Palette.DbLight;
        public static RLColor Gold = Palette.DbSun;

        public static RLColor DoorBackground = Palette.ComplimentDarkest;
        public static RLColor Door = Palette.ComplimentLighter;
        public static RLColor DoorBackgroundFov = Palette.ComplimentDarker;
        public static RLColor DoorFov = Palette.ComplimentLightest;

        public static RLColor  MonsterColor = Palette.DbBrightWood;
    }
}
