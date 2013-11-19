using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Speech.Synthesis;

namespace SonosVocalizer.Controllers
{
    public class VoicesController : ApiController
    {
        public IEnumerable<dynamic> Get()
        {
            IEnumerable<dynamic> voices = null;

            var t = new System.Threading.Thread(() =>
                {
                    using (var synth = new SpeechSynthesizer())
                    {
                        voices = synth.GetInstalledVoices()
                                      .Where(v => v.Enabled)
                                      .Select(v => new
                                       {
                                           name = v.VoiceInfo.Name,
                                           age = v.VoiceInfo.Age,
                                           culture = v.VoiceInfo.Culture,
                                           gender = v.VoiceInfo.Gender
                                       }).ToList();
                    }
                });

            t.Start();
            t.Join();

            return voices;
        }
    }
}
