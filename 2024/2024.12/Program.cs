using ClassLibrary;
using System.Formats.Asn1;
using System.Runtime.ExceptionServices;

List<string> map = [];
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
Dictionary<Guid, (int area, int perimeter, int corners)> regions = [];
List<Guid> guids = [];

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
        }
    }
}

// TODO Group by sides, by storing the verts along the same i together and the horizontales along the same j together
foreach (var guid in guids)
{
    var trailStart = fenceNodes.Where(fence => fence.region == guid).First();
    (Fence fence, Guid region) trail = (new Fence(-1, -1, Orientation.Right), new Guid());
    var currentOrientation = Direction.None;
    while (!trail.Equals(trailStart))
    {
        
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
            fenceNodes.Add((new Fence(i, j + 1, Orientation.Right), guid));
        }
        if (j == 0)
        {
            regions[guid] = (regions[guid].area, regions[guid].perimeter + 1, regions[guid].corners);
            TraceRegion(i, j + 1, guid, plant);
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
            fenceNodes.Add((new Fence(i, j, Orientation.Down), guid));
        }
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
    int i = i; 
    int j = j;
    Orientation orientation = orientation;
}

