using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Java_compiler
{
    class Program
    {
        static string checkJava(String[] dirs)
        {
            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Contains("jdk"))
                {
                    return dirs[i];
                }
            }
            return null;
        }
        static bool checkPath()
        {
            Process check = new Process();
            check.StartInfo.UseShellExecute = false;
            check.StartInfo.RedirectStandardError = true;
            check.StartInfo.FileName = "cmd.exe";
            check.StartInfo.Arguments = "/C javac";
            check.Start();
            String err = check.StandardError.ReadToEnd();
            if (!err.Contains("Usage"))
            {
                Console.WriteLine("La JDK non è correttamente impostata...");
                String[] workingdir= { "" };
                String[] workingdir86= { "" };
                String ProgFiles64 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                String ProgFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                if (System.IO.Directory.Exists(ProgFiles64))
                     workingdir = System.IO.Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)+@"\Java");
                else if (System.IO.Directory.Exists(ProgFiles))
                    workingdir86 = System.IO.Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Java");
                String vers = checkJava(workingdir);
                String versx86 = checkJava(workingdir86);
                if (vers == null && versx86 == null)
                {
                    Console.WriteLine("Sembra che tu non abbia una JDK installata...scaricala da questo sito e riapri il programma");
                    Process.Start("http://www.oracle.com/technetwork/java/javase/downloads/index-jsp-138363.html");
                } else
                {
                    Console.WriteLine("Sembra che la variabile PATH non sia configurata correttamente...rimedio");
                    String value = "";
                    if (vers!=null)
                        value = System.Environment.GetEnvironmentVariable("PATH")+ @";"+vers;
                    else
                        value = System.Environment.GetEnvironmentVariable("PATH") + @";"+versx86;
                  
                    var target = EnvironmentVariableTarget.Machine;
                    System.Environment.SetEnvironmentVariable("PATH", value, target);
                    Console.WriteLine("Tentativo di rimedio completato..riavviare il PC");      
                }
                return false;
            } return true;
        }
        static void Main(string[] args)
        {
            if (checkPath() == false)
            {
                Console.Read();
                return;
            }
            Console.WriteLine("Java Environment correttamente installato!");
            do {
                Console.WriteLine("\nTrascinare sulla console il file .java da compilare ed eseguire o premere Q per uscire.");
                String s = Console.ReadLine();
                String rep = "";
                String err = "";
                String s_dir = "";
                if (s.ToUpper() == "Q")
                {
                    break;
                }
                Process p = new Process();
                try
                {
                    s_dir = s.Replace(s.Substring(s.LastIndexOf('\\')), "");
                    if (s.Contains("\""))
                    {
                        s = s.Replace(s.Substring(1, s.LastIndexOf('\\') + 1), "");
                    }
                    else {
                        s = s.Replace(s.Substring(0, s.LastIndexOf('\\') + 1), "");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Il file inserito non è corretto: " + ex);
                    continue;
                }
                rep = s.Replace(".java", "");
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WorkingDirectory = s_dir;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/C javac " + s;
                Console.WriteLine("Compilazione in corso...");
                p.Start();
                err = p.StandardError.ReadToEnd();
                if (err=="")
                {
                    Console.WriteLine("Nessun errore di compilazione!");
                    Console.WriteLine("Eseguo il programma...\n");
                }
                else
                {
                    Console.WriteLine("Errore in fase di compilazione: \n"+ err);
                    continue;
                }
                p.WaitForExit();

                Process p2 = new Process();
                p2.StartInfo.WorkingDirectory = s_dir;
                p2.StartInfo.FileName = "CMD.EXE";
                p2.StartInfo.Arguments = "/K java " + rep;
                p2.Start();
                p2.WaitForExit();


            } while (true);
 
        }
    }
}
