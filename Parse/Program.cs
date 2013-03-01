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
            int averageOver = 100;

            for (int h = 2; h <= 8; h++)
            {
                for (int i = 1; i <= 3; i++)
                {
                    string runargs = (Math.Pow(2, i)).ToString() + " " + h.ToString() + " " + averageOver.ToString();
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
                var arguments = p.StartInfo.Arguments.Split(' ');
                string row = Path.GetFileName(p.StartInfo.FileName) + ',' + arguments[0] + ',' + arguments[1];
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
