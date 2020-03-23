using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class RoomObject {

    public Vector2 coordinateInGrid;
    float tileWidth;
    float tileHeight;

    public Doors doors;

    public RoomObject(Vector2 coord, Doors doors, float width, float height) {
        coordinateInGrid = coord;
        this.doors = doors;
        tileWidth = width;
        tileHeight = height;
    }

    public static Vector2 arrayIndexFromCoordinate(Vector2 coordinate, int maxRooms) {
        return coordinate + Vector2.one * 0.5f * maxRooms;
    }

    public Vector2 Coordinate {
        get {
            return coordinateInGrid;
        }
    }

    public struct Doors {
        public bool north, south;
        public bool east, west;

        public Doors(int configKey) {
            string bin = Convert.ToString(configKey, 2);
            north = bin[0] == '0';
            east = bin[1] == '0';
            south = bin[2] == '0';
            west = bin[3] == '0';
        }

        public Doors(bool north, bool south, bool east, bool west) {
            this.north = north;
            this.south = south;
            this.east = east;
            this.west = west;
        }
    }
}
