using System.Collections.Generic;
using System.Text.RegularExpressions;
using Moofetch.Generic;
using Moofetch.Writing;

namespace Moofetch.Fetch
{
    public static class Main
    {

        private static readonly Dictionary<string, string> info = new Dictionary<string, string>()
        {
            
            {
                "OS: ", ""
            },
            {
                "Kernel: ", ""
            },
            {
                "Uptime: ", ""
            },
            {
                "CPU: ", ""
            },
            {
                "GPU: ", ""
            }
            
        };
        
        public static void run()
        {
            Debug.throwInfo("Getting OS...");
            info["OS: "] = KeyValueParser.deserialise(FileReader.getFileLines("/etc/os-release"), '=')["PRETTY_NAME"];
            
            Debug.throwInfo("Getting Kernel Version...");
            info["Kernel: "] = CommandRunner.runCommand("uname -r");
            
            Debug.throwInfo("Getting Uptime...");
            info["Uptime: "] = CommandRunner.runCommand("uptime -p");
            
            Debug.throwInfo("Getting CPU...");
            info["CPU: "] = KeyValueParser.deserialise(FileReader.getFileLines("/proc/cpuinfo"), ':')["model name\t"];
            
            Debug.throwInfo("Getting GPU...");
            info["GPU: "] = getGPU();

            Printer.printResult(info);
        }
        
        private static string getGPU()
        {
            string lspciOutput = CommandRunner.runCommand("lspci | grep -i --color 'vga\\|3d\\|2d'");
            Regex pattern = new Regex(@"\[(.*?)\]");
            Match match = pattern.Match(lspciOutput);

            return match.ToString().Trim(new char[]{'[', ']'});
        }

        
    }
}