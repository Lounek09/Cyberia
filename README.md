# Cyberia

**Salamandra** a Discord Bot made with [DSharpPlus](https://dsharpplus.github.io/DSharpPlus/) and **Amphibian** a Website made with [ASP.NET Core](https://learn.microsoft.com/fr-fr/aspnet/core/) and [htmx](https://htmx.org/) for the MMORPG [Dofus Retro](https://www.dofus-retro.com/).

[![Build](https://github.com/Lounek09/Cyberia/actions/workflows/build-validation.yml/badge.svg)](https://github.com/Lounek09/Cyberia/actions/workflows/build-validation.yml)

![Logo salamandra](images/logo-salamandra.png) **Salamandra** : <https://discord.com/application-directory/687745374294638594>  
![Logo amphibian](images/logo-amphibian.png) **Amphibian** : <https://amphibian.fr>  
![Logo discord](images/logo-discord.png) **Discord** : <https://discord.gg/gfsPNqGXdD>  

## Setup

> [!NOTE]
> This procedure is tailored to my specific use case. Feel free to setup the application in any way that suits you.

### Installation Steps

#### Development:
//TODO

#### Production:
1. **Prerequisites:**  
   Ensure you have the following prerequisites:
   - An x64 or x86 processor architecture, [Flare](http://www.nowrap.de/flare.html) does not support ARM.
   - **.NET**: [Install .NET](https://learn.microsoft.com/en-us/dotnet/core/install/linux)
   - **Cyberia.Cdn**: Follow the [Cyberia.Cdn setup](https://github.com/Lounek09/Cyberia.Cdn#setup)
   - **Caddy**: [Install Caddy](https://caddyserver.com/docs/install)

> [!IMPORTANT]
> From this point forward, I will assume that all steps from the Cyberia.Cdn setup have been completed.

2. **Build the project:**  
   You have two options for building the project: manually from the server or by setting up the publish action in your fork.
   
   - **Manual Build:**  
     ```bash
     git clone git@github.com:Lounek09/Cyberia.git
     cd Cyberia
     dotnet build Cyberia/Cyberia.csproj -c Release -o output
     mv output /var/www/cyberia/App
     chmod -R 755 /var/www/cyberia/App/flare
     ```
     
   - **Setup the publish action in your fork:**  
     You will need to configure four secrets in your GitHub repository settings.
     - **SSH_HOST** - The IP of your server
     - **SSH_KEY** - Your private SSH key configured in the user `authorized_keys`
     - **SSH_PORT** - The port of your SSH
     - **SSH_USER** - The user it will connect to
     
     After configuring these secrets, you can manually trigger the publish action by following [GitHub's guide on manually running a workflow](https://docs.github.com/en/actions/using-workflows/manually-running-a-workflow).

3. **Configure Caddy:**  
   Edit the Caddy configuration file located at `/etc/caddy/Caddyfile` to include the following block. Replace *your-domain.com* with your actual domain:
   ```caddy
   your-domain.com {
     import common
   
     reverse_proxy :5009
     encode gzip
   }
   ```
   For more information, see the [Caddiyfile documentation](https://caddyserver.com/docs/caddyfile).  

4. **Restart Caddy:**  
   Restart the Caddy service to apply the new configuration[^1]:
   ```bash
   sudo systemctl restart caddy
   ```

5. **Create a systemd service:**  
   To launch the app in the background, a systemd service is a good approach. Go to `/etc/systemd/system` and create a new file called `cyberia.service` with this content. Replace *salamandra* with your user:
   ```service
   [Unit]
   Description=Cyberia Service
   After=network.target
   
   [Service]
   Type=simple
   User=salamandra
   ExecStart=/var/www/cyberia/App/Cyberia
   ExecStop=/bin/bash -c 'ps aux | grep /var/www/cyberia/App/Cyberia | grep -v grep | awk \'{print $2}\' | xargs kill -9'
   Restart=on-failure
   RestartSec=10

   [Install]
   WantedBy=multi-user.target
   ```
   For more information, see the [systemd.service documentation](https://www.freedesktop.org/software/systemd/man/latest/systemd.service.html).  

6. **Reload the systemd manager configuration:**  
   Reload the systemd manager configuration to apply the new service:
   ```bash
   sudo systemctl daemon-reload
   ```

7. **Configure the app:**  
   Inside the App directory, you will need to rename the `appsettings.sample.json` to `appsettings.json` and fill in the required configuration. See [Configuration](#configuration) for more detail about each variable.

8. **Launch the App:**  
   Start the service:
   ```bash
   sudo systemctl start cyberia
   ```

9. **First-time Launch:**  
   If this is your first time launching the application, you will need to generate the JSON data used by the API. You can either wait for the automatic Lang check to occur or manually trigger it using the `/langs check` command in Discord. After the check is complete, use the `/langs parse` command to parse the data to JSON. Then, restart the application or use the `/reload` command to hot reload the data. Finally, use the command `/emoji upload` to upload the emojis to your Discord application.

## Configuration

Below are the detailed descriptions of each variable of the [configuration](/Cyberia/appsettings.sample.json) of the App :

### Main Configuration

| Variable                    | Description                                       | Type     |
| :-------------------------- | :------------------------------------------------ | :------- |
| `EnableSalamandra`          | Launch the Discord bot at startup                 | Boolean  |
| `EnableAmphibian`           | Launch the website at startup                     | Boolean  |
| `EnableCheckCytrus`         | Activate the automatic check of Cytrus            | Boolean  |
| `CheckCytrusInterval`       | Interval between each Cytrus check                | Timespan |
| `EnableCheckLang`           | Activate the automatic check of the Official Lang | Boolean  |
| `CheckLangInterval`         | Interval between each Official Lang check         | Timespan |
| `EnableCheckBetaLang`       | Activate the automatic check of the Beta Lang     | Boolean  |
| `CheckBetaLangInterval`     | Interval between each Beta Lang check             | Timespan |
| `EnableCheckTemporisLang`   | Activate the automatic check of the Temporis Lang | Boolean  |
| `CheckTemporisLangInterval` | Interval between each Temporis Lang check         | Timespan |
| `ApiConfig`                 | The configuration related to the API              | [ApiConfig](#api-configuration) |
| `BotConfig`                 | The configuration related to the bot              | [BotConfig](#bot-configuration) |
| `WebConfig`                 | The configuration related to the website          | [WebConfig](#web-configuration) |

### API Configuration

| Variable            | Description                                                                                                    | Type   |
| :------------------- | :------------------------------------------------------------------------------------------------------------ | :----- |
| `CdnUrl`             | The URL of the [CDN](https://github.com/Lounek09/Cyberia.Cdn)                                                 | String |
| `Type`               | The type of lang loaded at startup                                                                            | [LangType](/Cyberia.Langzilla.Enums/LangType.cs) |
| `BaseLanguage`       | The language of lang from which the base data is loaded, only the translations will be loaded from the others | [Language](/Cyberia.Langzilla.Enums/Language.cs) |
| `SupportedLanguages` | The list of the supported languages, the first one will be the default language.                              | [Language](/Cyberia.Langzilla.Enums/Language.cs)[] |
| `DiscordInviteUrl`   | The invitation URL of the support Discord guild                                                               | String |
| `GitRepositoryUrl`   | The URL of the repository                                                                                     | String |

### Bot Configuration

| Variable                  | Description                                                                        | Type   |
| :------------------------ | :--------------------------------------------------------------------------------- | :----- |
| `Token`                   | The Discord bot token                                                              | String |
| `EmbedColor`              | The color of the embed (e.g. `#CD853F`)                                            | String |
| `AdminGuildId`            | The guild ID where the admin commands will be registered                           | UInt64 |
| `BotInviteUrl`            | The invitation URL of the bot                                                      | String |
| `LogChannelId`            | The channel where logs from certain events (e.g. guild added/removed) will be sent | UInt64 |
| `ErrorChannelId`          | The channel ID where errors related to command execution will be sent              | UInt64 |
| `LangForumChannelId`      | The forum channel ID where the automatic lang diff will be sent                    | UInt64 |
| `CytrusChannelId`         | The channel ID where the Cytrus diff will be sent                                  | UInt64 |
| `CytrusManifestChannelId` | The channel ID where the game manifest diff from Cytrus will be sent               | UInt64 |

### Web Configuration

| Variable     | Description                                                      | Type     |
| :----------- | :--------------------------------------------------------------- | :------- |
| `Environment`| The environment of the website (`Production` or `Development`)   | String   |
| `Urls`       | The URLs the host will listen on                                 | String[] |

## Support
If you encounter any issues or have questions, please:
- Open an issue on GitHub.
- Join my [Discord server](https://discord.gg/gfsPNqGXdD) for real-time support.

## License

Copyright (C) 2020-2024 Lounek

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.

The full text of the license can be found in the [LICENSE](LICENSE) file.

--

This project uses [Flare](http://www.nowrap.de/flare.html) for SWF decompilation. Please note that Flare is governed by its own licensing terms, which are separate and distinct from the licensing terms of this project under the AGPL-3.0. For the specific terms and conditions of Flare’s license, please refer to the [LICENSE](Cyberia.Langzilla/flare/LICENSE.TXT) file included in this distribution or visit the [official website](http://www.nowrap.de/flare.html) of Flare.

**Important:** Flare is provided for use under specific terms that do not allow commercial use without prior permission. Users are responsible for adhering to these terms and should not assume that the commercial permissions granted under the AGPL-3.0 extend to the use of Flare.

--

Parts of this project incorporate code derived from the [Lucene.Net](https://github.com/apache/lucenenet) project. Such code is provided under the Apache License, Version 2.0. You can find a copy of this license at [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0). The files containing these materials are identified in the source code along with the original licensing information from the Lucene.Net project.

**Important:** This licensing does not affect the usability of this software under the AGPL-3.0 for general purposes, but users must comply with the terms of the Apache License, Version 2.0, where applicable.

--

For any inquiries or further information, please contact [lounek09@proton.me](mailto:lounek09@proton.me).

[^1]: If the service fails to start due to missing permissions, it's likely because Caddy creates certain directories without the correct permissions. Try the following command after each restart until it works: `sudo chmod -R 755 /var/lib/caddy/.local`
