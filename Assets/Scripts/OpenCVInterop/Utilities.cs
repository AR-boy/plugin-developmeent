using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using UnityEngine;
using OpenCVInterop;


namespace OpenCVInterop.Utilities
{
    [Serializable]
    public struct CameraCalibSerializable {
        public double[][] distortionCoefficients;
        public double[][] cameraMatrix;
        public CameraCalibSerializable(double[][] distortionCoefficientsParam, double[][] cameraMatrixParam) {
            distortionCoefficients = distortionCoefficientsParam;
            cameraMatrix = cameraMatrixParam;
        }
    }

    public static class Utilities
    {
        private static string CAMERA_CALIBRATION_FILE = "CameraCalibParams.xml";
        public static void SaveCameraCalibrationParams(CameraCalibSerializable cameraCalibData) {
           if(File.Exists(CAMERA_CALIBRATION_FILE))
            {
                File.Delete(CAMERA_CALIBRATION_FILE);
            }
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer serialiser = new XmlSerializer(typeof(CameraCalibSerializable));
            using (MemoryStream stream = new MemoryStream())
            {
                serialiser.Serialize(stream, cameraCalibData);
                stream.Position = 0;
                xmlDoc.Load(stream);
                xmlDoc.Save(CAMERA_CALIBRATION_FILE);
            }
            CameraCalibSerializable calibData =  LoadCameraCalibrationParams();
            string shit = "shit";
        }
        public static CameraCalibSerializable LoadCameraCalibrationParams() {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(CAMERA_CALIBRATION_FILE);
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

    }
}