using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SpamBotRemaster.Infrastructure.Tools
{
    internal static class NativeMethods
    {
        [Flags]
        public enum KeyboardFlags : uint
        {
            None = 0,
            ExtendedKey = 1,
            KeyUp = 2,
            Unicode = 4,
            ScanCode = 8,
        }

        [Flags]
        public enum MouseFlags : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            VerticalWheel = 0x0800,
            HorizontalWheel = 0x1000,
            VirtualDesk = 0x4000,
            Absolute = 0x8000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort virtualKey;
            public ushort scanCode;
            public KeyboardFlags flags;
            public uint timeStamp;
            public IntPtr extraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int deltaX;
            public int deltaY;
            public uint mouseData;
            public MouseFlags flags;
            public uint time;
            public IntPtr extraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint message;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mouse;
            [FieldOffset(0)]
            public KEYBDINPUT keyboard;
            [FieldOffset(0)]
            public HARDWAREINPUT hardware;
        }
        public enum InputType : int
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }
        public struct INPUT
        {
            public InputType type;
            public InputUnion union;
        }

        public static int SizeOfInput { get; } = Marshal.SizeOf(typeof(INPUT));

        [DllImport("user32.dll")]

        private static extern uint SendInput(int nInputs, INPUT[] pInputs, int cbSize);


        public static void SendKey(Key key, ModifierKeys modifiers = ModifierKeys.None)
        {
            static INPUT BuildINPUT(Key k, KeyboardFlags flags) => new INPUT
            {
                type = InputType.Keyboard,
                union = new InputUnion { keyboard = new KEYBDINPUT { virtualKey = (ushort)KeyInterop.VirtualKeyFromKey(k), scanCode = 0, flags = flags, timeStamp = 0, extraInfo = IntPtr.Zero } }
            };
            List<Key> keys = new List<Key>();
            if (modifiers.HasFlag(ModifierKeys.Control)) keys.Add(Key.LeftCtrl);
            if (modifiers.HasFlag(ModifierKeys.Alt)) keys.Add(Key.LeftAlt);
            if (modifiers.HasFlag(ModifierKeys.Shift)) keys.Add(Key.LeftShift);
            if (modifiers.HasFlag(ModifierKeys.Windows)) keys.Add(Key.LWin);
            keys.Add(key);
            INPUT[] inputs = new INPUT[keys.Count * 2];
            for (int i = 0; i < keys.Count; i++)
            {
                inputs[i] = BuildINPUT(keys[i], KeyboardFlags.None);
                inputs[^(i + 1)] = BuildINPUT(keys[i], KeyboardFlags.KeyUp);
            }
            _ = SendInput(inputs.Length, inputs, SizeOfInput);
        }
    }
}

