using System.Collections.Generic;

namespace DiscordCore.Plugin.DefinitionFinder
{
    public class UrbanDictionaryResponseItem
    {
        public string definition { get; set; }
        public string permalink { get; set; }
        public int thumbs_up { get; set; }
        public string author { get; set; }
        public string word { get; set; }
        public int defid { get; set; }
        public string current_vote { get; set; }
        public string example { get; set; }
        public int thumbs_down { get; set; }
    }

    public class UrbanDictionaryResponse
    {
        public List<string> tags { get; set; }
        public string result_type { get; set; }
        public List<UrbanDictionaryResponseItem> list { get; set; }
        public List<string> sounds { get; set; }
    }
}