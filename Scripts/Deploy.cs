#!/usr/bin/env dotnet run

#:include Shared/*.cs

const string ProjectName = "Cyberia";
const string ProjectPath = $"../{ProjectName}/{ProjectName}.csproj";
const string RemoteUser = "salamandra";
const string RemoteHost = "amphibian.fr";
const int RemotePort = 9;

const string OutputPath = $"../publish/{ProjectName}";
const string RemotePath = $"/var/www/{ProjectName.ToLowerInvariant()}/App";

Command.Execute("dotnet", $"publish \"{ProjectPath}\" -c Release -r linux-x64 -o \"{OutputPath}\" -p:PublishReadyToRun=true -p:DebugType=Embedded");
Command.Execute("scp", $"-r -P {RemotePort} {OutputPath}/* {RemoteUser}@{RemoteHost}:{RemotePath}");
Command.Execute("ssh", $"{RemoteUser}@{RemoteHost} -p {RemotePort} \"chmod -R a+x {RemotePath}/flare\"");
