// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using JassObfuscator;

string currentDir = Directory.GetCurrentDirectory();
string jasshelper = Path.Combine(currentDir, "JassHelper/clijasshelper.exe");
string PathCommonJ = Path.Combine(currentDir, "JassHelper/common.txt");
string PathBlizzardJ = Path.Combine(currentDir, "JassHelper/Blizzardj.txt");
string PathScript = Path.Combine(currentDir, "C:\\Users\\Lasse\\Desktop\\war3map.j");

string script = File.ReadAllText(PathScript);
var optimized = Obfuscator.Obfuscate(script, PathCommonJ, PathBlizzardJ);
string optimizedPath = Path.Combine(currentDir, "C:\\Users\\Lasse\\Desktop\\optimized.j");
File.WriteAllText(optimizedPath, optimized);
string outputPath = Path.Combine(currentDir, "C:\\Users\\Lasse\\Desktop\\output.j");

Process p = new();
ProcessStartInfo startInfo = new();
startInfo.CreateNoWindow = true;
startInfo.FileName = jasshelper;
startInfo.Arguments = $"--scriptonly \"{PathCommonJ}\" \"{PathBlizzardJ}\" \"{optimizedPath}\" \"{outputPath}\"";
startInfo.RedirectStandardOutput = true;
p.StartInfo = startInfo;
p.Start();
p.WaitForExit();
string message = p.StandardOutput.ReadToEnd();
bool success = p.ExitCode == 0;
p.Kill();

if (success)
{
    Console.WriteLine("Success!");
}
else
{
    Console.WriteLine(message);
}