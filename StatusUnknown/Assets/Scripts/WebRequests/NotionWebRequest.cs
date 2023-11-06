using System;
using UnityEngine.Networking;

// TODO : reuse code from PokeWebReqest but getting it through the Notion API (pipedream hook ?)
namespace StatusUnknown
{
    namespace WebRequest
    {
        /// <summary>
        /// To be done. This class handles pipeline with Status Unknown's Notion to generate/update a scriptable object from documentation, 
        /// or to generate/update a documentation template in Notion
        /// </summary>
        public class NotionWebRequest : WebRequestBase
        {
            void Start()
            {

            }

            protected override void Populate_OnGetComplete(UnityWebRequest uwb)
            {
                throw new NotImplementedException();
            }

            protected override void Populate_OnPostComplete(UnityWebRequest uwb)
            {
                throw new NotImplementedException();
            }
        }
    }
}

