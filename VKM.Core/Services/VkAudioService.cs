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
            newList.Add(new Audio("Royal Republic", "Tommy-Gun", 240, "https://cs1-39v4.vk-cdn.net/p3/f8976024b186b0.mp3?extra=g8g_BX0Tew0zirBbXNMITv7kxfFz_ESbgG1jsDJKS-kFn149Lrplr64LG9vq3izujeCWRTCz-m7h6w78_Wpp9YomRejbrYXpLHDcv3M3C7mEaGVUm_1DX6GrPikhAHRGdSU1JI69v6MA"));
            newList.Add(new Audio("Royal Republic", "Addictive", 300, "https://cs1-50v4.vk-cdn.net/p22/301ae94e77c654.mp3?extra=JAJ-Xdi1KxjkiW5y5aFRARmyMSq9QPoGf43uad2B_zS5L2bDRzmufsAYke3jAPOxASoYCrsztz6j3hPHpYobbQmAnTnBCJ9cyyUkKiGCYARosPQYjZPuVnCVCnLm1G5LzZ45kS0Syps"));
            newList.Add(new Audio("Royal Republic", "Underwear", 300, "https://cs1-51v4.vk-cdn.net/p7/2a29b740d9fcd5.mp3?extra=vegpI22Lk464DWYzQjzta4JirPDL3cj_KytuYA52PyWvFP6Q1Y1gq6nhfMLeGIYJB95EChAB4KranM2__Dfzfh-FSU-M2iZkO8t7rs3bgCjmfju7MvkChsRHMlZl2fQ3jbrNbho88Q"));
            return newList;
        }
    }
}
