using JassObfuscator;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class ObfuscateTest
    {
        private static string pathCommonJ;
        private static string pathBlizzardJ;
        private static string pathJasshelper;

        private static string pathOptimizedDir;
        private static string pathTestsDir;
        private static string pathJasshelperOutput;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            string jasshelperDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources/JassHelper");
            pathCommonJ = Path.Combine(jasshelperDir, "common.txt");
            pathBlizzardJ = Path.Combine(jasshelperDir, "Blizzardj.txt");
            pathJasshelper = Path.Combine(jasshelperDir, "clijasshelper.exe");

            pathOptimizedDir = Path.Combine(Directory.GetCurrentDirectory(), "Tests");
            pathJasshelperOutput = Path.Combine(pathOptimizedDir, "war3map.j");
            
            if (Directory.Exists(pathOptimizedDir))
            {
                Directory.Delete(pathOptimizedDir, true);
            }
            Directory.CreateDirectory(pathOptimizedDir);
        }

        [DataTestMethod]
        [DataRow("war3map1.txt")]
        [DataRow("war3map2.txt")]
        [DataRow("war3map3.txt")]
        [DataRow("war3map4.txt")]
        [DataRow("war3map5.txt")]
        [DataRow("war3map6.txt")]
        [DataRow("war3map7.txt")]
        [DataRow("war3map8.txt")]
        [DataRow("war3map9.txt")]
        [DataRow("war3map10.txt")]
        [DataRow("war3map11.txt")]
        [DataRow("war3map12.txt")]
        [DataRow("war3map13.txt")]
        [DataRow("war3map14.txt")]
        [DataRow("war3map15.txt")]
        [DataRow("war3map16.txt")]
        [DataRow("war3map17.txt")]
        [DataRow("war3map18.txt")]
        [DataRow("war3map19.txt")]
        [DataRow("war3map20.txt")]
        [DataRow("war3map21.txt")]
        [DataRow("war3map22.txt")]
        [DataRow("war3map23.txt")]
        public void Test(string file)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "Resources/Scripts");
            string filePath = Path.Combine(dir, file);
            string optimizedPath = Path.Combine(pathOptimizedDir, file);

            string script = File.ReadAllText(filePath);
            script = Obfuscator.Obfuscate(script, pathCommonJ, pathBlizzardJ);
            File.WriteAllText(optimizedPath, script);

            Process p = new();
            ProcessStartInfo startInfo = new();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = pathJasshelper;
            startInfo.Arguments = $"--scriptonly \"{pathCommonJ}\" \"{pathBlizzardJ}\" \"{optimizedPath}\" \"{pathJasshelperOutput}\"";
            startInfo.RedirectStandardOutput = true;
            p.StartInfo = startInfo;
            p.Start();
            p.WaitForExit();
            string message = p.StandardOutput.ReadToEnd();
            bool success = p.ExitCode == 0;
            p.Kill();

            Assert.IsTrue(success, message);
        }
    }
}