using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task_2_BLC.Interface;
using Task_2_BLC.EventArgs;
using Task_2_BLC.Models;
using Translation = Task_2_BLC.Translations.Translation;

namespace Task_2_BLC
{
    public class FilesWatcher : ILocationsWatcher<FileModel>
    {
        private readonly ILogger _logger;
        public event EventHandler<CreatedEventArgs<FileModel>> Created;

        public FilesWatcher(IEnumerable<string> directories, ILogger logger)
        {
            _logger = logger;
            var _fileSystemWatchers = directories.Select(CreateWatcher).ToList();
        }

        private void OnCreated(FileModel file)
        {
            Created?.Invoke(this, new CreatedEventArgs<FileModel> { CreatedItem = file });
        }

        private FileSystemWatcher CreateWatcher(string path)
        {
            FileSystemWatcher fileSystemWatcher =
                new FileSystemWatcher(path)
                {
                    NotifyFilter = NotifyFilters.FileName,
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true
                };

            fileSystemWatcher.Created += (sender, fileSystemEventArgs) =>
            {
                _logger.Log(
                    string.Format(Translation.FileFounded, fileSystemEventArgs.Name));

                OnCreated(
                    new FileModel
                    {
                        FullPath = fileSystemEventArgs.FullPath,
                        Name = fileSystemEventArgs.Name
                    });
            };

            return fileSystemWatcher;
        }
    }
}
