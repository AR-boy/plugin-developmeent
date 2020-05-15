using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using UnityEngine;
using OpenCVInterop;
using OpenCVInterop.Marshallers;


namespace OpenCVInterop.Utilities
{
    [Serializable]
    public struct CameraCalibSerializable {
        public double[][] distortionCoefficients;
        public double[][] cameraMatrix;
        public double reProjectionError;
        public CameraCalibSerializable(double[][] distortionCoefficients, double[][] cameraMatrix, double reProjectionError) {
            this.distortionCoefficients = distortionCoefficients;
            this.cameraMatrix = cameraMatrix;
            this.reProjectionError = reProjectionError;
        }
    }
    public struct CharucoBoardParameters {
        public int squaresH;
        public int squaresW;
        public float squareLength;
        public float markerLength;
        public CharucoBoardParameters(int squaresH, int squaresW, float squareLength, float markerLength)
        {
            this.squaresH = squaresH;
            this.squaresW = squaresW;
            this.squareLength = squareLength;
            this.markerLength = markerLength;
        }
    }

    public struct ArucoBoardParameters {
        public int squaresH;
        public int squaresW;
        public float markerLength;
        public float markerSeperation;
        public ArucoBoardParameters(int squaresH, int squaresW, float markerLength, float markerSeperation)
        {
            this.squaresH = squaresH;
            this.squaresW = squaresW;
            this.markerLength = markerLength;
            this.markerSeperation = markerSeperation;
        }
    }
    

    public static class Utilities
    {
        private static string CAMERA_CALIBRATION_FILE = "CameraCalibParams.xml";
        public static void SaveCameraCalibrationParams(CameraCalibSerializable cameraCalibData) {
           if(File.Exists(Path.Combine(Application.persistentDataPath, CAMERA_CALIBRATION_FILE)))
            {
                File.Delete(Path.Combine(Application.persistentDataPath, CAMERA_CALIBRATION_FILE));
            }
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer serialiser = new XmlSerializer(typeof(CameraCalibSerializable));
            using (MemoryStream stream = new MemoryStream())
            {
                serialiser.Serialize(stream, cameraCalibData);
                stream.Position = 0;
                xmlDoc.Load(stream);
                xmlDoc.Save(Path.Combine(Application.persistentDataPath, CAMERA_CALIBRATION_FILE));
            }
            CameraCalibSerializable calibData =  LoadCameraCalibrationParams();
        }
        public static CameraCalibSerializable LoadCameraCalibrationParams() {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(Application.persistentDataPath, CAMERA_CALIBRATION_FILE));
            string xmlString = xmlDoc.OuterXml;

            CameraCalibSerializable calibData;
            using (StringReader read = new StringReader(xmlString))
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(CameraCalibSerializable));
                using (XmlReader reader = new XmlTextReader(read))
                {
                    calibData = (CameraCalibSerializable) serialiser.Deserialize(reader);
                }
            }
            return calibData;
        }

        // calculate scale
        public static float CalculateScale(Vec3d tvec, float markerLength, float originalScale)
        {
            float scale = originalScale;
            if(tvec.z !=0)
            {
                scale = (float) tvec.z / (markerLength * 10);
                scale = originalScale / scale;
            }
            return scale;
        }
        // convert Euler angles
        public static Quaternion CalculateEulerAngleRotation(double[][] eulerAngles, Quaternion originalRotation)
        {
        
            Quaternion newRotation = originalRotation;
            if( eulerAngles[0][0] != 0 ||
                eulerAngles[0][1] != 0 ||
                eulerAngles[0][2] != 0  )
            {
                Vec3d EulerAnglesDeg = new Vec3d(
                    Mathf.Rad2Deg * eulerAngles[0][0],
                    Mathf.Rad2Deg * eulerAngles[0][1],
                    Mathf.Rad2Deg * eulerAngles[0][2]
                );
                
                newRotation =  Quaternion.Euler(
                    new Vector3(
                        (float) EulerAnglesDeg.x, 
                        (float) EulerAnglesDeg.z, 
                        -(float) EulerAnglesDeg.y
                    )
                );
            }

            return newRotation;
        }

        // calculate average position
        public static Vector3 CalculateBoardAveragePosition(Vector3 currentPosition, List<List<Vector2>> markers)
        {
            float sum_x = 0;
            float sum_y = 0;
            int num_of_corners = 0;

            for(int i = 0; i < markers.Count; i++)
            {
                for(int j = 0; j < markers[i].Count; j++)
                {
                    sum_x += markers[i][j].x;
                    sum_y += markers[i][j].y;
                    num_of_corners++;
                }
            }

            return new Vector3(-sum_x / num_of_corners, -sum_y / num_of_corners, currentPosition.z);
        }

    }
}