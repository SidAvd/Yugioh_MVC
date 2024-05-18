namespace Yugioh_MVC.Models
{
    /// <summary>
    /// Used to pass static info from JSON file (StaticData.json) to drop down lists in search form and populate them.
    /// </summary>
    public class ListPopulator()
    {
        public List<string> attributes;
        public List<string> cardtypes;
        public List<string> races;
        public List<int> levels;
        public List<string> archetypes;
    }
}
