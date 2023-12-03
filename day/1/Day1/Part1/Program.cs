using System.IO;
using System.Linq;

// keep a total we'll add to on each line
int total = 0;

// open and iterate over each line of the file
await foreach (string line in File.ReadLinesAsync("./input.txt")){
    // a regex would be more efficient but would be more annoying to write... 
    // so here's a linq solution!
    
    // start at the front of the string and iterate until finding a digit
    Console.WriteLine(line);
    char firstDigit = line.First(c => char.IsDigit(c));
    Console.WriteLine($"first digit is: {firstDigit}");

    // start and the end of the string and iterate in reverse until finding a digit
    char lastDigit = line.Reverse().First(c => char.IsDigit(c));
    Console.WriteLine($"last digit is: {lastDigit}");

    // add both to the total
    int combinedDigits = int.Parse($"{firstDigit}{lastDigit}");
    Console.WriteLine($"combined digits: {combinedDigits}");
    total += combinedDigits;
}

Console.WriteLine($"the total is: {total}");