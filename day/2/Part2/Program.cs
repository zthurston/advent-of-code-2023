using System.ComponentModel;
using System.Text.RegularExpressions;

// bag with rgb cubes
// each play secret number of cubes of each color
// goal to figure out info about the number of cubes

// reach into the bag, grab random number of cubes, show, put back in bag

// game int id

int totalPowers = 0;

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
    var maxInRound = new Dictionary<string, int>{
        {"red", 0 },
        {"blue", 0 },
        {"green", 0}
    };

    foreach (string game in semiColonSplit)
    {
        Console.WriteLine(game);

        string pattern = @"(\d+)\s(\w+)"; // This regex pattern matches a number followed by a word

        MatchCollection matches = Regex.Matches(game, pattern);

        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Groups.Count == 3) // Ensuring that both number and color are captured
            {
                int number = int.Parse(match.Groups[1].Value);
                string color = match.Groups[2].Value;
                if(maxInRound[color] < number){
                    maxInRound[color] = number;
                }
            }
        }
    }
    Console.WriteLine($"Max {maxInRound["red"]} red, {maxInRound["blue"]} blue, {maxInRound["green"]} green");
    int power = maxInRound["red"] * maxInRound["green"] * maxInRound["blue"];
    totalPowers += power;
    //matchedIdTotal += ReturnIdIfValid(id, maxInRound);
    //Console.WriteLine("matchedIdTotal: " + matchedIdTotal);
    //Console.ReadKey();
}

Console.WriteLine($"Total power: {totalPowers}");
