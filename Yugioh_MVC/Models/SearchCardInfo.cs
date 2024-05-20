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
    }
}
