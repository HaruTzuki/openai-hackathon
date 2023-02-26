using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALL_E_Conectivity
{
    public class PlantDiseases
    {
        public string? PlantName { get; set; }
        public string? DiseaseName { get; set; }
        public PlantDiseases(string? plantName, string? diseaseName) {
            PlantName = plantName;
            DiseaseName = diseaseName;            
        }
    }
}
