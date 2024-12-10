

using ClassLibrary;

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 5)));
{
    var rules = new Dictionary<int, List<int>>();
    string line;
    while ((line = sr.ReadLine()) != "")
    {
        var rule = line.Split('|');
        int key = int.Parse(rule[0]);
        int value = int.Parse(rule[1]);
        if (!rules.ContainsKey(key))
        {
            rules.Add(key, []);
        }
        rules[key].Add(value);
    }
    int middlePageSums = 0;
    int sortedPageSums = 0;
    while ((line = sr.ReadLine()) != null)
    {
        bool pageIsValid = true;
        var pages = line.Split(',');
        var pageIntegers = new List<int>();
        foreach (var page in pages)
        {
            pageIntegers.Add(int.Parse(page));
        }
        for (int i = 0; i < pageIntegers.Count; i++)
        {
            for(int j = 0; j < i; j++)
            {
                if (!rules[pageIntegers[j]].Contains(pageIntegers[i]))
                {
                    pageIsValid = false;
                    break;
                }
            }
            if (!pageIsValid) break;
        }
        if (pageIsValid) middlePageSums += pageIntegers[pageIntegers.Count / 2];
        else
        {
            pageIntegers.Sort(new PageRuling(rules));
            sortedPageSums += pageIntegers[pageIntegers.Count / 2];
        }
    }
    Console.Out.WriteLine($"Sum of valid middle pages: {middlePageSums}");
    Console.Out.WriteLine($"Sum of sorted middle pages: {sortedPageSums}");
}



public class PageRuling(Dictionary<int, List<int>> rules) : Comparer<int>
{
    Dictionary<int, List<int>> rules = rules;

    override public int Compare(int x, int y)
    {
        if (rules[x].Contains(y)) return -1;
        else return 1;
    }
}