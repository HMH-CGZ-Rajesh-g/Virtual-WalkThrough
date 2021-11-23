using System.IO;
using Amazon.S3.Model;
using UnityEngine;
using UnityEngine.UI;

namespace AWSSDK.Examples
{
    public class AppManager : MonoBehaviour
    {
        #region VARIABLES 

        [Header("Infos")]
        [SerializeField] private string S3BucketName;
        [Tooltip("FileName with Extesion. E.G file.txt")] [SerializeField] private string fileNameOnBucket;
        [Tooltip("Path and FileName with Extesion. E.G Documents/file.txt")] [SerializeField] private string pathFileUpload;

        [Header("Buttons")]
        [SerializeField] private Button buttonListBuckets;
        [SerializeField] private Button buttonListFilesBucket;
        [SerializeField] private Button buttonGetFileBucket;
        [SerializeField] private Button buttonUploadFileBucket;
        [SerializeField] private Button buttonDeleteFileBucket;
        [SerializeField] private Text resultTextOperation;

        [SerializeField] private RawImage displayTexture;
        #endregion

        #region METHODS MONOBEHAVIOUR

        void Start()
        {

            pathFileUpload = Application.persistentDataPath + "/Feedback.json";

            Debug.Log(Application.persistentDataPath);
            //            buttonListBuckets.onClick.AddListener(() => { ListBuckets(); });
            buttonListFilesBucket.onClick.AddListener(() => { ListObjectsBucket(); });
            buttonGetFileBucket.onClick.AddListener(() => { GetObjectBucket(); });
            buttonUploadFileBucket.onClick.AddListener(() => { UploadObjectForBucket(pathFileUpload, S3BucketName, fileNameOnBucket); });
            buttonDeleteFileBucket.onClick.AddListener(() => { DeleteObjectOnBucket(); });

            S3Manager.Instance.OnResultGetObject += GetObjectBucket;
        }

        #endregion

        #region METHODS CREATED

        private void ListBuckets()
        {
            //resultTextOperation.text = "Fetching all the Buckets";

            //S3Manager.Instance.ListBuckets((result, error) =>
            //{
            //    resultTextOperation.text += "\n";

            //    if (string.IsNullOrEmpty(error))
            //    {
            //        resultTextOperation.text += "Got Response \nPrinting now \n";

            //        result.Buckets.ForEach((bucket) =>
            //        {
            //            resultTextOperation.text += string.Format("bucket = {0}\n", bucket.BucketName);
            //        });
            //    }
            //    else
            //    {
            //        print("Get Error:: " + error);
            //        resultTextOperation.text += "Got Exception \n";
            //    }
            //});
        }

        private void ListObjectsBucket()
        {
            resultTextOperation.text = "Fetching all the Objects from " + S3BucketName;

            S3Manager.Instance.ListObjectsBucket(S3BucketName, (result, error) =>
            {
                resultTextOperation.text += "\n";
                if (string.IsNullOrEmpty(error))
                {
                    resultTextOperation.text += "Got Response \nPrinting now \n";
                    foreach (var file in result.S3Objects)
                    {
                        string nameObject = file.Key.Split('/')[file.Key.Split('/').Length - 1];

                        if (string.IsNullOrEmpty(file.Key.Split('/')[file.Key.Split('/').Length - 1]))
                            resultTextOperation.text += string.Format("Folder: {0}\n", file.Key);

                        resultTextOperation.text += string.Format("File: {0}\n", file.Key);
                    }
                }
                else
                {
                    print("Get Error:: " + error);
                    resultTextOperation.text += "Got Exception \n";
                }
            });
        }

        private void GetObjectBucket(GetObjectResponse resultFinal = null, string errorFinal = null)
        {
            resultTextOperation.text = string.Format("fetching {0} from bucket {1}", fileNameOnBucket, S3BucketName);

            if (errorFinal != null)
            {
                resultTextOperation.text += "\n";
                resultTextOperation.text += "Get Data Error";
                print("Get Error:: " + errorFinal);
                return;
            }

            S3Manager.Instance.GetObjectBucket(S3BucketName, fileNameOnBucket, (responseObj) =>
            {
                //byte[] data = null;
                var response = responseObj.Response;
                Stream input = response.ResponseStream;

                //To Load Json
                string data = null;
               // var response = responseObj.Response;
                if (response.ResponseStream != null)
                {
                    using (StreamReader reader = new StreamReader(response.ResponseStream))
                    {
                        data = reader.ReadToEnd();
                    }


                    resultTextOperation.text += "\n";
                    resultTextOperation.text += data;

                    WriteDataToFile(data);
                }

                //To load Image
                //if (response.ResponseStream != null)
                //{
                //    byte[] buffer = new byte[16 * 1024];
                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        int read;
                //        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                //        {
                //            ms.Write(buffer, 0, read);
                //        }
                //        data = ms.ToArray();
                //    }

                //    //Display Image
                //    displayTexture.texture = bytesToTexture2D(data);
                //}

                //to CheckFile
                //if (string.IsNullOrEmpty(error))
                //{
                //    resultTextOperation.text += "\nGet Data Complete.";
                //}
                //else
                //{
                //    resultTextOperation.text += "\n";
                //    resultTextOperation.text += "Get Data Error";
                //    print("Get Error:: " + error);
                //}
            });

        }

        public static void WriteDataToFile(string jsonString)
        {
            string path = Application.persistentDataPath + "/Feedback.json";
            Debug.Log("AssetPath:" + path);
            File.WriteAllText(path, jsonString);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public Texture2D bytesToTexture2D(byte[] imageBytes)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            return tex;
        }

        private void UploadObjectForBucket(string pathFile, string S3BucketName, string fileNameOnBucket)
        {
            resultTextOperation.text = "Retrieving the file";
            resultTextOperation.text += "\nCreating request object";
            resultTextOperation.text += "\nMaking HTTP post call";

            S3Manager.Instance.UploadObjectForBucket(pathFile, S3BucketName, fileNameOnBucket, (result, error) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    resultTextOperation.text += "\nUpload Success";
                }
                else
                {
                    resultTextOperation.text += "\nUpload Failed";
                    Debug.LogError("Get Error:: " + error);
                }
            });
        }

        private void DeleteObjectOnBucket()
        {
            resultTextOperation.text = string.Format("deleting {0} from bucket {1}\n", fileNameOnBucket, S3BucketName);

            S3Manager.Instance.DeleteObjectOnBucket(fileNameOnBucket, S3BucketName, (result, error) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    resultTextOperation.text += "\nFile Deleted Success";
                }
                else
                {
                    resultTextOperation.text += "\nFile Deleted Failed";
                    print("Get Error:: " + error);
                }
            });
        }

        #endregion
    }
}
