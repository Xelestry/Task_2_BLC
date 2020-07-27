using System;
using System.Threading;
using System.Threading.Tasks;
using Task_2_BLC.Models;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using Task_2_BLC.EventArgs;
using Task_2_BLC.Translations;
using PathElement = Task_2_BLC.Configuration.PathElement;
using FileConfigurationSection = Task_2_BLC.Configuration.FileConfigurationSection;
using RuleElement = Task_2_BLC.Configuration.RuleElement;
using Task_2_BLC.Interface;
using Task_2_BLC.Logs;

namespace Task_2_BLC
{
    public class Program
    {
        private static List<string> _directories;
        private static List<RuleModel> _rules;
        private static IProcessFIle<FileModel> _fileHandler;
        private const string FileSection = "fileSection";

        static async Task Main(string[] args)
        {
            var config = ConfigurationManager.GetSection(FileSection) as FileConfigurationSection;
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ILogger logger = new Logger();

            ProcessConfig(config);

            Console.WriteLine(config.Culture.DisplayName);

            _fileHandler = new FileProcess(_rules, config.Rules.DefaultDirectory, logger);
            ILocationsWatcher<FileModel> watcher = new FilesWatcher(_directories, logger);

            watcher.Created += OnCreated;

            await FreezeConsole(tokenSource.Token);
        }

        private static void ProcessConfig(FileConfigurationSection config)
        {
            if (config != null)
            {
                ReadConfig(config);
            }
            else
            {
                Console.Write(Translation.ConfigNotFound);
                Console.ReadKey();
                return;
            }
        }

        private static void OnCreated(object sender, CreatedEventArgs<FileModel> args)
        {
            _fileHandler.MoveFileProcess(args.CreatedItem);
        }

        private static void ReadConfig(FileConfigurationSection config)
        {
            _directories = new List<string>(config.Directories.Count);
            _rules = new List<RuleModel>();

            foreach (PathElement directory in config.Directories)
            {
                _directories.Add(directory.Path);
            }

            foreach (RuleElement rule in config.Rules)
            {
                _rules.Add(new RuleModel
                {
                    FilePattern = rule.FilePattern,
                    DestinationFolder = rule.DestinationFolder,
                    IsDateAppended = rule.IsDateAppended,
                    IsOrderAppended = rule.IsOrderAppended
                });
            }

            CultureInfo.DefaultThreadCurrentCulture = config.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = config.Culture;
            CultureInfo.CurrentUICulture = config.Culture;
            CultureInfo.CurrentCulture = config.Culture;
        }

        private static Task FreezeConsole(CancellationToken cancellationToken)
        {
            return Task.Delay(TimeSpan.FromMilliseconds(-1), cancellationToken);
        }
    }
}
