﻿using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Classes;
namespace TagCloudTests
{
    [TestFixture]
    class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter layouter ;
        
        [SetUp]
        public void SetUp()
        {
            var center = new Point(100, 100);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void LayouterIsEmpty_AfterCreation()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        public void Constructor_ThrowsArgumentException_OnNegativeCoordinates(int x, int y)
        {
            var center = new Point(x, y);
            Action act = () => new CircularCloudLayouter(center);;
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_SetrectangleToCenter_OnFirstTime()
        {
            var rectangle = layouter.PutNextRectangle(new Size(50, 40));
            var expectedCenter = new Point(100, 100);
            
            var firstRectangleCenter = rectangle.Location + new Size(rectangle.Width / 2, rectangle.Height / 2);
            
            firstRectangleCenter.Should().Be(expectedCenter);
        }

        [Test]
        public void GetRectangleCenter_EqualsLayouterCenter_FirstTime()
        {
            var rectangle = layouter.PutNextRectangle(new Size(25, 25));
            var expectedCenter = new Point(100, 100);

            var actual = layouter.GetRectangleCenter(rectangle);

            actual.Should().BeEquivalentTo(expectedCenter);;
        }

        [TestCase(0, 0, 100, 50, ExpectedResult = true, TestName = "Intersecting rectangles")]
        [TestCase(130, 130, 10, 40, ExpectedResult = false, TestName = "Non-ntersecting rectangles")]
        public bool IntersectsWithOtherRectangles(int x, int y, int width, int height)
        {
            var rectangle = new Rectangle(x, y, width, height);
            var rectangles = new[] { new Rectangle(0, 0, 10, 10), new Rectangle(20, 20, 100, 50) };
            return layouter.IntersectsWithRectangles(rectangle, rectangles);
        }
        
        [TearDown]
        public void CreateImade_IfTestFail()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = Path.Combine(TestContext.CurrentContext.TestDirectory, TestContext.CurrentContext.Test.Name + ".bmp");
                var vizualizer = new Vizualizer(new Size(1500, 900));
                vizualizer.DrawRectangles(layouter.Rectangles);
                vizualizer.SaveImage(path);
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }
    }
}
