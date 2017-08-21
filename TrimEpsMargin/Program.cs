using System;
using System.IO;


namespace TrimEpsMargin {
    class Program {
        static void Main(string[] args) {
            const string cfgPath = "TrimEpsMargin.cfg";

            string cmd;
            string sourceFile;
            if (args.Length == 1) {
                cmd = TrimEpsMargin.detectGhostScriptInstallation(File.Exists(cfgPath) ? File.ReadAllText(cfgPath) : null);
                sourceFile = args[0];
            } else if (args.Length == 2) {
                cmd = TrimEpsMargin.detectGhostScriptInstallation(args[0]);
                sourceFile = args[1];
            } else if (args.Length == 0) {
                Console.WriteLine("[msg] usage:");
                Console.WriteLine("    exe SourceFile");
                Console.WriteLine("    exe CustomGhostScriptExe SourceFile");
                return;
            } else {
                Console.WriteLine("[err] no GhostScript installation is found!");
                return;
            }

            TrimEpsMargin.run(cmd, sourceFile);
        }
    }
}
