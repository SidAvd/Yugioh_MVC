using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace Yugioh_MVC.Models
{
    /// <summary>
    /// Creates the final endpoint, with the use of the search form filters, to fetch the result cards from the YuGiOh API.
    /// </summary>
    public static class EndpointCreator
    {
        // The dictionary pairs the strings used to create the endpoint and what the search form user has chosen.
        // Creates and returns the final endpoint based on the parameters given by the search form user.
        public static string CreateFinalEndpoint(SearchCardInfo searchCardInfo, Uri basicEndpoint)
        {
            Dictionary<string, string> EndpointDictionary = [];
            EndpointDictionary.Add("fname", searchCardInfo.Name);
            EndpointDictionary.Add("type", searchCardInfo.Type);
            EndpointDictionary.Add("attribute", searchCardInfo.Attribute);
            EndpointDictionary.Add("race", searchCardInfo.Race);
            EndpointDictionary.Add("archetype", searchCardInfo.Archetype);
            EndpointDictionary.Add("level", searchCardInfo.Level.ToString());
            EndpointDictionary.Add("atk", searchCardInfo.Atk.ToString());
            EndpointDictionary.Add("def", searchCardInfo.Def.ToString());

            string parameters = "?";
            foreach (var pair in EndpointDictionary)
                if (!(string.IsNullOrEmpty(pair.Value)))
                    parameters += pair.Key + "=" + pair.Value + "&";

            return basicEndpoint + parameters;
        }
    }
}
