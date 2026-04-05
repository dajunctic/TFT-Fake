using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DTT.AreaOfEffectRegions.Tests
{
    public class LineRegionTests
    {
        /// <summary>
        /// The line region being tested.
        /// </summary>
        private LineRegion _region;
        
        /// <summary>
        /// Creates a new line region.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _region = new GameObject().AddComponent<LineRegion>();
        }

        /// <summary>
        /// Destroys the line region.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_region.gameObject);
        }

        /// <summary>
        /// Check for the correct end position of the line.
        /// </summary>
        [Test]
        public void EndPosition_Test()
        {
            // Arrange.
            _region.Length = 1;
            _region.Angle = 0;
            _region.Width = 1;
            
            // Act.
            Vector3 position = _region.EndPosition;

            // Assert.
            Assert.IsTrue(Vector3.forward == position);
        }

        /// <summary>
        /// Checks for the correct width offset.
        /// </summary>
        [Test]
        public void WidthOffset_Test()
        {
            // Arrange.
            _region.Length = 1;
            _region.Angle = 0;
            _region.Width = 1;
            
            // Act.
            Vector3 widthOffset = _region.WidthOffset;

            // Assert.
            Assert.IsTrue(Vector3.left / 2 == widthOffset);
        }

        /// <summary>
        /// Tests for the left hand side start position.
        /// </summary>
        [Test]
        public void LeftHandSideStart_Test()
        {
            // Arrange.
            _region.Length = 1;
            _region.Angle = 0;
            _region.Width = 1;
            
            // Act.
            Vector3 lhs = _region.LeftHandSideStart;

            // Assert.
            Assert.IsTrue(Vector3.left / 2 == lhs);
        }

        /// <summary>
        /// Tests for the left hand side end position.
        /// </summary>
        [Test]
        public void LeftHandSideEnd_Test()
        {
            // Arrange.
            _region.Length = 1;
            _region.Angle = 0;
            _region.Width = 1;
            
            // Act.
            Vector3 lhs = _region.LeftHandSideEnd;

            // Assert.
            Assert.IsTrue(Vector3.forward + Vector3.left / 2 == lhs);
        }

        /// <summary>
        /// Tests for the right hand side start position.
        /// </summary>
        [Test]
        public void RightHandSideStart_Test()
        {
            // Arrange.
            _region.Length = 1;
            _region.Angle = 0;
            _region.Width = 1;
            
            // Act.
            Vector3 rhs = _region.RightHandSideStart;

            // Assert.
            Assert.IsTrue(Vector3.right / 2 == rhs);
        }

        /// <summary>
        /// Tests for the right hand side end position.
        /// </summary>
        [Test]
        public void RightHandSideEnd_Test()
        {
            // Arrange.
            _region.Length = 1;
            _region.Angle = 0;
            _region.Width = 1;
            
            // Act.
            Vector3 rhs = _region.RightHandSideEnd;

            // Assert.
            Assert.IsTrue(Vector3.forward + Vector3.right / 2 == rhs);
        }
    }
}