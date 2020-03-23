using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonGenerator : MonoBehaviour {

    public int numberOfRooms;
    public float roomWidth;
    public float roomHeight;

    public string dungeonSeed;

    public RoomObject[,] dungeons;

    private void Start() {
        dungeons = GenerateDungeon(numberOfRooms, dungeonSeed);
    }

    public RoomObject[,] GenerateDungeon(int numberOfRooms, string dungeonSeed) {
        int seed = (dungeonSeed != "") ? ((int.TryParse(dungeonSeed, out int i)) ? i : dungeonSeed.GetHashCode()) : UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        System.Random prng = new System.Random(seed);

        RoomObject[,] roomArray = new RoomObject[numberOfRooms * 2, numberOfRooms * 2];
        List<RoomObject> closed = new List<RoomObject>();   // rooms that have had their doors evaluated
        List<RoomObject> open = new List<RoomObject>();     // rooms that have yet to have their doors evaluated

        RoomObject initialRoom = new RoomObject(Vector2.zero, new RoomObject.Doors(true, true, true, true), roomWidth, roomHeight);
        roomArray[numberOfRooms, numberOfRooms] = initialRoom;
        open.Add(initialRoom);

        while (closed.Count + open.Count < numberOfRooms) {
            RoomObject currentRoom = open[0];

            if (currentRoom.doors.north) {
                RoomObject newRoom = roomFromPrevRoom(Vector2.up, currentRoom.Coordinate, prng);

                Vector2 arrayIndex = RoomObject.arrayIndexFromCoordinate(newRoom.Coordinate, numberOfRooms * 2);
                roomArray[(int)arrayIndex.x, (int)arrayIndex.y] = newRoom;
                open.Add(newRoom);
            }
            if (currentRoom.doors.south) {
                RoomObject newRoom = roomFromPrevRoom(Vector2.down, currentRoom.Coordinate, prng);

                Vector2 arrayIndex = RoomObject.arrayIndexFromCoordinate(newRoom.Coordinate, numberOfRooms * 2);
                roomArray[(int)arrayIndex.x, (int)arrayIndex.y] = newRoom;
                open.Add(newRoom);
            }
            if (currentRoom.doors.east) {
                RoomObject newRoom = roomFromPrevRoom(Vector2.right, currentRoom.Coordinate, prng);

                Vector2 arrayIndex = RoomObject.arrayIndexFromCoordinate(newRoom.Coordinate, numberOfRooms * 2);
                roomArray[(int)arrayIndex.x, (int)arrayIndex.y] = newRoom;
                open.Add(newRoom);
            }
            if (currentRoom.doors.west) {
                RoomObject newRoom = roomFromPrevRoom(Vector2.left, currentRoom.Coordinate, prng);

                Vector2 arrayIndex = RoomObject.arrayIndexFromCoordinate(newRoom.Coordinate, numberOfRooms * 2);
                roomArray[(int)arrayIndex.x, (int)arrayIndex.y] = newRoom;
                open.Add(newRoom);
            }

            open.Remove(currentRoom);
            closed.Add(currentRoom);
        }

        return roomArray;
    }

    RoomObject roomFromPrevRoom(Vector2 dir, Vector2 prevCoordinate, System.Random prng) {
        Vector2 newCoord = prevCoordinate + dir;
        Vector2 arrayIndex = RoomObject.arrayIndexFromCoordinate(newCoord, numberOfRooms * 2);
        RoomObject.Doors doorConfig = new RoomObject.Doors(prng.Next(0, 16));
        if (dir == Vector2.up) {
            doorConfig.south = true;
        }
        else if (dir == Vector2.down) {
            doorConfig.north = true;
        }
        else if (dir == Vector2.left) {
            doorConfig.east = true;
        }
        else if (dir == Vector2.right) {
            doorConfig.west = true;
        }

        return new RoomObject(newCoord, doorConfig, roomWidth, roomHeight);
    }
}
