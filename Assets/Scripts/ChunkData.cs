using UnityEngine;

[System.Flags]
public enum ChunkExit
{
    None = 0,
    Left = 1 << 0,
    Middle = 1 << 1,
    Right = 1 << 2
}

public enum ChunkEntrance
{
    None = 0,
    Left = 1 << 0,
    Middle = 1 << 1,
    Right = 1 << 2
}

public class ChunkData : MonoBehaviour
{
    public ChunkExit exits;
    public ChunkEntrance entrances;

    public bool HasExit(ChunkExit exit)
    {
        return (exits & exit) != 0;
    }

    public bool HasEntrance(ChunkEntrance entrance)
    {
        return (entrances & entrance) != 0;
    }
}
