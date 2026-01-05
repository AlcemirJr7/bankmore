#r "System.Diagnostics.Process"

using System;
using System.Diagnostics;

int RunVerbose(string command)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"\nExecutando: {command}");
    Console.ResetColor();

    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-Command {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.OutputDataReceived += (s, e) =>
    {
        if (!string.IsNullOrEmpty(e.Data))
            Console.WriteLine(e.Data);
    };

    process.ErrorDataReceived += (s, e) =>
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Data);
            Console.ResetColor();
        }
    };

    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    process.WaitForExit();

    Console.ForegroundColor = process.ExitCode == 0
        ? ConsoleColor.Green
        : ConsoleColor.Red;

    Console.WriteLine($"Finalizado (ExitCode: {process.ExitCode})");
    Console.ResetColor();

    return process.ExitCode;
}

var exit1 = RunVerbose("docker-compose -f kafka.yml up -d");
if (exit1 != 0) return;

var files = new[] 
    {
        "contacorrente.yml",
        "transferencia.yml",
        "tarifa.yml",
    };

foreach (var file in files)
{
    var exit1 = RunVerbose($"docker-compose -f {file} up --build -d");
    if (exit1 != 0) return;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\nTodas as aplicações foram iniciadas com sucesso.");
Console.ResetColor();