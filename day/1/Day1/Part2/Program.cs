using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
;

// keep a total we'll add to on each line
int total = 0;

Regex forwardRegex = new("one|two|three|four|five|six|seven|eight|nine");
List<string> digitWords = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

// open and iterate over each line of the file
await foreach (string line in File.ReadLinesAsync("./input.txt"))
{
    Console.WriteLine(line);
    int firstDigit = GetFirstIntegerValue(line);
    Console.WriteLine($"first digit: {firstDigit}");
    int lastDigit = GetLastIntegerValue(line);
    Console.WriteLine($"last digit: {lastDigit}");

    // add both to the total
    int combinedDigits = int.Parse($"{firstDigit}{lastDigit}");
    Console.WriteLine($"combined digits: {combinedDigits}");
    total += combinedDigits;
    Console.WriteLine("-----------");
}

Console.WriteLine($"the total is: {total}");

int GetFirstIntegerValue(string value)
{
    var result = forwardRegex.Match(value);
    if (result.Success)
    {
        //Console.WriteLine($"Has a word match: {result.Value} at index: {result.Index}");
        // has a word match, is it first over any digits?
        var firstDigit = GetFirstDigitAndIndex(value);
        if (firstDigit.index < result.Index)
        {
            // digit is first
            return firstDigit.digit;
        }
        else
        {
            // word is first
            // parse the word to an int
            return WordToInt(result.Value);
        }
    }
    else
    {
        return GetFirstDigitAndIndex(value).digit;
    }
}

int GetLastIntegerValue(string value)
{
    var result = FindLastWord(value);
    if (!string.IsNullOrWhiteSpace(result.word))
    {
        // has a word match, are there any digits after that match?
        var lastDigit = GetLastDigitAndIndex(value);
        if (lastDigit.index > result.index)
        {
            // digit is last
            return lastDigit.digit;
        }
        else
        {
            // word is last
            // parse the word to an int
            return WordToInt(result.word);
        }
    }
    else
    {
        return GetLastDigitAndIndex(value).digit;
    }
}

static int WordToInt(string word)
{
    return word switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => throw new Exception($"{word} not an accepted word")
    };
}

static (int digit, int index) GetFirstDigitAndIndex(string input)
{
    char digit = input.First(c => char.IsDigit(c));
    int value = int.Parse(digit.ToString());
    return (value, input.IndexOf(digit));
}

static (int digit, int index) GetLastDigitAndIndex(string input)
{
    char digit = input.Reverse().First(c => char.IsDigit(c));
    int value = int.Parse(digit.ToString());
    return (value, input.LastIndexOf(digit));
}

// I tried using a regex for this and gave up on it because
// I couldn't get it to work properly before I ran out of care.
(string? word, int? index) FindLastWord(string inputString)
{
    List<(string word, int index)> foundWords = new();
    foreach (var word in digitWords)
    {
        int lastIndex = inputString.LastIndexOf(word);
        if (lastIndex != -1)
        {
            foundWords.Add(new(word, lastIndex));
            Console.WriteLine($"Last occurrence of '{word}': Index {lastIndex}");
        }
        else
        {
            Console.WriteLine($"'{word}' not found in the string.");
        }
    }
    if(foundWords.Any()){
        return foundWords.OrderByDescending(fw => fw.index).FirstOrDefault();
    }
    return (null, null);
}