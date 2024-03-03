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
        private MainViewModel _viewModel;
        public OpenAIUtility(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public string GetTranscription(string filepath)
        {
            string scriptPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\transcribe.py";

            // Specify the path to the Python executable in your venv
            string pythonPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\myenv\Scripts\python.exe";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath; // Use the Python executable from the venv
            start.Arguments = string.Format("\"{0}\" \"{1}\"", scriptPath, filepath); // Wrap the paths in quotes
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

        public List<int> GetExtractedSubtitles(string filepath, string prompt)
        {
            string scriptPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\subtitle_extractor.py";

            // Specify the path to the Python executable in your venv
            string pythonPath = @"C:\Users\joshk\OneDrive\Documents\Github\open-edit-ai\OpenEditAI\OpenEditAI\Scripts\myenv\Scripts\python.exe";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath; // Use the Python executable from the venv
            start.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\"", scriptPath, filepath, prompt + " | Format the list of id integers from the JSON file into a C# array.");
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;

            string output = string.Empty;
            using (Process? process = Process.Start(start))
            {
                if (process != null)
                {
                    // Read the output stream first and then wait.
                    string result = process.StandardOutput?.ReadToEnd();
                    process.WaitForExit();

                    output = result ?? string.Empty;
                }
            }

            var ids = ExtractIntegersFromString(output);
            _viewModel.Log = $"Extracted IDs: \n\t{string.Join(", ", ids)}";
            return ids;
        }

        private List<int> ExtractIntegersFromString(string raw)
        {

            return raw.Trim('[', ']').Split(',').Select(s => int.Parse(s.Trim().Trim('\''))).OrderBy(i => i).ToList();
        }
    }
}
