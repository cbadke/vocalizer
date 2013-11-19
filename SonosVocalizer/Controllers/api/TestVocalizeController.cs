using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Speech.Synthesis;
using System.Web.Http;

namespace SonosVocalizer.Controllers.api
{
    public class TestVocalizeController : ApiController
    {
        static Dictionary<Guid, Stream> streams = new Dictionary<Guid, Stream>();

        public HttpResponseMessage Get(Guid id)
        {
            if (streams.ContainsKey(id))
            {
                var stream = streams[id];
                streams.Remove(id);

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StreamContent(stream);

                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                resp.Content.Headers.ContentDisposition.FileName = id.ToString() + ".wav";

                return resp;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public dynamic Post(dynamic req)
        {
            try 
            {
                string voice = null;
                string phrase = req["phrase"].Value;

                try { voice = req["voice"].Value; } catch { }

                var id = Guid.NewGuid();

                var stream = new MemoryStream();
                var t = new System.Threading.Thread(() =>
                    {
                        using (var synth = new SpeechSynthesizer())
                        {
                            if (voice != null)
                            {
                                synth.SelectVoice(voice);
                            }
                            synth.SetOutputToWaveStream(stream);
                            synth.Speak(phrase);
                            synth.SetOutputToNull();
                        }
                    });

                t.Start();
                t.Join();

                stream.Position = 0;

                streams.Add(id, stream);

                return new { id = id };
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
