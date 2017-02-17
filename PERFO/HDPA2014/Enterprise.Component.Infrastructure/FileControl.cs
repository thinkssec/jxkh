using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Enterprise.Component.Infrastructure
{

    /**/
    /// <summary>
    /// FileControl 的摘要说明
    /// </summary>
    public sealed class FileControl
    {

        public FileControl()
        {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        }


        /// <summary>
        /// 根据URL路径获取指定文件的物理路径
        /// </summary>
        /// <param name="webPath">URL</param>
        /// <returns></returns>
        public static string GetFilePhysicsPath(string webPath)
        {
            string fileRealPath = System.Web.HttpContext.Current.Server.MapPath(webPath);
            FileInfo fInfo = new FileInfo(fileRealPath);
            if (fInfo.Exists)
            {
                return fileRealPath;
            }
            return "";
        }

        /// <summary>
        /// 判断是否存在指定文件
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="FileName"></param>
        public static void IsCreateFile(string Path)
        {
            if (Directory.Exists(Path))
            { }
            else
            {
                FileInfo CreateFile = new FileInfo(Path); //创建文件 
                if (!CreateFile.Exists)
                {
                    FileStream FS = CreateFile.Create();
                    FS.Close();
                }
            }
        }
   

        /// <summary> 
        /// 在根目录下创建文件夹 
        /// </summary> 
        /// <param name="FolderPath">要创建的文件路径</param> 
        public static void CreateFolder(string FolderPathName)
        {
            if (FolderPathName.Trim().Length > 0)
            {
                try
                {
                    //string CreatePath = System.Web.HttpContext.Current.Server.MapPath("/" + FolderPathName).ToString();
                    if (!Directory.Exists(FolderPathName))
                    {
                        Directory.CreateDirectory(FolderPathName);
                    }
                }
                catch
                {
                   throw;
                }
            }
        }


        /// <summary> 
        /// 删除一个文件夹下面的字文件夹和文件 
        /// </summary> 
        /// <param name="FolderPathName"></param> 
        public static void DeleteChildFolder(string FolderPathName)
        {
            if (FolderPathName.Trim().Length > 0)
            {
                try
                {
                    //string CreatePath = System.Web.HttpContext.Current.Server.MapPath(FolderPathName).ToString();
                    if (Directory.Exists(FolderPathName))
                    {
                        Directory.Delete(FolderPathName, true);
                    }
                }
                catch
                {
                   throw;
                }
            }
        }


        /// <summary> 
        /// 删除一个文件 
        /// </summary> 
        /// <param name="FilePathName"></param> 
        public static void DeleteFile(string FilePathName)
        {
            try
            {
                FileInfo DeleFile = new FileInfo(FilePathName);
                DeleFile.Delete();

            }
            catch
            {
            }
        }

        /// <summary>
        /// 建立一个文件
        /// </summary>
        /// <param name="FilePathName"></param>
        public static void CreateFile(string FilePathName)
        {
            try
            {
                //创建文件夹 
                string[] strPath = FilePathName.Split('/');
                CreateFolder(FilePathName.Replace("/" + strPath[strPath.Length - 1].ToString(), "")); //创建文件夹 
                FileInfo CreateFile = new FileInfo(FilePathName); //创建文件 
                if (!CreateFile.Exists)
                {
                   FileStream FS = CreateFile.Create();
                   FS.Close();
                }
            }
            catch
            {
            }
        }


        /// <summary> 
        /// 删除整个文件夹及其字文件夹和文件 
        /// </summary> 
        /// <param name="FolderPathName"></param> 
        public static void DeleParentFolder(string FolderPathName)
        {
            try
            {
                DirectoryInfo DelFolder = new DirectoryInfo(FolderPathName);
                if (DelFolder.Exists)
                {
                    DelFolder.Delete();
                }
            }
            catch
            {
            }
        }


        /// <summary> 
        /// 在文件里追加内容 
        /// </summary> 
        /// <param name="FilePathName"></param> 
        public static void ReWriteReadinnerText(string FilePathName, string WriteWord)
        {
            try
            {
                //建立文件夹和文件 
                //CreateFolder(FilePathName); 
                CreateFile(FilePathName);
                //得到原来文件的内容 
                FileStream FileRead = new FileStream(FilePathName, FileMode.Open, FileAccess.ReadWrite);
                StreamReader FileReadWord = new StreamReader(FileRead, System.Text.Encoding.Default);
                string OldString = FileReadWord.ReadToEnd().ToString();
                OldString = OldString + WriteWord;
                //把新的内容重新写入 
                StreamWriter FileWrite = new StreamWriter(FileRead, System.Text.Encoding.Default);
                FileWrite.Write(WriteWord);
                //关闭 
                FileWrite.Close();
                FileReadWord.Close();
                FileRead.Close();
            }
            catch
            {
              // throw; 
            }
        }


        /// <summary> 
        /// 读取文件里内容 
        /// </summary> 
        /// <param name="FilePathName"></param> 
        public static string ReaderFileData(string FilePathName)
        {
            try
            {
                FileStream FileRead = new FileStream(FilePathName, FileMode.Open, FileAccess.Read);
                StreamReader FileReadWord = new StreamReader(FileRead, System.Text.Encoding.Default);
                string TxtString = FileReadWord.ReadToEnd().ToString();
                //关闭 
                FileReadWord.Close();
                FileRead.Close();
                return TxtString;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static bool ChkUrlFileIsExists(string filePathName)
        {
            bool isExists = false;
            try
            {
                string filePN = System.Web.HttpContext.Current.Server.MapPath(filePathName).ToString();
                FileInfo fInfo = new FileInfo(filePN);
                isExists = fInfo.Exists;
            }
            catch
            {
                isExists = false;
            }
            return isExists;
        }


        /// <summary> 
        /// 读取文件夹的文件 
        /// </summary> 
        /// <param name="FilePathName"></param> 
        /// <returns></returns> 
        public static DirectoryInfo checkValidSessionPath(string FilePathName)
        {
            try
            {
                DirectoryInfo MainDir = new DirectoryInfo(FilePathName);
                return MainDir;
            }
            catch
            {
                  throw;
            }
        }


      /// <summary> 
      /// 拷贝目录里的文件 
      /// </summary> 
      /// <param name=\"sourceFilePath\">源文件目录</param> 
      /// <param name=\"destFilePath\">目地文件目录</param> 
        public static void CopyDirs(string sourceFilePath, string destFilePath) 
      { 
           if (Directory.Exists(sourceFilePath))  
           { 
                // 检查目标目录是否以目录分割字符结束如果不是则添加 
                if (destFilePath[destFilePath.Length - 1] != Path.DirectorySeparatorChar) 
                   destFilePath += Path.DirectorySeparatorChar; 

                // 判断目标目录是否存在如果不存在则新建 
                // 得到源目录的文件列表,该里面是包含文件以及目录路径的一个数组 
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法 
                // string[] fileList = Directory.GetFiles(sourceFilePath);    
                if (!Directory.Exists(destFilePath))  
                   Directory.CreateDirectory(destFilePath); 

                string[] fileList = Directory.GetFileSystemEntries(sourceFilePath); 

                // 遍历所有的文件和目录 
                foreach (string file in fileList) 
                { 
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件，否则直接Copy文件 
                    if (Directory.Exists(file)) {
                        CopyDirs(file, destFilePath + Path.GetFileName(file)); 
                    }
                   else {
                        System.IO.File.Copy(file, destFilePath + Path.GetFileName(file), true); 
                    }
                } 
           } 
      }


        /// <summary>
        /// 拷贝文件操行
        /// </summary>
        /// <param name="sourFile">源文件路径+文件名</param>
        /// <param name="destFile">目标文件路径+文件名</param>
        public static void CopyFile(string sourFile, string destFile)
        {
            if (File.Exists(sourFile))
            {
                try
                {
                    File.Copy(sourFile, destFile, true);
                }
                catch
                {
                }
            }
        }

        /*
        // 遍历所有的文件和目录 
        foreach (string file in fileList) 
        { 
         // 先当作目录处理如果存在这个目录就递归Move该目录下面的文件，否则直接Move文件 
         string destFileName = destFilePath + file.Remove(0, file.LastIndexOf(\"\\\\\") + 1); 
         File.Move(file, destFileName); [Page]
        } 
        */


        /// <summary>
        /// 获取目录下所有文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static IEnumerable<System.IO.FileInfo> GetFiles(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                throw new System.IO.DirectoryNotFoundException();
            }
            else
            {
                string[] fileNames = null;
                List<System.IO.FileInfo> files = new List<System.IO.FileInfo>();
                fileNames = System.IO.Directory.GetFiles(path, "*.*", System.IO.SearchOption.AllDirectories);
                foreach (string name in fileNames)
                {
                    files.Add(new System.IO.FileInfo(name));
                }
                return files;
            }
        }

    }

}