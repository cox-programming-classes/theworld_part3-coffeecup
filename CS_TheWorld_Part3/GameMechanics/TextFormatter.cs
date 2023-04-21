namespace CS_TheWorld_Part3.GameMechanics;

public static class TextFormatter
{
    private static ConsoleColor PositiveColor => ConsoleColor.Green;
    private static ConsoleColor WarningColor => ConsoleColor.Yellow;
    private static ConsoleColor NegativeColor => ConsoleColor.Red;
    private static ConsoleColor NeutralColor => ConsoleColor.Cyan;
    private static ConsoleColor SurpriseColor => ConsoleColor.Magenta;

    public static void Write(ConsoleColor color, string message)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = prevColor;
    }

    public static string GetPlayerInput(string? promptMessage = null)
    {
        if(!string.IsNullOrEmpty(promptMessage))
            WriteLineNeutral(promptMessage);
        
        WriteNeutral(">> ");
        
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = PositiveColor;
        var response = Console.ReadLine();
        Console.ForegroundColor = prevColor;
        return response!;
    }
    
    public static void WriteLine(ConsoleColor color, string message)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = prevColor;
    }

    public static void WritePositive(string message) => Write(PositiveColor, message);
    public static void WriteLinePositive(string message) => WriteLine(PositiveColor, message);
    
    public static void WriteWarning(string message) => Write(WarningColor, message);
    public static void WriteLineWarning(string message) => WriteLine(WarningColor, message);
    
    public static void WriteNegative(string message) => Write(NegativeColor, message);
    public static void WriteLineNegative(string message) => WriteLine(NegativeColor, message);
    
    public static void WriteNeutral(string message) => Write(NeutralColor, message);
    public static void WriteLineNeutral(string message) => WriteLine(NeutralColor, message);
    
    public static void WriteSurprise(string message) => Write(SurpriseColor, message);
    public static void WriteLineSurprise(string message) => WriteLine(SurpriseColor, message);
}