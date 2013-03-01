using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> processList = new List<Process>();
            string alg = "graph_distance.exe";

            for (int h = 2; h <= 8; h++)
            {
                for (int i = 1; i <= 3; i++)
                {
                    string runargs = (Math.Pow(2, i)).ToString() + " " + h.ToString();
                    var proce = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = @"C:\Users\Max\Documents\GitHub\hpc\Release\" + alg,
                            Arguments = runargs,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    processList.Add(proce);
                    //Console.WriteLine(proce.StartInfo.FileName);
                }
            }

            StreamWriter file = new System.IO.StreamWriter("output.csv");
            file.WriteLine("Algorithm, Prob Size N, CPUs, Time Orig, Time Tbb, Time Opt, Time Seq");
            foreach (var p in processList)
            {
                string row = Path.GetFileName(p.StartInfo.FileName) + ',' + p.StartInfo.Arguments.Replace(' ', ',');
                p.Start();
                while (!p.StandardOutput.EndOfStream)
                {
                    string line = p.StandardOutput.ReadLine();
                    row += ',' + line;
                }
                p.WaitForExit();
                Console.WriteLine(row);
                file.WriteLine(row);
            }
            file.Close();
        }
    }
}
