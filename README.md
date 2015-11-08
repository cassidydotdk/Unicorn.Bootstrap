# Unicorn.Bootstrap
Utilising [kamsar\Unicorn](https://github.com/kamsar/Unicorn) to auto-install [Sitecore](http://www.sitecore.net) items automatically when including a [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) enabled NuGet package to your solution.

- Mark Cassidy, November 2015. [@cassidydotdk](https://twitter.com/cassidydotdk), [https://github.com/cassidydotdk](https://github.com/cassidydotdk)

### For module installers:
1. Add [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) ***enabled module (NuGet Package)*** to your solution with NuGet Package Manager. You don't need to add a reference to [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) directly, unless you are creating a Sitecore module.
2. Start Sitecore
3. Done.

### For module creators:
1. Add a reference to [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) via NuGet Package Manager.
2. Create a Unicorn configuration for your module and place it in `\App_Config\Include\Unicorn.Bootstrap.YourModuleName.config`. Configure the repository folder as `$(dataFolder)\Unicorn.Bootstrap\$(configurationName)`. (see example).
3. Work on your module as normal; [Unicorn](https://github.com/kamsar/Unicorn) will serialise in the background as usual.
4. When packaging your module; keep the [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) dependency and make sure to add your .config and contents of the serialisation folder to your NuGet package.
	1. Unicorn.Bootstrap.YourModule.config => content\App_Config\Include
	2. Unciorn Target DataStore root folder => content\Unicorn.Bootstrap
	3. Unicorn.Bootstrap will move folders under ~\Unicorn.Bootstrap to their matching Target Data Store folders during startup, and perform a sync operation.
5. Publish the module as `YourModuleName.Bootstrap`
6. Done.

### Example Configuration
	<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
	  <sitecore>
		<unicorn>
		  <configurations>
			<configuration name="Example Module" description="Unicorn.Bootstrap Example Module configuration">
			  <predicate type="Unicorn.Predicates.SerializationPresetPredicate, Unicorn" singleInstance="true">
	          	<include database="master" path="/sitecore/system/modules/Example Module" />
			  </predicate>
	
			  <targetDataStore type="Rainbow.Storage.SerializationFileSystemDataStore, Rainbow" physicalRootPath="$(dataFolder)\Unicorn.Bootstrap\$(configurationName)" useDataCache="false" singleInstance="true"/>
	   		</configuration>
		  </configurations>
		</unicorn>
	  </sitecore>
	</configuration>

### Caveats
Make sure to keep your Target Data Store root folder name the same as your configuration name a all times. [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) relies on this, to figure out which configurations to sync at startup. Absolutely make sure your Data Store root folder is unique, but Unicorn should also throw an exception if this is not the case. Place this *alongside* your regular Unicorn storage path, not *inside* it.

## TL;DR ##
[Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) will install itself as a processor in your `<initialize>` [Sitecore](http://www.sitecore.net) pipeline, and force a sync of all configurations found in `\Unicorn.Bootstrap\`.

### Further information
To be fair, knowledge of how Unicorn works is probably required at this stage. I just don't have time to write a long documentation with examples and graphs and fireworks right now.

# DISCLAIMER
Unicorn.Bootstrap is currently under **evaluation** and has been released to public for comments. Absolutely use at your own risk. Also see license.
