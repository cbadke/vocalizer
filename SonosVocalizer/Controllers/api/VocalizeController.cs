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
        public dynamic Post(dynamic req)
        {
            try 
            {
                string voice = null;

                string phrase = req["phrase"].Value;

                try { voice = req["voice"].Value; } catch { }

                var id = Guid.NewGuid();
                var waveDir = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                    "SonosVocalizer");
                var waveFile = Path.Combine(waveDir,
                                    id + ".wav");

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