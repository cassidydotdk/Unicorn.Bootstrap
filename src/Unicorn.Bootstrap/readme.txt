Unicorn.Bootstrap / https://github.com/cassidydotdk/Unicorn.Bootstrap

Adding a reference to Unicorn.Bootstrap is only required for Sitecore module creators who wish to create NuGet distributable modules with the ability to auto-install related Sitecore content after NuGet package installation.

To get started, create a unique Unicorn configuration and place it under \App_Config\Include.

Example:

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

- Make sure to give your configuration a unique name for your module, and configure one or more <predicate> entries as your module requires.

- Perform initial Unicorn synchronization. See https://github.com/kamsar/Unicorn for Unicorn documentation. Usually you can call http://mysite/Unicorn.aspx, select your module configuration and "Perform Initial Synchronization".

- Work on your module as normal. Whenever Sitecore content for your module is modified, Unicorn will keep synchronising it to the targetDataStore specified above.

- When ready to package your module, include your .config file in your .nuspec package definition, add your targetDataStore root folder to content->Unicorn.Bootstrap, and add a dependency for Unicorn.Bootstrap

- Package and distribute NuGet package as normal


See https://github.com/cassidydotdk/Unicorn.Bootstrap for more information.

Unicorn.Bootstrap IS PRESENTED TO THE PUBLIC AS EXPERIMENTAL, REQUESTING COMMENTS AND FEEDBACK. NO WARRANTIES. SEE LICENSE.
