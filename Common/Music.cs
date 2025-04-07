using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Audio.OpenAL;
using OpenTK.Compute.OpenCL;

namespace opentk_cg.Common
{
    class OpenALPlayer
    {
        int buffer, source, sourceHit, bufferHit;
        ALDevice device;
        ALContext context;

        public void StartPlayback()
        {
            var error = AL.GetError();
            string filePath = "../../../Sound/music.wav"; // file path 

            // OpenAL init
            device = ALC.OpenDevice(null);
            context = ALC.CreateContext(device, (int[])null);
            if (device == nint.Zero)
            {
                Console.WriteLine("Error: OpenAL device not found.");
                return;
            }
            if (context == nint.Zero)
            {
                Console.WriteLine("Error: cannot create OpenAL context.");
                return;
            }

            ALC.MakeContextCurrent(context);

            // Source and buffer creation
            source = AL.GenSource();
            buffer = AL.GenBuffer();

            // Загрузка звука
            LoadWav(filePath, ref buffer);

            // Binding buffer to source and starting playback
            AL.Source(source, ALSourcei.Buffer, buffer);
            AL.Source(source, ALSourceb.Looping, true);
            AL.SourcePlay(source);

            int state;
            AL.GetSource(source, ALGetSourcei.SourceState, out state);
            Console.WriteLine($"Source state: {state}"); // must be AL.Playing (4114)

            filePath = "../../../Sound/hit.wav";
            sourceHit = AL.GenSource();
            bufferHit = AL.GenBuffer();

            LoadWav(filePath, ref bufferHit);

            AL.Source(sourceHit, ALSourcei.Buffer, bufferHit);
        }

        public void PlayHitSound()
        {
            AL.SourcePlay(sourceHit);
        }

        public void StopPlayback()
        {
            // Resource cleanup
            AL.SourceStop(source);
            AL.DeleteSource(source);
            AL.DeleteBuffer(buffer);
            AL.SourceStop(sourceHit);
            AL.DeleteSource(sourceHit);
            AL.DeleteBuffer(bufferHit);
            ALC.MakeContextCurrent(ALContext.Null);
            ALC.DestroyContext(context);
            ALC.CloseDevice(device);
        }

        void LoadWav(string path, ref int buffer)
        {
            using (FileStream fs = File.OpenRead(path))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                reader.BaseStream.Seek(22, SeekOrigin.Begin);
                short channels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                reader.BaseStream.Seek(34, SeekOrigin.Begin);
                short bitsPerSample = reader.ReadInt16();
                reader.BaseStream.Seek(40, SeekOrigin.Begin);
                int dataSize = reader.ReadInt32();
                nint data = Marshal.AllocHGlobal(dataSize);
                Marshal.Copy(reader.ReadBytes(dataSize), 0, data, dataSize);

                ALFormat format = channels == 1 && bitsPerSample == 8 ? ALFormat.Mono8 :
                                  channels == 1 && bitsPerSample == 16 ? ALFormat.Mono16 :
                                  channels == 2 && bitsPerSample == 8 ? ALFormat.Stereo8 :
                                  ALFormat.Stereo16;

                //dataSize -= dataSize % 4;
                Console.WriteLine($"Format: {format}, Buffer: {buffer}, Rate: {sampleRate}, Data size: {dataSize}");
                AL.BufferData(buffer, format, data, dataSize, sampleRate);
                Marshal.FreeHGlobal(data);
            }
        }
    }
}