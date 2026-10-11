using System;
using System.Runtime.InteropServices;
using System.Threading;
namespace GodfatherTheme

{

    internal class Program

    {

        // --- Windows Multimedia API (winmm.dll) ---

        [DllImport("winmm.dll")]

        private static extern uint midiOutOpen(out IntPtr lphmo, uint uDeviceID, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);



        [DllImport("winmm.dll")]

        private static extern uint midiOutShortMsg(IntPtr hmo, uint dwMsg);



        [DllImport("winmm.dll")]

        private static extern uint midiOutClose(IntPtr hmo);



        private static IntPtr hMidi;



        static void Main(string[] args)

        {

            Console.Title = "The Godfather Theme Player";

            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine("=========================================");

            Console.WriteLine(" THE GODFATHER THEME");

            Console.WriteLine(" (Speak Softly Love)");

            Console.WriteLine("=========================================\n");

            Console.ResetColor();



            // MIDI Cihazını Aç

            if (midiOutOpen(out hMidi, 0, IntPtr.Zero, IntPtr.Zero, 0) != 0)

            {

                Console.WriteLine("HATA: MIDI Cihazı açılamadı!");

                return;

            }



            // Enstrüman Ayarı: 21 = Akordeon (İsterseniz 0 = Piyano yapabilirsiniz)

            SetProgram(0, 21);



            // The Godfather Melodisi (MIDI Nota, Süre ms)

            var godfatherTheme = new (int note, int duration)[]

            {

// --- 1. Bölüm ---

(64, 300), (69, 600), (72, 300), (71, 300),

(69, 600), (72, 600), (69, 600), (71, 300),

(69, 300), (65, 600), (67, 600), (64, 1200),



(0, 400), // Es



// --- 2. Bölüm ---

(64, 300), (69, 600), (72, 300), (71, 300),

(69, 600), (72, 600), (69, 600), (71, 300),

(69, 300), (64, 600), (63, 600), (62, 1200),



(0, 400), // Es



// --- 3. Bölüm ---

(62, 300), (65, 600), (70, 300), (69, 300),

(67, 600), (70, 600), (67, 600), (69, 300),

(67, 300), (64, 600), (65, 600), (64, 1500)

            };



            Console.WriteLine("Melodi çalınıyor... (Durdurmak için Ctrl+C)\n");



            // Melodiyi Çalma Döngüsü

            foreach (var (note, duration) in godfatherTheme)

            {

                if (note == 0)

                {

                    Console.WriteLine(" [ Es ]");

                    Thread.Sleep(duration);

                }

                else

                {

                    NoteOn(0, note, 100);

                    Console.WriteLine($" Çalınan Nota: {GetNoteName(note)}");



                    Thread.Sleep(duration - 50);



                    NoteOff(0, note); // Artık ilgili notayı doğru şekilde susturuyor

                    Thread.Sleep(50);

                }

            }



            Thread.Sleep(1000);

            midiOutClose(hMidi);

            Console.WriteLine("\nParça bitti.");

        }



        // ==========================================

        // DÜZELTİLMİŞ MIDI YARDIMCI METOTLARI

        // ==========================================



        private static void NoteOn(int channel, int note, int velocity)

        {

            uint msg = (uint)(0x90 | channel | (note << 8) | (velocity << 16));

            midiOutShortMsg(hMidi, msg);

        }



        private static void NoteOff(int channel, int note)

        {

            // DÜZELTİLDİ: (note << 8) parametresi eklendi

            uint msg = (uint)(0x80 | channel | (note << 8) | (0 << 16));

            midiOutShortMsg(hMidi, msg);

        }



        private static void SetProgram(int channel, int program)

        {

            uint msg = (uint)(0xC0 | channel | (program << 8));

            midiOutShortMsg(hMidi, msg);

        }



        private static string GetNoteName(int midiNote)

        {

            string[] names = { "C (Do)", "C# (Do#)", "D (Re)", "D# (Re#)", "E (Mi)", "F (Fa)", "F# (Fa#)", "G (Sol)", "G# (Sol#)", "A (La)", "A# (Sib)", "B (Si)" };

            return names[midiNote % 12];

        }

    }

}