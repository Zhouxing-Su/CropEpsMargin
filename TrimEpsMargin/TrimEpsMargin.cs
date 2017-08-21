using System.Diagnostics;
using System.IO;


namespace TrimEpsMargin {
    public class TrimEpsMargin {
        public static void run(string ghostScriptExe, string sourceFile, string targetFile, bool unixLineEnding = true) {
            const string LF = "\n";
            const string BoundingBoxLinePrefix = "%%BoundingBox";

            string[] lines = File.ReadAllLines(sourceFile);
            for (int i = 0; i < lines.Length; ++i) {
                if (lines[i].StartsWith(BoundingBoxLinePrefix)) {
                    lines[i] = calculateNewBoundingBox(ghostScriptExe, sourceFile);
                    if (unixLineEnding) {
                        File.WriteAllText(targetFile, string.Join(LF, lines));
                    } else {
                        File.WriteAllLines(targetFile, lines);
                    }
                    return;
                }
            }
        }
        public static void run(string ghostScriptExe, string sourceFile, bool unixLineEnding = true) {
            run(ghostScriptExe, sourceFile, sourceFile, unixLineEnding);
        }


        public static string detectGhostScriptInstallation(string customPath = null) {
            if ((customPath != null) && File.Exists(customPath)) { return customPath; }

            string[] GhostScriptExeNames = new string[] {
                "/gswin32c.exe",
                "/gswin64c.exe",
            };
            string[] GhostScriptExeDirs = new string[] {
                // current directory.
                ".",
                // independent installation.
                "C:/Program Files/gs/gs9.15/bin",
                "C:/Program Files (x86)/gs/gs9.15/bin",
                // texlive.
                "C:/texlive/2016/tlpkg/tlgs/bin",
                // ctex.
                "C:/CTEX/Ghostscript/gs9.00/bin",
            };

            foreach (var name in GhostScriptExeNames) {
                foreach (var dir in GhostScriptExeDirs) {
                    if (File.Exists(dir + name)) { return dir + name; }
                }
            }

            return null;
        }

        public static string calculateNewBoundingBox(string ghostScriptExe, string sourceFile) {
            const string GhostScriptExeOption = " -dNOPAUSE -dBATCH -q -sDEVICE=bbox ";
            //const string RedirectStderrToTmpFile = @" 2>szx.tmp.gs.bbox.txt";
            const string RedirectStderrToTmpFile = "";

            using (Process p = new Process()) {
                //p.StartInfo.CreateNoWindow = true;
                //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;

                p.StartInfo.FileName = ghostScriptExe;
                p.StartInfo.Arguments = GhostScriptExeOption + quote(sourceFile) + RedirectStderrToTmpFile;
                //p.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

                p.Start();

                return p.StandardError.ReadLine();
            }
        }


        public static string quote(string str) { return ('\"' + str + '\"'); }
    }
}
