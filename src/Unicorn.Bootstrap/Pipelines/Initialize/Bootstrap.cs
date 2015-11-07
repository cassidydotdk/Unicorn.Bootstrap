using System;
using Unicorn.Configuration;
using Unicorn.ControlPanel;
using Unicorn.Logging;
using Unicorn.Predicates;

namespace Unicorn.Bootstrap.Pipelines.Initialize
{
    public class Bootstrap
    {
        public void Process(object args)
        {
            var configNames = GetConfigNames();

            var bootstrapConfigurations = GetBootstrapConfigurations(configNames);

            SyncBootstrapConfigurations(bootstrapConfigurations);
        }

        public virtual void SyncBootstrapConfigurations(IConfiguration[] bootstrapConfigurations)
        {
            foreach (var configuration in bootstrapConfigurations)
            {
                var logger = configuration.Resolve<ILogger>();
                var helper = configuration.Resolve<SerializationHelper>();

                try
                {
                    logger.Info(string.Empty);
                    logger.Info("Unicorn.Bootstrap is syncing " + configuration.Name);

                    // Need to check with @kamsar on this. Leaving this code in, forces a reference to sitecore.kernel
                    //using (new TransparentSyncDisabler())
                    //{
                    var pathResolver = configuration.Resolve<PredicateRootPathResolver>();

                    var roots = pathResolver.GetRootSerializedItems();

                    helper.SyncTree(configuration, null, roots);
                    //}
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    break;
                }
            }
        }

        public virtual IConfiguration[] GetBootstrapConfigurations(string[] configNames)
        {
            return ControlPanelUtility.ResolveConfigurationsFromQueryParameter(string.Join("^", configNames));
        }

        public virtual string[] GetConfigNames()
        {
            throw new NotImplementedException();
        }
    }
}