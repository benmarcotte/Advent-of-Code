using ClassLibrary;

List<(int size, int spacing, int id)> diskMap = [];
List<(int size, int spacing, int id, bool check)> defragmentedDiskMap = [];

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 9)));
{
    int id = 0;
    string line = sr.ReadLine();
    for (int i = 0; i < line.Length - 1; i+= 2)
    {
        diskMap.Add((int.Parse(line[i].ToString()), int.Parse(line[i+1].ToString()), id));
        defragmentedDiskMap.Add((int.Parse(line[i].ToString()), int.Parse(line[i+1].ToString()), id, false));
        id++;
    }
    diskMap.Add((int.Parse(line[^1].ToString()), 0, id));
    defragmentedDiskMap.Add((int.Parse(line[^1].ToString()), 0, id, false));
}

List<int> cache = [];

while (diskMap.Count > 0)
{
    if (diskMap[0].size > 0)
    {
        cache.Add(diskMap[0].id);
        diskMap[0] = (diskMap[0].size - 1, diskMap[0].spacing, diskMap[0].id);
    }
    else if (diskMap[0].spacing > 0)
    {
        if (diskMap.Count > 1)
        {
            cache.Add(diskMap[^1].id);
            diskMap[^1] = (diskMap[^1].size - 1, diskMap[^1].spacing, diskMap[^1].id);
            diskMap[0] = (diskMap[0].size, diskMap[0].spacing - 1, diskMap[0].id);
            if (diskMap[^1].size == 0) diskMap.RemoveAt(diskMap.Count - 1);
        }
        else diskMap.RemoveAt(0);
    }
    else
    {
        diskMap.RemoveAt(0);
    }
}

long checksum = 0;

for (int i = 0; i < cache.Count; i++)
{
    checksum += cache[i] * i;
}

Console.WriteLine($"Checksum total after compacting: {checksum}");

while (true)
{
    (int size, int spacing, int id, bool check) current = (0, 0, 0, false);
    bool found = false;
    for (int i = defragmentedDiskMap.Count - 1; i >= 0; i--)
    {
        if (!defragmentedDiskMap[i].check)
        {
            current = defragmentedDiskMap[i];
            found = true;
            break;
        }
    }
    if (found)
    {
        foreach (var fileToMoveAfter in defragmentedDiskMap)
        {
            if (fileToMoveAfter.id == current.id)
            {
                defragmentedDiskMap[defragmentedDiskMap.IndexOf(current)] = (current.size, current.spacing, current.id, true);
                break;
            }
            if (fileToMoveAfter.spacing >= current.size)
            {
                var beforeCurrent = defragmentedDiskMap[defragmentedDiskMap.IndexOf(current) - 1];
                if (beforeCurrent == fileToMoveAfter)
                {
                    defragmentedDiskMap[defragmentedDiskMap.IndexOf(current)] = (current.size, beforeCurrent.spacing + current.spacing, current.id, true);
                    defragmentedDiskMap[defragmentedDiskMap.IndexOf(beforeCurrent)] = (beforeCurrent.size, 0, beforeCurrent.id, beforeCurrent.check);
                }
                else
                {
                    defragmentedDiskMap[defragmentedDiskMap.IndexOf(beforeCurrent)] = (beforeCurrent.size, beforeCurrent.spacing + current.spacing + current.size, beforeCurrent.id, beforeCurrent.check);
                    defragmentedDiskMap.Remove(current);
                    current = (current.size, fileToMoveAfter.spacing - current.size, current.id, true);
                    defragmentedDiskMap.Insert(defragmentedDiskMap.IndexOf(fileToMoveAfter) + 1, current);
                    defragmentedDiskMap[defragmentedDiskMap.IndexOf(fileToMoveAfter)] = (fileToMoveAfter.size, 0, fileToMoveAfter.id, fileToMoveAfter.check);
                }
                break;
            }
        }
    }
    else break;
}

List<int> defragmentedCache = [];

foreach (var file in defragmentedDiskMap)
{
    for (int i = 0; i < file.size; i++)
    {
        defragmentedCache.Add(file.id);
    }
    for (int i = 0; i < file.spacing; i++)
    {
        defragmentedCache.Add(0);
    }
}

long defragmentedChecksum = 0;

for (int i = 0; i < defragmentedCache.Count; i++)
{
    defragmentedChecksum += defragmentedCache[i] * i;
}

Console.WriteLine($"Checksum total after defragmenting: {defragmentedChecksum}");