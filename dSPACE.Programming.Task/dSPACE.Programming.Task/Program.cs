using System;

namespace dSPACE.Programming.Task
{
    internal class Program
    {
        private static SignalDecoder signalDecoder = null;
        static void Main(string[] args)
        {
            Console.WriteLine("dSPACE Programming Task Solution:");

            signalDecoder = new SignalDecoder(@"Resources/InputFile.txt");
        }
    }
}
