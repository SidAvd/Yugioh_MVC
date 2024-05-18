using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Threading;
using static Yugioh_MVC.Models.CardResults;


namespace Yugioh_MVC.Models
{
    /// <summary>
    /// Holds all the info from the search form filters that the user has chosen.
    /// Creates the final endpoint, with the use of the search form filters, to fetch the result cards from the YuGiOh API.
    /// </summary>
    public class SearchCardInfo
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? Atk { get; set; }
        public int? Def { get; set; }
        public int? Level { get; set; }
        public string? Race { get; set; }
        public string? Attribute { get; set; }
        public string? Archetype { get; set; }

        public Dictionary<string, string> EndpointDictionary = [];

        // The dictionary pairs the strings used in the endpoint and what the search form user has chosen
        public void CreateEndpointDictionary()
        {
            EndpointDictionary.Add("fname", Name);
            EndpointDictionary.Add("type", Type);
            EndpointDictionary.Add("attribute", Attribute);
            EndpointDictionary.Add("race", Race);
            EndpointDictionary.Add("archetype", Archetype);
            EndpointDictionary.Add("level", Level.ToString());
            EndpointDictionary.Add("atk", Atk.ToString());
            EndpointDictionary.Add("def", Def.ToString());
        }

        // Creates the final endpoint based on the parameters given by the search form user (from the dictionary pairs)
        public string ReturnEndpoint(Uri basicEndpoint)
        {
            string parameters = "?";
            foreach (var pair in EndpointDictionary)
                if(!(string.IsNullOrEmpty(pair.Value)))
                    parameters += pair.Key + "=" + pair.Value + "&";
            return basicEndpoint + parameters;
        }
    }
}
