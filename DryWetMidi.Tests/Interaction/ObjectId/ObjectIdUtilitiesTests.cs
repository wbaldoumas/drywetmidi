﻿using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using NUnit.Framework;
using System;
using System.Linq;

namespace Melanchall.DryWetMidi.Tests.Interaction
{
    [TestFixture]
    internal class ObjectIdUtilitiesTests
    {
        #region Test methods

        [Test]
        public void GetObjectId_AllObjectsTypes()
        {
            var baseType = typeof(ITimedObject);
            var objectsTypes = baseType
                .Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && baseType.IsAssignableFrom(t) && t != typeof(ValueChange<>))
                .ToArray();

            Assert.IsTrue(objectsTypes.Any(), "No object types found.");

            foreach (var type in objectsTypes)
            {
                ITimedObject obj = null;

                if (type == typeof(Rest))
                    obj = new Rest(0, 100, null, null);
                else if (type == typeof(Note))
                    obj = new Note((SevenBitNumber)70);
                else if (type == typeof(TimedEvent))
                    obj = new TimedEvent(new TextEvent("A"));
                else
                    obj = (ITimedObject)Activator.CreateInstance(type);
                
                var objectId = obj.GetObjectId();
                Assert.IsNotNull(objectId, $"Null ID for [{obj}] object.");
            }
        }

        #endregion
    }
}
