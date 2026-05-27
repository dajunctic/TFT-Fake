using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DTT.AreaOfEffectRegions.Tests
{
    public class ArcRegionTests
    {
        /// <summary>
        /// The region that is being tested.
        /// </summary>
        private ArcRegion _region;
        
        /// <summary>
        /// Creates a new arc region.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _region = new GameObject().AddComponent<ArcRegion>();
        }

        /// <summary>
        /// Destroys the arc region.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_region.gameObject);
        }
        
        /// <summary>
        /// Tests the end of the left position to match.
        /// </summary>
        [Test]
        public void LeftEndPosition_Test()
        {
            // Arrange.
            _region.Arc = 180;
            _region.Radius = 1;
            _region.Angle = 0;
            
            // Act.
            Vector3 position = _region.LeftEndPosition;
            
            // Assert.
            Assert.IsTrue(Vector3.left == position);
        }
        
        /// <summary>
        /// Tests the right end position to match.
        /// </summary>
        [Test]
        public void RightEndPosition_Test()
        {
            // Arrange.
            _region.Arc = 180;
            _region.Radius = 1;
            _region.Angle = 0;
            
            // Act.
            Vector3 position = _region.RightEndPosition;
            
            // Assert.
            Assert.IsTrue(Vector3.right == position);
        }
        
        /// <summary>
        /// Tests the left angle value.
        /// </summary>
        [Test]
        public void LeftAngle_Test()
        {
            // Arrange.
            _region.Arc = 180;
            _region.Radius = 1;
            _region.Angle = 0;
            
            // Act.
            float leftAngle = _region.LeftAngle;
            
            // Assert.
            Assert.AreEqual(-90, leftAngle);
        }
        
        /// <summary>
        /// Tests the right angle value.
        /// </summary>
        [Test]
        public void RightAngle_Test()
        {
            // Arrange.
            _region.Arc = 180;
            _region.Radius = 1;
            _region.Angle = 0;
            
            // Act.
            float rightAngle = _region.RightAngle;
            
            // Assert.
            Assert.AreEqual(90, rightAngle);
        }
    }
}