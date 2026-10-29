// Ask for the user's name
Console.Write("What is your name? ");
string name = (Console.ReadLine() ?? "").Trim();

// Ask for the user's age
Console.Write("How old are you? ");
string age = (Console.ReadLine() ?? "").Trim();

// Display a personalized message that includes both pieces of information
Console.WriteLine($"Hello, {name}! You are {age} years old.");