using System;
using System.IO;
using System.Text;

namespace SimpleTail
{
    class SimpleTail
    {
        private const int READ_CHUNK_SIZE = 1024; // Bytes
        private const int LINE_COUNT = 10;
        private FileArguments[] fileList;

        public SimpleTail(FileArguments[] fileList)
        {
            this.fileList = fileList;

            foreach (FileArguments fileArgs in fileList)
            {
                // Verify that the file exists, abort early if it doesn't
                if (!File.Exists(fileArgs.path))
                {
                    Console.WriteLine("No file could be found at: " + fileArgs.path);
                    continue;
                }

                // TODO: Print a header with file path if not quiet mode

                LinkedCharBuffer lines = ReadLastLines(LINE_COUNT, fileArgs.path);

                foreach (char c in lines)
                {
                    Console.Write(c);
                }

                // TODO: Start follow threads for each fileArg if follow flag is set
            }
        }

        // Read lines at end of file till line limit has been reached
        private LinkedCharBuffer ReadLastLines(int lineLimit, string path)
        {
            var lines = new LinkedCharBuffer();
            int lineCount = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                char[] buffer;
                sr.BaseStream.Seek(-READ_CHUNK_SIZE, SeekOrigin.End);
                
                // Read byte chunks till we've reached line limit or top of file
                while (sr.BaseStream.Position > 0)
                {
                    // Read chunk (default 1024 bytes) of file into memory
                    buffer = new char[READ_CHUNK_SIZE];
                    sr.Read(buffer, 0, READ_CHUNK_SIZE);

                    // Start looping 1 char at a time, increase line counter upon newline operators
                    for (int i = READ_CHUNK_SIZE - 1; i >= 0; i--)
                    {
                        // If '\n' is found look at one more char to see if it is a newline
                        if (buffer[i] == '\n' && buffer[i - 1] == '\r') // TODO: Figure nice and efficient way to support UNIX style line endings as well (No, we're not casting every char to string and comparing with Environment.Newline)
                        {
                            lineCount++;

                            // Check if we have reached line limit, return if yes
                            if (lineCount == lineLimit)
                            {
                                int restLength = (READ_CHUNK_SIZE - 1) - i;
                                char[] rest = new char[restLength];
                                Array.Copy(buffer, i, rest, 0, restLength);
                                lines.addBuffer(rest);
                                return lines;
                            }
                        }
                    }

                    lines.addBuffer(buffer);
                    sr.BaseStream.Seek(-(READ_CHUNK_SIZE * 2), SeekOrigin.Current);
                }
            }

            return lines;
        }
    }
}
