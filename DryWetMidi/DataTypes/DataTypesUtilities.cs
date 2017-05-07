﻿using System;

namespace Melanchall.DryWetMidi
{
    /// <summary>
    /// Internal utilities to manipulate MIDI data types.
    /// </summary>
    public static class DataTypesUtilities
    {
        #region Methods

        /// <summary>
        /// Merges two four-bit numbers into one byte.
        /// </summary>
        /// <param name="head"><see cref="FourBitNumber"/> representing left part of resulting number.</param>
        /// <param name="tail"><see cref="FourBitNumber"/> representing right part of resulting number.</param>
        /// <returns>Single byte made of four-bit halfs.</returns>
        public static byte Combine(FourBitNumber head, FourBitNumber tail)
        {
            return (byte)((head << 4) | tail);
        }

        /// <summary>
        /// Merges two seven-bit numbers into one 16-bit unsigned integer number.
        /// </summary>
        /// <param name="head"><see cref="SevenBitNumber"/> representing left part of resulting number.</param>
        /// <param name="tail"><see cref="SevenBitNumber"/> representing right part of resulting number.</param>
        /// <returns>Single unsigned 16-bit integer number made of seven-bit halfs.</returns>
        public static ushort Combine(SevenBitNumber head, SevenBitNumber tail)
        {
            return (byte)((head << 7) | tail);
        }

        /// <summary>
        /// Merges two signed bytes into one 16-bit signed integer number.
        /// </summary>
        /// <param name="head">Byte representing left part of resulting number.</param>
        /// <param name="tail">Byte representing right part of resulting number.</param>
        /// <returns>Single signed 16-bit integer number made of byte halfs.</returns>
        public static short Combine(byte head, byte tail)
        {
            return (short)((head << 8) | tail);
        }

        /// <summary>
        /// Extracts right four-bit part of a byte.
        /// </summary>
        /// <param name="number">Byte to extract right part of.</param>
        /// <returns><see cref="FourBitNumber"/> representing the right part of the byte.</returns>
        public static FourBitNumber GetTail(this byte number)
        {
            return (FourBitNumber)(number & FourBitNumber.MaxValue);
        }

        /// <summary>
        /// Extracts right seven-bit part of an unsigned 16-bit integer number.
        /// </summary>
        /// <param name="number">Number to extract right part of.</param>
        /// <returns><see cref="SevenBitNumber"/> representing the right part of the unsigned 16-bit integer number.</returns>
        public static SevenBitNumber GetTail(this ushort number)
        {
            return (SevenBitNumber)(number & SevenBitNumber.MaxValue);
        }

        /// <summary>
        /// Extracts right eight-bit part of an nsigned 16-bit integer number.
        /// </summary>
        /// <param name="number">Number to extract right part of.</param>
        /// <returns>Byte representing the right part of the signed 16-bit integer number.</returns>
        public static byte GetTail(this short number)
        {
            return (byte)(number & byte.MaxValue);
        }

        /// <summary>
        /// Extracts left four-bit part of a byte.
        /// </summary>
        /// <param name="number">Byte to extract left part of.</param>
        /// <returns><see cref="FourBitNumber"/> representing the left part of the byte.</returns>
        public static FourBitNumber GetHead(this byte number)
        {
            return (FourBitNumber)(number >> 4);
        }

        /// <summary>
        /// Extracts left seven-bit part of an unsigned 16-bit integer number.
        /// </summary>
        /// <param name="number">Number to extract left part of.</param>
        /// <returns><see cref="SevenBitNumber"/> representing the left part of the unsigned 16-bit integer number.</returns>
        public static SevenBitNumber GetHead(this ushort number)
        {
            return (SevenBitNumber)(number >> 7);
        }

        /// <summary>
        /// Extracts left eight-bit part of an signed 16-bit integer number.
        /// </summary>
        /// <param name="number">Number to extract left part of.</param>
        /// <returns>Byte representing the left part of the signed 16-bit integer number.</returns>
        public static byte GetHead(this short number)
        {
            return (byte)(number >> 8);
        }

        /// <summary>
        /// Gets length of variable-length quantity (VLQ) representation of an integer number.
        /// </summary>
        /// <param name="number">Number to calculate VLQ length for.</param>
        /// <returns>Bytes count required to represent the number in VLQ.</returns>
        /// <remarks>
        /// Numbers in VLQ format are represented 7 bits per byte, most significant bits first.
        /// All bytes except the last have bit 7 set, and the last byte has bit 7 clear. If the
        /// number is between 0 and 127, it is thus represented exactly as one byte.
        /// The largest number which is allowed is 0FFFFFFF so that the VLQ representations
        /// must fit in 32 bits in a routine to write variable-length numbers.
        /// </remarks>
        public static int GetVlqLength(this int number)
        {
            var mask = 1 << 30;
            var bitsCount = 31;

            while ((number & mask) == 0 && mask > 0)
            {
                mask >>= 1;
                bitsCount--;
            }

            return Math.Max(bitsCount / 7 + (bitsCount % 7 > 0 ? 1 : 0), 1);
        }

        /// <summary>
        /// Gets bytes of a number in variable-length quantity (VLQ) format.
        /// </summary>
        /// <param name="number">Number to get VLQ bytes for.</param>
        /// <returns>Bytes representing the number coded in VLQ.</returns>
        /// <remarks>
        /// Numbers in VLQ format are represented 7 bits per byte, most significant bits first.
        /// All bytes except the last have bit 7 set, and the last byte has bit 7 clear. If the
        /// number is between 0 and 127, it is thus represented exactly as one byte.
        /// The largest number which is allowed is 0FFFFFFF so that the VLQ representations
        /// must fit in 32 bits in a routine to write variable-length numbers.
        /// </remarks>
        public static byte[] GetVlqBytes(this int number)
        {
            var result = new byte[number.GetVlqLength()];
            var i = result.Length - 1;

            result[i--] = (byte)(number & 127);

            while ((number >>= 7) > 0)
            {
                result[i] |= 128;
                result[i--] += (byte)(number & 127);
            }

            return result;
        }

        #endregion
    }
}