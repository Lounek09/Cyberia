﻿using System.Text;

namespace Cyberia.Langs
{
    public static class Flare
    {
        private static readonly object _lock = new();

        /// <summary>
        /// Extract the ActionScript code of a swf file. A XXX.flr file will be generated where XXX is the name of the given swf.
        /// </summary>
        /// <param name="swfPath">The path of the swf to decompile to</param>
        /// <param name="warningMessage">Warning message returned by flare</param>
        /// <returns>True if the swf is successfully extracted.</returns>
        public static bool ExtractSwf(string swfPath, out string warningMessage)
        {
            lock (_lock)
                return ExecuteCmd.ExecuteCommand(Constant.GetFlareExecutablePath(), swfPath, out warningMessage);
        }

        /// <summary>
        /// Extracts the lang and generates a file named 'current.as' with its contents. If a file already exists, it is moved to 'old.as' before.
        /// </summary>
        /// <returns>True if the lang is successfully extracted.</returns>
        /// <exception cref="Exception"></exception>
        public static bool ExtractLang(Lang lang)
        {
            if (!ExtractSwf(lang.FilePath, out string warningMessage))
            {
                DofusLangs.Instance.Logger.Error($"Error when decompiled '{lang.FilePath}'\nWarning : {warningMessage}");
                return false;
            }

            string flareOutputFilePath = $"{lang.FilePath.TrimEnd(".swf")}.flr";

            List<string> content = new();
            foreach (string line in File.ReadAllLines(flareOutputFilePath, Encoding.UTF8).Skip(7).SkipLast(3))
            {
                string temp = line.Trim();

                if (temp.Length == 0 || temp.Equals("}") || temp.Equals("frame 1 {"))
                    continue;

                content.Add(temp);
            }

            if (File.Exists($"{lang.DirectoryPath}/current.as"))
                File.Move($"{lang.DirectoryPath}/current.as", $"{lang.DirectoryPath}/old.as", true);
            File.WriteAllLines($"{lang.DirectoryPath}/current.as", content, Encoding.UTF8);
            File.Delete(flareOutputFilePath);

            return true;
        }
    }
}