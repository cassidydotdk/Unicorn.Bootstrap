# Unicorn.Bootstrap
Utilising [kamsar\Unicorn](https://github.com/kamsar/Unicorn) to auto-install [Sitecore](http://www.sitecore.net) items automatically when including a [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) enabled NuGet package to your solution.

### For module installers:
1. Add [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) enabled module to your solution with NuGet Package Manager.
2. Start Sitecore
3. Done.
### For module creators:
1. Add a reference to [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) via NuGet Package Manager.
2. Create a Unicorn configuration for your module and place it in `\App_Config\Include\Unicorn.Bootstrap\YourModuleName.config`. Configure the repository folder as `\App_Config\Include\Unicorn.Bootstrap\YourModuleName`. (see included example).
3. Work on your module as normal; [Unicorn](https://github.com/kamsar/Unicorn) will serialise in the background as usual.
4. When packaging your module; keep the [Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) dependency and make sure to add your .config and contents of the serialisation folder to your NuGet package.
5. Publish the module as `YourModuleName.Bootstrap` 
6. Done.

## TL;DR ##
[Unicorn.Bootstrap](https://github.com/cassidydotdk/Unicorn.Bootstrap) will install itself as a processor in your `<initialize>` [Sitecore](http://www.sitecore.net) pipeline, and force a sync of all configurations found in `\App_Config\Include\Unicorn.Bootstrap\`.

### Further information
To be fair, knowledge of how Unicorn works is probably required at this stage. I just don't have time to write a long documentation with examples and graphs and fireworks right now.
