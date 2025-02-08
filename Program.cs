using System;
using System.IO;
using System.Linq;

// notes at bottom of code

class Program
{
    static string[] lines;

    static void Main()
    {
        string filePath = "input.csv";
        lines = File.ReadAllLines(filePath);

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Add Character");
            Console.WriteLine("3. Level Up Character");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllCharacters(lines);
                    break;
                case "2":
                    AddCharacter(ref lines);
                    break;
                case "3":
                    LevelUpCharacter(ref lines);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllCharacters(string[] lines)
    {
        // Skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] fields = ParseCsvLine(line);

            string name = fields[0];
            string characterClass = fields[1];
            int level = int.Parse(fields[2]);
            int hitPoints = int.Parse(fields[3]);
            string[] equipment = fields[4].Split('|');

            // Display character information
            Console.WriteLine($"Name: {name}, Class: {characterClass}, Level: {level}, HP: {hitPoints}, Equipment: {string.Join(", ", equipment)}");
        }
    }

    static string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        bool inQuotes = false;
        string currentField = string.Empty;

        foreach (char c in line)
        {
            if (c == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(currentField);
                currentField = string.Empty;
            }
            else
            {
                currentField += c;
            }
        }

        fields.Add(currentField);
        return fields.ToArray();
    }

    static void AddCharacter(ref string[] lines)
    {
        // TODO: Implement logic to add a new character
        // Prompt for character details (name, class, level, hit points, equipment)
        // DO NOT just ask the user to enter a new line of CSV data or enter the pipe-separated equipment string
        // Append the new character to the lines array

        using (StreamWriter writer = new StreamWriter("input.csv", true))
        {
            Console.WriteLine("Add new character's name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Add new character's occupation: ");
            string characterClass = Console.ReadLine();
            Console.WriteLine("Add new character's level: ");
            int level = int.Parse(Console.ReadLine());
            Console.WriteLine("Add new character's hit points: ");
            int hitPoints = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new character's equipment: ");
            string equipment = Console.ReadLine();

            writer.WriteLine($"\r\n{name},{characterClass},{level},{hitPoints},{equipment}");
        }

        Console.WriteLine("New character added and saved to file.");
    }

    static void LevelUpCharacter(ref string[] lines)
    {
        string filePath = "input.csv";

        Console.Write("Enter the name of the character to level up: ");
        string nameToLevelUp = Console.ReadLine();

        // Change for Git
        // Loop through characters to find the one to level up
        for (int i = 1; i < lines.Length; i++)
        {
            string characterLine = lines[i];
            string[] fields = ParseCsvLine(characterLine);

            string name = fields[0].Trim('\"');

            // TODO: Check if the name matches the one to level up
            // Do not worry about case sensitivity at this point
            if (name == nameToLevelUp)
            {
                // TODO: Split the rest of the fields locating the level field
                int level;
                if (int.TryParse(fields[2], out level))
                {
                    level++;
                    Console.WriteLine($"Character {name} leveled up to level {level}!");
                    fields[2] = level.ToString();
                    lines[i] = string.Join(",", fields);

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (var line in lines)
                        {
                            writer.WriteLine(line);
                        }
                    }

                    break;
                }
            }
        }
    }
}



// Wrote much of the code but used ChatGPT and CoPilot in attempts to clean it up
// Specifics: In LevelUpCharacters method, I was trying to put the using StreamWriter outside of the for loop
// ChatGPT informed me because it could "potentially overwrite the files"
//Specifics: In my original code for the level up characters method -
// "fields was being created by line.Split(","), which would work fine if the line did not contain commas inside quoted fields."
// ^^^ Is what ChatGPT told me
// Must enter name of character exactly as is in CSV file to level up successfully
// Additionally, you will see when you run the display characters method,
// I attempted to use both copilot and chatgpt to help figure out the commas issue but to no avail