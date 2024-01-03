// anything but digits, . and newline are symbols

// easier to check around the symbols for digits, rather than the other way around


// bigger file would need something that doesn't read all lines at once
using System.Text.RegularExpressions;


//string[] allLines = await File.ReadAllLinesAsync("./test_input.txt");
string[] allLines = await File.ReadAllLinesAsync("./input.txt");

static bool IsNonSymbolChar(char currentChar)
{
    return char.IsDigit(currentChar) || currentChar == '.' || char.IsControl(currentChar);
}

Dictionary<int, List<Match>> matches = new();

// this is a symbol centric solution - look in each line for symbols, then look around the symbol for digits, if digits are found, process out into a number
// and create a Find object. After we have processed all lines, we can add up the found numbers and print the result
void ProcessFile()
{
    for (int i = 0; i < allLines.Length; i++)
    {

        Console.WriteLine($"processing line: {i + 1}: {allLines[i]}");
        string current = allLines[i];
        string? previousLine = i > 0 ? allLines[i - 1] : null;
        string? nextLine = i < allLines.Length - 1 ? allLines[i + 1] : null;
        int lineNumber = i + 1;

        // idea, look through each char of the string, when found, look around the character
        // for digits, and if found, grab that number. Use a found list to avoid double counting
        for (int j = 0; j < current.Length; j++)
        {
            // could do a regex and iterate over matches to avoid iterating over the string
            char currentChar = current[j];

            // check if currentChar is a digit, a period, or a newline, if so, continue to next char
            if (IsNonSymbolChar(currentChar))
            {
                //Console.WriteLine($"ignoring char: {currentChar}");
                continue;
            }
            //Console.WriteLine($"found symbol: {currentChar} at line: {i + 1} position: {j + 1}");

            // found a symbol, look around it for digits
            // this algorithm is very brute force in 
            var currentSlice = current.AsSpan().Slice(j - 1, 3);
            ProcessForMatches(currentSlice, current, lineNumber, j);
            var previousSlice = previousLine?.Substring(j - 1, 3);
            if (previousSlice != null && previousLine != null)
            {
                ProcessForMatches(previousSlice, previousLine, lineNumber - 1, j);
            }
            var nextSlice = nextLine?.Substring(j - 1, 3);
            if (nextSlice != null && nextLine != null)
            {
                ProcessForMatches(nextSlice, nextLine, lineNumber + 1, j);
            }
        }
    }
}

void ProcessForMatches(ReadOnlySpan<char> slice, string line, int lineNumber, int symbolIndex)
{
    // see if the slice has digits, if not, return
    if (!HasDigit(slice))
    {
        return;
    }

    //Console.WriteLine($"slice has digits: {slice}");

    // the slice has digits, so this line has some numbers that are part numbers
    // match strings that consist only of numbers
    // would be nice to constrain where we're looking instead of looking at the whole string
    string pattern = @"\d+";

    foreach (var match in Regex.Matches(line, pattern).Cast<Match>())
    {
        //Console.WriteLine($"Regex match: {match.ToString()}");
        int startIndex = match.Index;
        int endIndex = match.Index + match.Value.Length - 1;

        // the match is near the symbol when any one of its characters are within 1 character of the symbol index
        bool startIsNearSymbol = Math.Abs(symbolIndex - startIndex) <= 1;
        bool endIsNearSymbol = Math.Abs(symbolIndex - endIndex) <= 1;
        // or if the match overlaps the symbol index - eg longer than 3 chars
        bool symbolInMiddleOfMatch = symbolIndex > startIndex && endIndex > symbolIndex;
        if (startIsNearSymbol || endIsNearSymbol || symbolInMiddleOfMatch)
        {
            // we have a number that is next to the symbol, try to add it
            AddMatchIfNotAlreadyAdded(match, lineNumber);
        }
    }
}

void AddMatchIfNotAlreadyAdded(Match match, int lineNumber)
{
    if (!matches.ContainsKey(lineNumber))
    {
        matches.Add(lineNumber, []);
    }
    // see if we have already found this number, if so, return, otherwise add it to the list
    if (matches[lineNumber].Any(f => f == match)) // don't need to check value
    {
        Console.WriteLine($"number: {match.Value} already found, skipping");
        return;
    }
    Console.WriteLine($"Found part number: {match.Value} on line {lineNumber}");

    matches[lineNumber].Add(match);
}

ProcessFile();

Console.WriteLine("printing finds");
int sum = 0;
foreach (var matchPair in matches)
{
    //Console.WriteLine($"line: {line.Key}");
    foreach (var lineMatches in matchPair.Value)
    {
        sum += int.Parse(lineMatches.Value);
        Console.WriteLine($"value: {lineMatches.Value} line: {matchPair.Key} start position: {lineMatches.Index}");
    }
}
ProcessFile();
// answer was 509115
Console.WriteLine($"sum: {sum}");

bool HasDigit(ReadOnlySpan<char> input)
{
    foreach (char c in input)
    {
        if (char.IsDigit(c))
        {
            return true;
        }
    }
    return false;
}