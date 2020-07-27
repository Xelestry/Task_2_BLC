using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using Translation = Task_2_BLC.Translations.Translation;
using Task_2_BLC.Interface;
using Task_2_BLC.Models;

namespace Task_2_BLC
{
    public class FileProcess : IProcessFIle<FileModel>
    {
        private readonly string _defaultFolder;
        private readonly ILogger _logger;
        private readonly List<RuleModel> _rules;

        public FileProcess(IEnumerable<RuleModel> rules, string defaultFolder, ILogger logger)
        {
            _rules = rules.ToList();
            _logger = logger;
            _defaultFolder = defaultFolder;
        }

        public void MoveFileProcess(FileModel item)
        {
            string from = item.FullPath;

            foreach (var rule in _rules)
            {
                var match = Regex.Match(item.Name, rule.FilePattern);

                if (match.Success && match.Length == item.Name.Length)
                {
                    rule.MatchesCount++;

                    string to = CreatePath(item, rule);
                    _logger.Log(Translation.RuleFounded);

                    MoveFile(from, to);
                    _logger.Log(string.Format(Translation.FileMovedTo, item.FullPath, to));

                    return;
                }
            }

            string destination = Path.Combine(_defaultFolder, item.Name);
            _logger.Log(Translation.RuleNotFound);
            MoveFile(from, destination);
            _logger.Log(string.Format(Translation.FileMovedTo, item.FullPath, destination));
        }

        private void MoveFile(string from, string to)
        {
            string dir = Path.GetDirectoryName(to);
            bool cannotAccessFile = true;
            Directory.CreateDirectory(dir);

            while (cannotAccessFile)
            {
                try
                {
                    if (File.Exists(to))
                    {
                        File.Delete(to);
                    }

                    File.Move(from, to);
                    cannotAccessFile = false;
                }
                catch (FileNotFoundException)
                {
                    _logger.Log(Translation.FileNotFound);
                    break;
                }
            }
        }

        private string CreatePath(FileModel file, RuleModel rule)
        {
            string extension = Path.GetExtension(file.Name);
            string filename = Path.GetFileNameWithoutExtension(file.Name);
            StringBuilder destination = new StringBuilder();

            destination.Append(Path.Combine(rule.DestinationFolder, filename));

            AppendDateName(rule, destination);
            AppendOrderName(rule, destination);
            destination.Append(extension);

            return destination.ToString();
        }

        private void AppendDateName(RuleModel rule, StringBuilder destination)
        {
            if (rule.IsDateAppended)
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
                dateTimeFormat.DateSeparator = ".";
                destination.Append($"_{DateTime.Now.ToLocalTime().ToString(dateTimeFormat.ShortDatePattern)}");
            }
        }

        private void AppendOrderName(RuleModel rule, StringBuilder destination)
        {
            if (rule.IsOrderAppended)
            {
                destination.Append($"_order-{rule.MatchesCount}");
            }
        }
    }
}
