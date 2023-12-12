using System.ComponentModel;
using System.Text.RegularExpressions;

// bag with rgb cubes
// each play secret number of cubes of each color
// goal to figure out info about the number of cubes

// reach into the bag, grab random number of cubes, show, put back in bag

// game int id

int matchedIdTotal = 0;

var maxValues = new Dictionary<string, int>{
    {"red", 12 },
    {"green", 13 },
    {"blue", 14 },
};
await foreach (string line in File.ReadLinesAsync("./input.txt"))
{
    // get the game id (left of colon)
    string[] colonSplit = line.Split(':');
    //Console.WriteLine(colonSplit[0]);
    int id = int.Parse(colonSplit[0].Remove(0, 5));
    //Console.WriteLine(id);

    // split remainder of string on semicolon
    string[] semiColonSplit = colonSplit[1].Split(';');
    bool allRoundsPassed = true;
    foreach (string round in semiColonSplit)
    {
        // its invalid if an individual hand has more than max, not all in the line
        var foundColors = new Dictionary<string, int>{
            {"red", 0 },
            {"blue", 0 },
            {"green", 0}
        };
        Console.WriteLine(round);

        string pattern = @"(\d+)\s(\w+)"; // This regex pattern matches a number followed by a word

        MatchCollection matches = Regex.Matches(round, pattern);

        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Groups.Count == 3) // Ensuring that both number and color are captured
            {
                int number = int.Parse(match.Groups[1].Value);
                string color = match.Groups[2].Value;
                foundColors[color] += number;
                //Console.WriteLine($"Number: {number}, Color: {color}");
            }
        }
        //Console.WriteLine($"Found {foundColors["red"]} red, {foundColors["blue"]} blue, {foundColors["green"]} green");
        foreach (var color in foundColors)
        {
            // abort if any is over the max
            if (maxValues[color.Key] < color.Value)
            {
                Console.WriteLine($"{color.Value} is too much of {color.Key}, game {id} is inplausible.");
                allRoundsPassed = false;
                break;
            }
            else
            {
                //Console.WriteLine($"saw {color.Value} of {color.Key}, less than the max of {maxValues[color.Key]}");
            }
        }
        if (!allRoundsPassed)
        {
            break;
        }
    }
    if (allRoundsPassed)
    {
        Console.WriteLine($"Adding game id: {id} to total");
        matchedIdTotal += id;
    }
    //Console.WriteLine("matchedIdTotal: " + matchedIdTotal);
    //Console.ReadKey();
}

Console.WriteLine($"Total of valid game ids: {matchedIdTotal}");
