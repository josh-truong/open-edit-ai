using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEditAI.Code
{
    public class OpenAIUtility
    {
        public string GetTranscription(string filepath)
        {
            // This is temporary to ensure I don't run the api call
            throw new NotImplementedException();

            string scriptPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\transcribe.py";

            // Specify the path to the Python executable in your venv
            string pythonPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\myenv\Scripts\python.exe";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath; // Use the Python executable from the venv
            start.Arguments = string.Format("{0} {1}", scriptPath, filepath);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;

            string output = string.Empty;
            using (Process process = Process.Start(start))
            {
                // Read the output stream first and then wait.
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                output = result;
            }
            if (!File.Exists(output)) { throw new Exception($"Output file: {output} does not exists."); }
            return output;
        }

        public List<int> GetExtractedSubtitles(string filepath)
        {
            // This is temporary to ensure I don't run the api call
            throw new NotImplementedException();

            string scriptPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\subtitle_extractor.py";

            // Specify the path to the Python executable in your venv
            string pythonPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\myenv\Scripts\python.exe";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath; // Use the Python executable from the venv
            start.Arguments = string.Format("{0} {1}", scriptPath, filepath);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;

            string output = string.Empty;
            using (Process process = Process.Start(start))
            {
                // Read the output stream first and then wait.
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                output = result;
            }
            return ExtractIntegersFromString(output);
        }

        private List<int> ExtractIntegersFromString(string raw)
        {
            return raw.Trim('[', ']').Split(',').Select(s => int.Parse(s.Trim().Trim('\''))).ToList();
        }
    }
}
