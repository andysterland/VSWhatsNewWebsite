using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsNewWebsite.GenerateContent
{ 
    static class FFmpeg
    {
        // Download ffmpeg from https://ffmpeg.org/download.html
        const string PathToFFmpeg = @"c:\tools\ffmpg\ffmpeg.exe";

        public static void ConvertToGif(string InputVideoPath, string OutputGifPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = PathToFFmpeg,
                // the -n will not overwrite the file if it already exists change to -y to overwrite
                Arguments = $"-n -i {InputVideoPath} {OutputGifPath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.WaitForExit();
        }
    }
}
