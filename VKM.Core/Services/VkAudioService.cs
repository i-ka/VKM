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
            newList.Add(new Audio("Royal Republic", "Tommy-Gun", 240, "https://cs3-2v4.vk-cdn.net/p14/e8d84a086abb0d.mp3?extra=8oxAmF1KHXVX3klLbTMHa39kgNA6rrG5g1Awp7hcXOPdtmFVo2gGeFW879pwIzBtakZkeEOr83MDNaiosHxXGlaUtDYDgm_zI6KwcWRDy4_s1uhlcu12-VpIrxkLzs_jA5P6GF6u"));
            newList.Add(new Audio("Royal Republic", "Addictive", 300, "https://cs3-3v4.vk-cdn.net/p1/5756ae9fbdbc59.mp3?extra=dqrqdvbEU3QAybM_VCpM3ScsHn3342reaMofFerIGcxEFf-DGGjz4a8ZYnEVn3JlG6mtYPvz38OHip1j7kIwmJFoQsTWE7WulF2GG0qUKoqYASUTJd1eOa5XMAZ9QL1nvJbSA2HPn94z"));
            newList.Add(new Audio("Royal Republic", "Underwear", 300, "https://cs3-2v4.vk-cdn.net/p18/50cd36ed751da4.mp3?extra=s_KOQPNxsv7tbhtodMjHonxRPdcne6AysyZuddMqvI4BnqS9T_xLxKn8lLQm2WjvndOsbfLyGApczbHv9u10y9iszeStHbKGwYoFfMEG6rf0WqS4YtzcCiSmPeJwY4pg2a_pTM1e6A"));
            return newList;
        }
    }
}
