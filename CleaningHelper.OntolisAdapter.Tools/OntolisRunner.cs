using System;
using System.Diagnostics;
using System.IO;

namespace CleaningHelper.OntolisAdapter.Tools
{
    public class OntolisRunner
    {
        private readonly string _pathToOntolis;
        private readonly bool _block;
        private Process _process = null;

        public OntolisRunner(string pathToOntolis, bool block = false)
        {
            _pathToOntolis = pathToOntolis;
            _block = block;
        }

        public void RunOntolis()
        {
            // var startInfo = new ProcessStartInfo();
            // startInfo.WorkingDirectory = Path.GetDirectoryName(_pathToOntolis);
            // startInfo.FileName = _pathToOntolis;
                
            _process = Process.Start(_pathToOntolis);
            if (_block)
            {
                _process.WaitForExit();
                Console.WriteLine("Ontolis finished");
            }
        }

        public void StopOntolis()
        {
            _process?.Kill();
        }
    }
}