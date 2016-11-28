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
            newList.Add(new Audio("Royal Republic", "Tommy-Gun", 240, "https://cs3-2v4.vk-cdn.net/p24/3d4a7e8b83b5f6.mp3?extra=YU0H6Wk7hf-sGedNQQrNrRb7X09kN9O3OTP5tZ4B2e0fycf2MhW-qAoRKYceptqPICVKS41-kLpY90-z23ZaXyiFz9zZahCz1KJgO2abpOKRpUOuDD3QZMesvelvOcYYb0zeLtAEIVCaXAw"));
            newList.Add(new Audio("Royal Republic", "Addictive", 300, "https://cs3-3v4.vk-cdn.net/p1/73684f4c48454e.mp3?extra=m1wHunwIay1PwD-ddGVQRX9_KH8quA9AA5-gbHAWWLF3b9Aav28ioebDSfry1GYb24_X7o8Zi3gBib_1uPm3OU6bSFgtlgF6hLNgblonp45yB9uWjyTtyo3FqQc898gURDyZ6XA9vYbB"));
            newList.Add(new Audio("Royal Republic", "Underwear", 300, "https://cs3-2v4.vk-cdn.net/p18/8cc0a546ff7c82.mp3?extra=ugF1cYdYuooTTSRX_4CJUYJNoqUP2gj4eQgpRwQbV6NuoafAKNBiAxj5epySn0iZvntKUjeOh0mLhDaAfsomNCewiHNbyDEZokHFo8v5Xotv-PXhxwaez6GBcSnKWv8guvKDt-C2ow"));
            return newList;
        }
    }
}
