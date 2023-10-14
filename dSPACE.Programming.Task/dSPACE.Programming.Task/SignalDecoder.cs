using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace dSPACE.Programming.Task
{
    public class SignalDecoder : IDisposable
    {
        private FileStream stream;
        private StreamReader reader;        

        public SignalDecoder()
        {
            stream = null;
            reader = null;
        }

        public SignalDecoder(string path) : this()
        {
            Open(path);
        }

        ~SignalDecoder()
        {
            Close();
        }

        public void Open(string path)
        {
            // Make sure we are in a valid state
            Close();

            /*
             * Copy this Resource folder in dSPACE.Programming.Task\bin\Debug\netcoreapp3.1 folder
             * or copy fullpath from Resources folder and set path value.
             * For example: 
             *  path = E:\dspace_prog_test\dSPACE.Programming.Task\dSPACE.Programming.Task\Resources\InputFile.txt;
             */


            // Open a stream
            stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            reader = new StreamReader(stream, Encoding.UTF8);

            string line = String.Empty;            

            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
                       
        }        

        public void Close()
        {
            stream?.Close();
            stream = null;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
