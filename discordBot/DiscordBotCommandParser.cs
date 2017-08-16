using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace discordBot
{
    class DiscordBotCommandPaserMatchResult
    {
        public bool Success { get; set; }
        public Dictionary<string,string> Matches { get; set; }
    }
    class DiscordBotCommandParser
    {
        public string[] Result => _result;
        public bool Success => _success;

        private readonly string[] _result;
        private readonly string _prefix;
        private readonly bool _success;

        public DiscordBotCommandParser(string prefix, string line)
        {
            _prefix = prefix;

            if (line.StartsWith(_prefix))
            {
                _success = true;
                line = line.Remove(0, _prefix.Length).TrimStart();

                if (!string.IsNullOrEmpty(line))
                {
                    _result = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            else
            {
                _success = false;
            }
        }

        public DiscordBotCommandPaserMatchResult Match(params string[] parameters)
        {
            Dictionary<string, string> matches = new Dictionary<string, string>();
            bool success = false;
            var resultList = _result.ToList();
            parameters.ToList().ForEach((A) => matches.Add(A, string.Empty));

            if (resultList.Count % 2 == 0)
            {
                foreach (var parameter in parameters)
                {
                    var index = resultList.IndexOf(parameter);
                    if (index != -1 && index+1 != resultList.Count)
                    {
                        matches[parameter] = resultList[index+1];
                        success = true;
                    }
                }
            }

            return new DiscordBotCommandPaserMatchResult() { Success = success, Matches = matches };
        }
    }
}
