using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Geometry;

namespace RhinoPluginTests
{
    /// <summary>
    /// Example test methods that invoke RhinoCommon
    /// </summary>
    [TestClass]
    public class ExampleTests
    {
        /// <summary>
        /// Transform a brep using a translation
        /// </summary>
        [TestMethod]
        public void Brep_TranslationA()
        {
            // Arrange
            var bb = new BoundingBox(new Point3d(0, 0, 0), new Point3d(100, 100, 100));
            var brep = bb.ToBrep();
            var t = Transform.Translation(new Vector3d(30, 40, 50));

            // Act
            brep.Transform(t);

            // Assert
            Assert.AreEqual(brep.GetBoundingBox(true).Center, new Point3d(80, 90, 100));
        }

        [TestMethod]
        public void TestLineComponent()
        {
            var ptA = new Point3d(10.0, 10.0, 10.0);
            var ptB = new Point3d(140.0, 110.0, -100.0);
            var lineBaseCase = new Line(ptA, ptB);

            // Results of any FindComponent() just appear to be null
            var groupPointsInfo = Rhino.NodeInCode.Components.FindComponent("PointGroups"); // this definitely exists
            var lineInfo = Rhino.NodeInCode.Components.FindComponent("Line");
            var lineFunction = (System.Func<object, object, object>)lineInfo.Delegate; // Input oject and output object
            var lineResults = (IList<object>)lineFunction(ptA, ptB);
            var lineTestCase = lineResults[0] as Line?;

            Assert.AreEqual(lineBaseCase, lineTestCase); // Check plugin's line matches the RhinoCommon line
        }

        /// <summary>
        /// Intersect sphere with a plane to generate a circle
        /// </summary>
        [TestMethod]
        public void Brep_Intersection()
        {
            // Arrange
            var radius = 4.0;
            var brep = Brep.CreateFromSphere(new Sphere(new Point3d(), radius));
            var cuttingPlane = Plane.WorldXY;

            // Act
            Rhino.Geometry.Intersect.Intersection.BrepPlane(brep, cuttingPlane, 0.001, out var curves, out var points);

            // Assert
            Assert.AreEqual(1, curves.Length, "Wrong curve count");
            Assert.AreEqual(2 * Math.PI * radius, curves[0].GetLength(), "Wrong curve length");
        }
    }
}
