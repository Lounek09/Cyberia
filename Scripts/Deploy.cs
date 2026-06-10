#!/usr/bin/env dotnet run

#:include Shared/*.cs

const string ProjectPath = "../Cyberia/Cyberia.csproj";
const string OutputPath = "../publish/Cyberia";

const string RemoteUser = "salamandra";
const string RemoteHost = "amphibian.fr";
const int RemotePort = 9;
const string RemotePath = "/var/www/cyberia/App";

Command.Execute("dotnet", $"publish {ProjectPath} -c Release -r linux-x64 -o {OutputPath} -p:PublishReadyToRun=true -p:DebugType=Embedded");
Command.Execute("ssh", $"{RemoteUser}@{RemoteHost} -p {RemotePort} \"sudo systemctl stop cyberia\"");
Command.Execute("scp", $"-r -P {RemotePort} {OutputPath}/* {RemoteUser}@{RemoteHost}:{RemotePath}");
Command.Execute("ssh", $"{RemoteUser}@{RemoteHost} -p {RemotePort} \"chmod -R a+x {RemotePath}/Cyberia && chmod -R a+x {RemotePath}/flare\"");
Command.Execute("ssh", $"{RemoteUser}@{RemoteHost} -p {RemotePort} \"sudo systemctl start cyberia\"");
