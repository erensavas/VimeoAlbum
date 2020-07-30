using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VimeoDotNet.Enums;

namespace VimeoDotNet.Parameters
{
   public class AddToAlbumParameters : IParameterProvider
    {


  

       
            /// <summary>
            /// Name
            /// </summary>
            [PublicAPI]
            public string videos { get; set; }

           

           
            public IDictionary<string, string> GetParameterValues()
            {
                var parameterValues = new Dictionary<string, string>();

                

                if (videos != null)
                {
                    parameterValues.Add("videos", videos);
                }

               

                return parameterValues.Keys.Count > 0 ? parameterValues : null;
            }

        public string ValidationError()
        {
            if (string.IsNullOrEmpty(videos))
            {
                return "videos parametresi gerekli";
            }

            return null;
        }
    }
}
 
 
