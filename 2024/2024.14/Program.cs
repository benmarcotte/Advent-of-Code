using ClassLibrary;
using System.Runtime.InteropServices.Marshalling;

List<((int x, int y) position, (int x, int y) velocity)> robots = [];
//var sr = new StringReader("p=0,4 v=3,-3\r\np=6,3 v=-1,-3\r\np=10,3 v=-1,2\r\np=2,0 v=2,-1\r\np=0,0 v=1,3\r\np=3,0 v=-2,-2\r\np=7,6 v=-1,-3\r\np=3,0 v=-1,-2\r\np=9,3 v=2,3\r\np=7,3 v=-1,2\r\np=2,4 v=2,-3\r\np=9,5 v=-3,-3");
var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 14)));
{
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        string position = line.Split(' ')[0];
        string velocity = line.Split(' ')[1];
        int px = int.Parse(position.Split(',')[0][2..]);
        int py = int.Parse(position.Split(',')[1]);
        int vx = int.Parse(velocity.Split(',')[0][2..]);
        int vy = int.Parse(velocity.Split(',')[1]);

        robots.Add(((px, py), (vx, vy)));
    }
}


int q1 = 0;
int q2 = 0;
int q3 = 0;
int q4 = 0;

int width = 101;
int height = 103;
foreach (var robot in robots)
{
    int x = (robot.position.x + (100 * robot.velocity.x)) % width;
    int y = (robot.position.y + (100 * robot.velocity.y)) % height;

    if (x < 0) x = width + x;
    if (y < 0) y = height + y;

    if (x < width/2)
    {
        if (y < height/2) q1++;
        else if (y > height/2) q3++;
    }
    if (x > width/2)
    {
        if (y < height/2) q2++;
        else if (y > height / 2) q4++;
    }
}

for (int i = 0; ; i++)
{
    bool uniquePositions = true;
    HashSet<(int x, int y)> positions = [];
    foreach (var robot in robots)
    {
        int x = (robot.position.x + (i * robot.velocity.x)) % width;
        int y = (robot.position.y + (i * robot.velocity.y)) % height;

        if (x < 0) x = width + x;
        if (y < 0) y = height + y;

        if (positions.Contains((x, y)))
        {
            uniquePositions = false;
            break;
        }
        positions.Add((x, y));
    }
    if (uniquePositions)
    {
        Console.WriteLine($"Easter Egg found at {i} seconds");
        break;
    }
}

Console.WriteLine($"Total safety factor: {q1*q2*q3*q4}");
