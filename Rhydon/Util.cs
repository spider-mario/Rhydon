﻿using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Rhydon.Properties;

namespace Rhydon
{
    public class Util
    {
        public static Font Pokered_US;
        public static bool UseFancySprites = true;

        private static readonly Bitmap[] partysprites = { Resources.SPRITE_MON, Resources.SPRITE_BALL, Resources.SPRITE_HELIX, Resources.SPRITE_FAIRY, Resources.SPRITE_BIRD_M, Resources.SPRITE_WATER, Resources.SPRITE_BUG, Resources.SPRITE_GRASS, Resources.SPRITE_SNAKE, Resources.SPRITE_QUADRUPED };

        public static Bitmap GetPartySprite(PK1 pk)
        {
            return UseFancySprites
                ? (Resources.ResourceManager.GetObject($"_{Tables.ID_To_Dex[pk.Species]}_0") as Image ?? new Bitmap(16, 16)).Clone() as Bitmap
                : GetPartySprite(Tables.ID_To_Sprite[pk.Species]);
        }

        public static Bitmap GetPartySprite(int index)
        {
            return partysprites[index].Clone() as Bitmap;
        }

        // Message Displays (From PKHeX)
        internal static DialogResult Error(params string[] lines)
        {
            System.Media.SystemSounds.Exclamation.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal static DialogResult Alert(params string[] lines)
        {
            System.Media.SystemSounds.Asterisk.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        internal static DialogResult Prompt(MessageBoxButtons btn, params string[] lines)
        {
            System.Media.SystemSounds.Question.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Prompt", btn, MessageBoxIcon.Asterisk);
        }

        public static byte[] GetBytes(ushort value)
        {
            return BitConverter.IsLittleEndian 
                ? BitConverter.GetBytes(value).Reverse().ToArray() 
                : BitConverter.GetBytes(value);
        }

        public static byte[] GetBytes(uint value)
        {
            return BitConverter.IsLittleEndian 
                ? BitConverter.GetBytes(value).Reverse().ToArray() 
                : BitConverter.GetBytes(value);
        }

        public static ushort ToUInt16(byte[] d, int i)
        {
            return BitConverter.IsLittleEndian
                ? (ushort)IPAddress.HostToNetworkOrder((short) BitConverter.ToUInt16(d, i))
                : BitConverter.ToUInt16(d, i);
        }

        public static uint ToUInt32(byte[] d, int i)
        {
            return BitConverter.IsLittleEndian
                ? (uint)IPAddress.HostToNetworkOrder((int) BitConverter.ToUInt32(d, i))
                : BitConverter.ToUInt32(d, i);
        }

        internal static int ToInt32(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value.Trim());
        }

        internal static uint ToUInt32(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? 0 : uint.Parse(value.Trim());
        }

        internal static int getIndex(ComboBox cb)
        {
            return (int) (cb?.SelectedValue ?? 0);
        }

        internal static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public class cbItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private static readonly PrivateFontCollection fonts = new PrivateFontCollection();
        public static void LoadFont()
        {
            try
            {
                byte[] fontData = Resources.Pokered_US;
                IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
                System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                uint dummy = 0;
                fonts.AddMemoryFont(fontPtr, Resources.Pokered_US.Length);
                AddFontMemResourceEx(fontPtr, (uint)Resources.Pokered_US.Length, IntPtr.Zero, ref dummy);
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

                Pokered_US = new Font(fonts.Families[0], 9.0F);
            }
            catch
            {
                Pokered_US = new Font("Consolas", 9.0F);
            }
        }
    }

}
