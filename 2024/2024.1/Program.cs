using ClassLibrary;

List<int> A = [];
List<int> B = [];

var sr = new StreamReader(await InputManager.GetInputAsync(new DateTime(2024, 12, 1)));

var line = sr.ReadLine();

while(line != null)
{
    var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    A.Add(int.Parse(nums[0]));
    B.Add(int.Parse(nums[1]));
    line = sr.ReadLine();
}

sr.Close();
A.Sort();
B.Sort();

int difference = 0;

for(int i = 0; i < A.Count; i++)
{
    difference += Math.Abs(A[i] - B[i]);
}

Console.WriteLine($"difference: {difference}");

int similarity = 0;

for (int i = 0; i < B.Count; i++)
{
    foreach (int j in B)
    {
        if (A[i] == j)
        {
            similarity += j;
        }
    }
}

Console.WriteLine($"similarity: {similarity}");
            
