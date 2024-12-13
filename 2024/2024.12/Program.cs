using ClassLibrary;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using System.Runtime.ExceptionServices;

List<string> map = [];
//using var sr = new StringReader("RRRRIICCFF\r\nRRRRIICCCF\r\nVVRRRCCFFF\r\nVVRCCCJFFF\r\nVVVVCJJCFE\r\nVVIVCCJJEE\r\nVVIIICJJEE\r\nMIIIIIJJEE\r\nMIIISIJEEE\r\nMMMISSJEEE");
using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 12)));
{
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        map.Add(line);
    }
}


HashSet<(int i, int j)> explored = [];
HashSet<(Fence fence, Guid region)> fenceNodes = [];
HashSet<(Fence fence, Guid region)> markedFenceNodes = [];
Dictionary<Guid, (int area, int perimeter, int corners)> regions = [];
List<Guid> guids = [];
List<char> plants = [];

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Length; j++)
    {
        if (!explored.Contains((i, j)))
        {
            var regionGuid = Guid.NewGuid();
            guids.Add(regionGuid);
            regions.Add(regionGuid, (0, 0, 0));
            TraceRegion(i, j, regionGuid, map[i][j]);
            plants.Add(map[i][j]);
        }
    }
}

foreach (var guid in guids)
{
    bool secondTry = false;
    trailcheck:
    var trailStart = fenceNodes.Where(fence => fence.region == guid).First();
    Direction currentDirection;
    if (trailStart.fence.orientation == Orientation.Right)
        currentDirection = Direction.Right;
    else
        currentDirection = Direction.Up;
    (Fence fence, Guid region) trail = (new Fence(-1, -1, Orientation.Right), new Guid());
    while (!trail.Equals(trailStart))
    {
        if (trail.fence.i == -1) trail = trailStart;
        markedFenceNodes.Add(trail);
        (Fence fence, Guid region) tempTrail;
        if (currentDirection == Direction.Right)
        {
            if (trail.fence.i == 0 || trail.fence.i == map.Count)
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j + 1, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Up;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j + 1, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Down;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j + 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.i > 0 && map[trail.fence.i - 1][trail.fence.j] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j + 1, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Up;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j + 1, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Down;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j + 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.i < map.Count && map[trail.fence.i][trail.fence.j] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j + 1, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Down;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j + 1, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Up;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j + 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
        }
        else if (currentDirection == Direction.Left)
        {
            if (trail.fence.i == 0 || trail.fence.i == map.Count)
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Up;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Down;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.i > 0 && map[trail.fence.i - 1][trail.fence.j] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Up;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Down;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.i < map.Count && map[trail.fence.i][trail.fence.j] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Down;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Up;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
        }
        else if (currentDirection == Direction.Up)
        {
            if (trail.fence.j == 0 || trail.fence.j == map[0].Length)
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j, Orientation.Right), guid), out tempTrail))
                {
                    trail = (new Fence(trail.fence.i, trail.fence.j, Orientation.Right), guid);
                    currentDirection = Direction.Right;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Left;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.j > 0 && map[trail.fence.i][trail.fence.j - 1] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Left;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j, Orientation.Right), guid), out tempTrail))
                {
                    trail = (new Fence(trail.fence.i, trail.fence.j, Orientation.Right), guid);
                    currentDirection = Direction.Right;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.j < map[0].Length && map[trail.fence.i][trail.fence.j] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j, Orientation.Right), guid), out tempTrail))
                {
                    trail = (new Fence(trail.fence.i, trail.fence.j, Orientation.Right), guid);
                    currentDirection = Direction.Right;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Left;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i - 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
        }
        else if (currentDirection == Direction.Down)
        {
            if (trail.fence.j == 0 || trail.fence.j == map[0].Length)
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Left;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Right;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.j > 0 && map[trail.fence.i][trail.fence.j - 1] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Left;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Right;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
            else if (trail.fence.j < map[0].Length && map[trail.fence.i][trail.fence.j] == plants[guids.IndexOf(guid)])
            {
                if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Right;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j - 1, Orientation.Right), guid), out tempTrail))
                {
                    trail = tempTrail;
                    currentDirection = Direction.Left;
                    regions[guid] = (regions[guid].area, regions[guid].perimeter, regions[guid].corners + 1);
                }
                else if (ContainsOut(fenceNodes, (new Fence(trail.fence.i + 1, trail.fence.j, Orientation.Down), guid), out tempTrail))
                {
                    trail = tempTrail;
                }
            }
        }
    }
    fenceNodes.ExceptWith(markedFenceNodes);
    markedFenceNodes.Clear();
    var q = fenceNodes.Where(i => i.region == guid);
    if (q.Any())
    {
        goto trailcheck;
    }
}


int totalPrice = 0;
int cornerPrice = 0;

foreach (var (id, (area, perimeter, corner)) in regions)
{
    totalPrice += area * perimeter;
    cornerPrice += area * corner;
}

Console.WriteLine($"Total price: {totalPrice}");
Console.WriteLine($"Total price with bulk discounts: {cornerPrice}");

void TraceRegion(int i, int j, Guid guid, char plant)
{
    if (map[i][j] != plant)
    {
        regions[guid] = (regions[guid].area, regions[guid].perimeter + 1, regions[guid].corners);
    }
    else if (!explored.Contains((i, j)))
    {
        explored.Add((i, j));
        regions[guid] = (regions[guid].area + 1, regions[guid].perimeter, regions[guid].corners);

        if (i == 0)
        {
            regions[guid] = (regions[guid].area, regions[guid].perimeter + 1, regions[guid].corners);
            TraceRegion(i + 1, j, guid, plant);
            if (map[i + 1][j] != plant)
            {
                fenceNodes.Add((new Fence(i + 1, j, Orientation.Right), guid));
            }
            fenceNodes.Add((new Fence(i, j, Orientation.Right), guid));
        }
        else if (i < map.Count - 1)
        {
            TraceRegion(i + 1, j, guid, plant);
            TraceRegion(i - 1, j, guid, plant);
            if (map[i + 1][j] != plant)
            {
                fenceNodes.Add((new Fence(i + 1, j, Orientation.Right), guid));
            }
            if (map[i - 1][j] != plant)
            {
                fenceNodes.Add((new Fence(i, j, Orientation.Right), guid));
            }
        }
        else
        {
            regions[guid] = (regions[guid].area, regions[guid].perimeter + 1, regions[guid].corners);
            TraceRegion(i - 1, j, guid, plant);
            if (map[i - 1][j] != plant)
            {
                fenceNodes.Add((new Fence(i, j, Orientation.Right), guid));
            }
            fenceNodes.Add((new Fence(i + 1, j , Orientation.Right), guid));
        }
        if (j == 0)
        {
            regions[guid] = (regions[guid].area, regions[guid].perimeter + 1, regions[guid].corners);
            TraceRegion(i, j + 1, guid, plant);
            if (map[i][j + 1] != plant)
            {
                fenceNodes.Add((new Fence(i, j + 1, Orientation.Down), guid));
            }
            fenceNodes.Add((new Fence(i, j, Orientation.Down), guid));
        }
        else if (j < map[i].Length - 1)
        {
            TraceRegion(i, j + 1, guid, plant);
            TraceRegion(i, j - 1, guid, plant);
            if (map[i][j + 1] != plant)
            {
                fenceNodes.Add((new Fence(i, j + 1, Orientation.Down), guid));
            }
            if (map[i][j - 1] != plant)
            {
                fenceNodes.Add((new Fence(i, j, Orientation.Down), guid));
            }
        }
        else
        {
            regions[guid] = (regions[guid].area, regions[guid].perimeter + 1, regions[guid].corners);
            TraceRegion(i, j - 1, guid, plant);
            if (map[i][j - 1] != plant)
            {
                fenceNodes.Add((new Fence(i, j, Orientation.Down), guid));
            }
            fenceNodes.Add((new Fence(i, j + 1, Orientation.Down), guid));
        }
    }
}

bool ContainsOut(HashSet<(Fence fence, Guid region)> set, (Fence fence, Guid region) fenceToCheck, out (Fence fence, Guid region) fenceOutput)
{
    if (set.Contains(fenceToCheck))
    {
        fenceOutput = fenceToCheck;
        return true;
    }
    else
    {
        fenceOutput = default;
        return false;
    }
}

enum Orientation
{
    Right,
    Down,
}

enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

struct Fence(int i, int j, Orientation orientation)
{ 
    public int i = i; 
    public int j = j;
    public Orientation orientation = orientation;
}


