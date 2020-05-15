using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using OpenCVInterop.Utilities;
using OpenCVInterop.Marshallers;

namespace Tests
{
    public class TestSuite
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestCalculateScaleWithValidZValue()
        {
            // initialised mock values
            Vec3d mockTvec = new Vec3d(
                -0.183446751848711,
                -0.24590448762994,
                0.725316990825065
            );
            float mockMarkerLength = 0.045f;
            float mockOriginalScale = 100f;
            // checked correct value for mock parameters
            float expectedScale = 62.0418434f;

            float actualScale = Utilities.CalculateScale(mockTvec, mockMarkerLength, mockOriginalScale);

            Assert.AreEqual(expectedScale, actualScale);
        }
        [Test]
        public void TestCalculateScaleWithInvalidZValue()
        {
            // initialised mock values
            Vec3d mockTvec = new Vec3d(
                -0.183446751848711,
                -0.24590448762994,
                0.0
            );
            float mockMarkerLength = 0.045f;
            float mockOriginalScale = 100f;
            // checked correct value for mock parameters
            float expectedScale = mockOriginalScale;

            float actualScale = Utilities.CalculateScale(mockTvec, mockMarkerLength, mockOriginalScale);

            Assert.AreEqual(expectedScale, actualScale);
        }

        [Test]
        public void TestCalculateEulerAngleRotationWithValidValues()
        {
            // initialised mock values
            double[][] mockEulerAngles = new double[1][] {
                new double[3] {
                    -0.0990780677802371, 1.63066957624119, -3.05544587755646
                }
            };
            Quaternion mockRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
            Quaternion actualRotation = Utilities.CalculateEulerAngleRotation(mockEulerAngles, mockRotation);

            Quaternion expectedRotation = new Quaternion(
                0.724925816f,
                -0.685700655f,
                -0.0652272254f,
                -0.00652711652f
            );
            
            Assert.AreEqual(expectedRotation, actualRotation);
        }

        [Test]
        public void TestCalculateEulerAngleRotationWithInvalidValues()
        {
            // initialised mock values
            double[][] mockEulerAngles = new double[1][] {
                new double[3] {
                    0.0,
                    1.63066957624119,
                    -3.05544587755646
                }
            };
            Quaternion mockRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
            Quaternion actualRotation = Utilities.CalculateEulerAngleRotation(mockEulerAngles, mockRotation);

            Quaternion expectedRotation = mockRotation;
            
            Assert.AreNotEqual(expectedRotation, actualRotation);
        }
        [Test]
        public void TestCalculateBoardAveragePositionWithValidValues()
        {
            // Use the Assert class to test conditions
            Vector3 mockCurrentPosition = new Vector3(0.0f, 0.0f, 100.0f);
            List<List<Vector2>> mockMarkers = new List<List<Vector2>>() {
                new List<Vector2>() {
                    new Vector2(862.6231f, 533.1071f),
                    new Vector2(858.2434f, 622.4136f),
                    new Vector2(768.9725f, 616.1558f),
                    new Vector2(773.0092f, 527.7619f),
                },
                new List<Vector2>() {
                    new Vector2(629.5861f, 517.6705f),
                    new Vector2(626.5369f, 604.6815f),
                    new Vector2(545.4857f, 597.8793f),
                    new Vector2(548.7595f, 512.6361f),
                },
                new List<Vector2>() {
                    new Vector2(749.5685f, 405.857f),
                    new Vector2(744.4148f, 494.7486f),
                    new Vector2(658.6698f, 489.5322f),
                    new Vector2(662.829f, 401.9781f),
                },
                new List<Vector2>() {
                    new Vector2(877.3463f, 290.22631f),
                    new Vector2(872.127f, 380.42f),
                    new Vector2(781.6138f, 376.3586f),
                    new Vector2(786.8241f, 286.6443f),
                }
            };

            Vector3 actualAveragePosition = Utilities.CalculateBoardAveragePosition(mockCurrentPosition, mockMarkers);
            Vector3 expectedAveragePosition = new Vector3(-734.163208f, -478.629456f, 100.0f);
            Assert.AreEqual(expectedAveragePosition, actualAveragePosition);
        }
        [Test]
        public void TestCalculateBoardAveragePositionWithInvalidValues()
        {
            // Use the Assert class to test conditions
            Vector3 mockCurrentPosition = new Vector3(0.0f, 0.0f, 100.0f);
            List<List<Vector2>> mockMarkers = new List<List<Vector2>>() {
                new List<Vector2>() {
                    new Vector2(862.6231f, 5234.1078f),
                    new Vector2(858.2434f, 622.4136f),
                    new Vector2(768.9725f, 616.1558f),
                    new Vector2(773.0092f, 527.7619f),
                },
                new List<Vector2>() {
                    new Vector2(629.5861f, 517.6705f),
                    new Vector2(626.5369f, 604.6815f),
                    new Vector2(545.4857f, 597.8793f),
                    new Vector2(548.7595f, 512.6361f),
                },
                new List<Vector2>() {
                    new Vector2(749.5685f, 405.857f),
                    new Vector2(744.4148f, 494.7486f),
                    new Vector2(658.6698f, 489.5322f),
                    new Vector2(662.829f, 401.9781f),
                },
                new List<Vector2>() {
                    new Vector2(877.3463f, 290.22631f),
                    new Vector2(872.127f, 380.42f),
                    new Vector2(781.6138f, 376.3586f),
                    new Vector2(786.8241f, 286.6443f),
                }
            };

            Vector3 actualAveragePosition = Utilities.CalculateBoardAveragePosition(mockCurrentPosition, mockMarkers);
            Vector3 expectedAveragePosition = new Vector3(-734.163208f, -478.629456f, 100.0f);
            Assert.AreNotEqual(expectedAveragePosition, actualAveragePosition);
        }
    }
}
