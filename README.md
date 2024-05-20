# Jass Obfuscator
A basic obfuscation tool for JASS scripts in WC3.

[![NuGet downloads](https://img.shields.io/nuget/dt/JASS-Obfuscator.svg)](https://www.nuget.org/packages/JASS-Obfuscator/)
[![NuGet version](https://img.shields.io/nuget/v/JASS-Obfuscator.svg)](https://www.nuget.org/packages/JASS-Obfuscator/)

## Usage:

Does not support vJass. Use JassHelper to transform vJass to Jass before passing the string to this obfuscator.

``` cs
using JassObfuscator;

string obfuscated = Obfuscator.Obfuscate(script, pathToCommonJ, pathToBlizzardJ);
```

## Dependencies

[![dotnet standard 2.1](https://img.shields.io/badge/.NET%20standard-v2.1-brightgreen.svg)](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1)