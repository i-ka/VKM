using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    class VkAudioService : IVkAudioService
    {
        public List<Audio> GetAudioList()
        {
            var newList = new List<Audio>();
            newList.Add(new Audio("SomeAuthor0", "SomeTrack0", 240));
            newList.Add(new Audio("SomeAuthor1", "SomeTrack1", 300));
            newList.Add(new Audio("SomeAuthor2", "SomeTrack2", 180));
            newList.Add(new Audio("SomeAuthor3", "SomeTrack3", 158));
            newList.Add(new Audio("SomeAuthor4", "SomeTrack4", 262));
            newList.Add(new Audio("SomeAuthor5", "SomeTrack5", 175));
            newList.Add(new Audio("SomeAuthor6", "SomeTrack6", 175));
            newList.Add(new Audio("SomeAuthor7", "SomeTrack7", 175));
            newList.Add(new Audio("SomeAuthor8", "SomeTrack8", 175));
            newList.Add(new Audio("SomeAuthor9", "SomeTrack9", 175));
            return newList;
        }
    }
}
