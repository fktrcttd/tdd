﻿using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Classes;

namespace TagCloudTests
{
    [TestFixture]
    class SpiralGenerator_Tests
    {
        private SpiralGenerator spiralGenerator;

        [SetUp]
        public void SetUp()
        {
            spiralGenerator = new SpiralGenerator(new Point(100, 100));
        }
        
        [Test]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        public void ConstructorThrowsException_WhenArgsIsNegative(int x, int y)
        {
            var center = new Point(x, y);
            Action act = () =>
            {
                new CircularCloudLayouter(center);
            };
            act.Should().Throw<ArgumentException>();
        }
    }
}
