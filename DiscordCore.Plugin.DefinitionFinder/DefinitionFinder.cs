using System;
using DiscordCore.Interfaces;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DiscordCore.Plugin.DefinitionFinder
{
    public class DefinitionFinder : IDiscordCorePlugin
    {
        private string _name = "UrbanDictionarySearcher";
        private string _description = "Plugin that will find definitions for words from Urban Dictionary";
        private IDiscordCoreEvents _discordEventClient;
        private string _urbanDictionaryAPiURL = "http://api.urbandictionary.com/v0/define?term=";

        public void Initalize(IDiscordCoreEvents discordEventClient)
        {
            _discordEventClient = discordEventClient;
        }

        private void _discordEventClient_newMessage(object sender, NewMessageEventArgs e)
        {
            if (e.NewMessage.Contains("@DefinitionFinder"))
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                List<Assembly> assems = currentDomain.GetAssemblies().ToList();
                string searchString = _urbanDictionaryAPiURL + e.NewMessage.Replace("@DefinitionFinder", "");
                var response = ApiCall(searchString);
                string formattedResult = FormatResponse(response);
                _discordEventClient.PostMessage(e.ChannelId, _name, formattedResult);
            }
        }

        private UrbanDictionaryResponse ApiCall(string searchString)
        {
            var client = new RestClient(searchString);
            var response = client.Execute<List<UrbanDictionaryResponse>>(new RestRequest());
            var releases = response.Data;
            return releases.FirstOrDefault();
        }

        private string FormatResponse(UrbanDictionaryResponse response)
        {
            string returnString = " ";
            returnString += Environment.NewLine;
            if (response.result_type != "no_results")
            {
                returnString += "**Meaning**: " + response.list.First().definition + Environment.NewLine;
                returnString += "**Example**: " + response.list.First().example + Environment.NewLine;
            }
            else
            {
                returnString += "**Unable to find definition**";
            }
            return returnString;
        }

        public void Start()
        {
            _discordEventClient.NewMessage += _discordEventClient_newMessage;
        }

        public void Stop()
        {
            _discordEventClient.NewMessage -= _discordEventClient_newMessage;
        }

        public string GetPluginName()
        {
            return _name;
        }

        public string GetPluginDescription()
        {
            return _description;
        }
    }
}