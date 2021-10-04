using Newtonsoft.Json;
using System.Collections.Generic;

namespace katal.conexion.model.entity
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Root
    {
        [JsonProperty("gridLookup")]
        public List<string> GridLookup { get; set; }

        [JsonProperty("grid_DXEFL_DXEditor1")]
        public string GridDXEFLDXEditor1 { get; set; }

        [JsonProperty("grid_DXEFL_DXEditor2")]
        public object GridDXEFLDXEditor2 { get; set; }

        [JsonProperty("grid_DXEFL_DXEditor3")]
        public string GridDXEFLDXEditor3 { get; set; }

        [JsonProperty("grid_DXEFL_DXEditor5_DDD_C")]
        public object GridDXEFLDXEditor5DDDC { get; set; }

        [JsonProperty("grid_DXEFL_DXEditor5")]
        public object GridDXEFLDXEditor5 { get; set; }

        [JsonProperty("gridLookupCostos")]
        public List<string> GridLookupCostos { get; set; }

        [JsonProperty("grid_DXEFL_DXEditor7")]
        public string GridDXEFLDXEditor7 { get; set; }

        [JsonProperty("grid_DXEFL_DXEditor8")]
        public string GridDXEFLDXEditor8 { get; set; }

        [JsonProperty("gridLookup_DDD_gv_DXFREditorcol1")]
        public object GridLookupDDDGvDXFREditorcol1 { get; set; }

        [JsonProperty("gridLookup_DDD_gv_DXFREditorcol2")]
        public object GridLookupDDDGvDXFREditorcol2 { get; set; }
    }
}