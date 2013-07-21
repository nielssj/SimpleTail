using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTail
{
    public class FileArguments
    {
        public string path;
        public bool follow;
        // TODO: Add further fields when more options are supported
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<FileArguments> fileList = new List<FileArguments>();
            FileArguments fileArgs = new FileArguments();

            // Collect arguments, if any
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    // Is the argument an option or file?
                    if (arg.StartsWith("-"))
                    {
                        // OPTION:
                        switch (arg.ToLower())
                        {
                            case "-f":
                                fileArgs.follow = true;
                                break;
                            // TODO: Support more options such as '-n', '--retry' and '-s'
                        }
                    }
                    else
                    {
                        // FILE:
                        fileArgs.path = arg;
                        fileList.Add(fileArgs);
                        fileArgs = new FileArguments(); // Prep a new args object for further files
                    }
                }
            }

            // If no arguments, prompt on stdout
            if (fileList.Count == 0)
            {
                fileArgs = new FileArguments();

                Console.WriteLine("Which file do you wish to print tail of?");
                fileArgs.path = Console.ReadLine();

                Console.WriteLine("Do you wish to follow the file? (y/n) (Outputs appended data as the file grows)");
                string fInput = Console.ReadLine();
                if (fInput == "y" || fInput == "yes")
                {
                    fileArgs.follow = true;
                }

                fileList.Add(fileArgs);
            }

            new SimpleTail(fileList.ToArray());
        }
    }
}
