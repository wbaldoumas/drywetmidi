﻿using Melanchall.DryWetMidi.Common;
using System;

namespace Melanchall.DryWetMidi.Core
{
    /// <summary>
    /// Represents a Key Signature meta event.
    /// </summary>
    /// <remarks>
    /// The MIDI key signature meta message specifies the key signature and scale of a MIDI file.
    /// </remarks>
    public sealed class KeySignatureEvent : MetaEvent
    {
        #region Constants

        /// <summary>
        /// Default key (C).
        /// </summary>
        public const sbyte DefaultKey = 0;

        /// <summary>
        /// Default scale (major).
        /// </summary>
        public const byte DefaultScale = 0;

        // TODO: public
        private const sbyte MinKey = -7;
        private const sbyte MaxKey = 7;

        private const byte MinScale = 0;
        private const byte MaxScale = 1;

        #endregion

        #region Fields

        private sbyte _key = DefaultKey;
        private byte _scale = DefaultScale;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="KeySignatureEvent"/>.
        /// </summary>
        public KeySignatureEvent()
            : base(MidiEventType.KeySignature)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeySignatureEvent"/> with the
        /// specified key and scale.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scale"></param>
        public KeySignatureEvent(sbyte key, byte scale)
            : this()
        {
            Key = key;
            Scale = scale;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets key signature in terms of number of flats (if negative) or
        /// sharps (if positive).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Key is out of range.</exception>
        public sbyte Key
        {
            get { return _key; }
            set
            {
                ThrowIfArgument.IsOutOfRange(nameof(value), value, MinKey, MaxKey, "Key is out of range.");

                _key = value;
            }
        }

        /// <summary>
        /// Gets or sets scale (0 for major or 1 for minor).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Scale is out of range.</exception>
        public byte Scale
        {
            get { return _scale; }
            set
            {
                ThrowIfArgument.IsOutOfRange(nameof(value), value, MinScale, MaxScale, "Scale is out of range.");

                _scale = value;
            }
        }

        #endregion

        #region Methods

        private int ProcessValue(int value, string property, int min, int max, InvalidMetaEventParameterValuePolicy policy)
        {
            if (value >= min && value <= max)
                return value;

            switch (policy)
            {
                case InvalidMetaEventParameterValuePolicy.Abort:
                    throw new InvalidMetaEventParameterValueException(GetType(), property, value);
                case InvalidMetaEventParameterValuePolicy.SnapToLimits:
                    return Math.Min(Math.Max(value, min), max);
            }

            return value;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Reads content of a MIDI meta event.
        /// </summary>
        /// <param name="reader">Reader to read the content with.</param>
        /// <param name="settings">Settings according to which the event's content must be read.</param>
        /// <param name="size">Size of the event's content.</param>
        protected override void ReadContent(MidiReader reader, ReadingSettings settings, int size)
        {
            var invalidMetaEventParameterValuePolicy = settings.InvalidMetaEventParameterValuePolicy;

            Key = (sbyte)ProcessValue(reader.ReadSByte(),
                                       nameof(Key),
                                       MinKey,
                                       MaxKey,
                                       invalidMetaEventParameterValuePolicy);

            Scale = (byte)ProcessValue(reader.ReadByte(),
                                        nameof(Scale),
                                        MinScale,
                                        MaxScale,
                                        invalidMetaEventParameterValuePolicy);
        }

        /// <summary>
        /// Writes content of a MIDI meta event.
        /// </summary>
        /// <param name="writer">Writer to write the content with.</param>
        /// <param name="settings">Settings according to which the event's content must be written.</param>
        protected override void WriteContent(MidiWriter writer, WritingSettings settings)
        {
            writer.WriteSByte(Key);
            writer.WriteByte(Scale);
        }

        /// <summary>
        /// Gets the size of the content of a MIDI meta event.
        /// </summary>
        /// <param name="settings">Settings according to which the event's content must be written.</param>
        /// <returns>Size of the event's content.</returns>
        protected override int GetContentSize(WritingSettings settings)
        {
            return 2;
        }

        /// <summary>
        /// Clones event by creating a copy of it.
        /// </summary>
        /// <returns>Copy of the event.</returns>
        protected override MidiEvent CloneEvent()
        {
            return new KeySignatureEvent(Key, Scale);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Key Signature ({Key}, {Scale})";
        }

        #endregion
    }
}
