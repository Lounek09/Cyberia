# Cyberia

**Salamandra** a Discord Bot made with [DSharpPlus](https://dsharpplus.github.io/DSharpPlus/) and **Amphibian** a Website made with [ASP.NET Core](https://learn.microsoft.com/fr-fr/aspnet/core/) and [htmx](https://htmx.org/) for the MMORPG [Dofus Retro](https://www.dofus-retro.com/).

[![Build](https://github.com/Lounek09/Cyberia/actions/workflows/build-validation.yml/badge.svg)](https://github.com/Lounek09/Cyberia/actions/workflows/build-validation.yml)

![Logo salamandra](images/logo-salamandra.png) **Salamandra** : <https://discord.com/application-directory/687745374294638594>  
![Logo amphibian](images/logo-amphibian.png) **Amphibian** : <https://amphibian.fr>  
![Logo discord](images/logo-discord.png) **Discord** : <https://discord.gg/gfsPNqGXdD>  

## Setup

> [!NOTE]
> This procedure is tailored to my specific use case. Feel free to setup the application in any way that suits you.

### Prerequisites

Ensure you have the following prerequisites installed:
- **.NET**: [Install .NET](https://learn.microsoft.com/en-us/dotnet/core/install/linux)
- **Cyberia.Cdn**: Follow the [Cyberia.Cdn setup](https://github.com/Lounek09/Cyberia.Cdn#setup)
- **Caddy**: [Install Caddy](https://caddyserver.com/docs/install)

### Installation Steps

> [!IMPORTANT]  
> From this point forward, I will assume that all steps from the Cyberia.Cdn setup have been completed.

1. **Build the project:**
   You have two options for building the project: manually from the server or by setting up the publish action in your fork.
   - **Manual Build:**
     ```bash
     git clone git@github.com:Lounek09/Cyberia.git
     cd Cyberia
     dotnet build Cyberia/Cyberia.csproj -c Release -o output
     mv output /var/www/cyberia/App
     ```
   - Setup the publish action in your fork:
     You will need to configure four secrets in your GitHub repository settings:
     - **SSH_HOST** - The ip of your server
     - **SSH_KEY** - Your private SSH key configured for your user
     - **SSH_PORT** - The port of your SSH
     - **SSH_USER** - The user it will connect to  
     After configuring these secrets, you can manually trigger the publish action by following [GitHub's guide on manually running a workflow](https://docs.github.com/en/actions/using-workflows/manually-running-a-workflow).

2. **Configure Caddy:**
   Edit the Caddy configuration file `/etc/caddy/Caddyfile` to include the following block. Replace *your-domain.com* with your actual domain:
   ```caddy
   your-domain.com {
     import common
   
     reverse_proxy :5009
     encode gzip
   }
   ```

3. **Restart Caddy:**  
   Restart the Caddy service to apply the new configuration[^1]:
   ```bash
   sudo systemctl restart caddy
   ```

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

For any inquiries or further information, please contact <a href="mailto:lounek09@proton.me">lounek09@proton.me</a>.

[^1]: If the service fails to start due to missing permissions, it's likely because Caddy creates certain directories without the correct permissions. Try the following command after each restart until it works: `sudo chmod -R 755 /var/lib/caddy/.local`
