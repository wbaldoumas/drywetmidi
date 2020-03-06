﻿using Melanchall.DryWetMidi.Core;
using NUnit.Framework;

namespace Melanchall.DryWetMidi.Tests.Core
{
    [TestFixture]
    public sealed class MidiChunkTests
    {
        #region Test methods

        [Test]
        public void GetStandardChunkIds()
        {
            var ids = MidiChunk.GetStandardChunkIds();
            CollectionAssert.AreEqual(new[] { HeaderChunk.Id, TrackChunk.Id }, ids, "IDs are invalid.");
        }

        #endregion
    }
}
