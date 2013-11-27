using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Speech.Synthesis;
using System.IO;

namespace SonosVocalizer.Controllers
{
    public class VocalizeController : ApiController
    {
        System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        public dynamic Post(dynamic req)
        {
            try 
            {
                string voice = null;

                string phrase = req["phrase"].Value;

                try { voice = req["voice"].Value; } catch { }

                var id = Guid.NewGuid();
                var waveDir = config.AppSettings.Settings["wavDirectory"].Value;
                var waveFile = Path.Combine(waveDir,
                                    "announcement.wav");

                if (!Directory.Exists(waveDir))
                    Directory.CreateDirectory(waveDir);

                var t = new System.Threading.Thread(() =>
                    {
                        using (var synth = new SpeechSynthesizer())
                        {
                            if (voice != null)
                            {
                                synth.SelectVoice(voice);
                            }
                            synth.SetOutputToWaveFile(waveFile);
                            synth.Speak(phrase);
                        }
                    });

                t.Start();
                t.Join();

                return new { result = true, phrase = phrase };
            }
            catch
            {
                return new { result = false };
            }

        }
    }
}