using System;
using System.IO;
using System.Linq;
using Unicorn.Configuration;
using Unicorn.ControlPanel;
using Unicorn.Data;
using Unicorn.Logging;
using Unicorn.Predicates;

namespace Unicorn.Bootstrap.Pipelines.Initialize
{
    public class Bootstrap
    {
        public void Process(object args)
        {
            var bootstrapDirectories = GetBootstrapDirectories();

            foreach (var directory in bootstrapDirectories)
            {
                ProcessDirectory(directory);
            }
        }

        public virtual void ProcessDirectory(string directory)
        {
            var directoryShortName = new DirectoryInfo(directory).Name;
            var configurations = UnicornConfigurationManager.Configurations;
            var configuration = configurations.First(c => c.Name.Equals(directoryShortName, StringComparison.OrdinalIgnoreCase));

            if (configuration != null)
            {
                var success = MoveBootstrapToTargetDataStore(directory, GetTargetDataStorePathFromIConfiguration(configuration), configuration);
                if (success)
                    SynchroniseTargetDataStore(configuration);
            }
        }

        public virtual void SynchroniseTargetDataStore(IConfiguration configuration)
        {
            var logger = configuration.Resolve<ILogger>();
            var helper = configuration.Resolve<SerializationHelper>();

            try
            {
                logger.Info(string.Empty);
                logger.Info("Unicorn.Bootstrap is syncing " + configuration.Name);

                var pathResolver = configuration.Resolve<PredicateRootPathResolver>();

                var roots = pathResolver.GetRootSerializedItems();

                helper.SyncTree(configuration, null, roots);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        // http://stackoverflow.com/questions/2553008/directory-move-doesnt-work-file-already-exist
        public static void MoveDirectory(string source, string target)
        {
            var sourcePath = source.TrimEnd('\\', ' ');
            var targetPath = target.TrimEnd('\\', ' ');
            var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                .GroupBy(Path.GetDirectoryName);
            foreach (var folder in files)
            {
                var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                Directory.CreateDirectory(targetFolder);
                foreach (var file in folder)
                {
                    if (file != null)
                    {
                        var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                        if (File.Exists(targetFile)) File.Delete(targetFile);
                        File.Move(file, targetFile);
                    }
                }
            }
            Directory.Delete(source, true);
        }

        public virtual bool MoveBootstrapToTargetDataStore(string directory, string targetDataStorePath, IConfiguration configuration)
        {
            var logger = configuration.Resolve<ILogger>();

            if (string.IsNullOrEmpty(directory))
            {
                logger.Error("Source directory not specified.");
                return false;
            }

            if (string.IsNullOrEmpty(targetDataStorePath))
            {
                logger.Error("targetStoreDataPath directory not specified (or resolved).");
                return false;
            }

            logger.Info(string.Empty);
            logger.Info($"Bootstrap folder \"{directory}\" being moved to \"{targetDataStorePath}\"");

            try
            {
                if (Directory.Exists(targetDataStorePath))
                    Directory.Delete(targetDataStorePath, true);
                MoveDirectory(directory, targetDataStorePath);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }

            return true;
        }

        public virtual string GetTargetDataStorePathFromIConfiguration(IConfiguration configuration)
        {
            var targetDataStore = configuration.Resolve<ITargetDataStore>();
            if (targetDataStore == null)
                throw new Exception($"targetDatastore undefined in configuration '{configuration.Name}'");

            return targetDataStore.GetConfigurationDetails().First(kvp => kvp.Key.Equals("Physical root path")).Value;
        }

        public virtual string[] GetBootstrapDirectories()
        {
            var baseDir = $"{AppDomain.CurrentDomain.BaseDirectory}Unicorn.BootStrap";

            if (Directory.Exists(baseDir))
                return Directory.GetDirectories(baseDir);

            return new string[] {};
        }
    }
}