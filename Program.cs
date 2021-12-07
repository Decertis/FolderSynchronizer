using System;
using System.IO;
using System.Timers;
namespace Tests
{
    static class Config
    {
        static string _source_path = @"\\OLGA\по работе";
        public static string SourcePath { get => _source_path; }

        static string _storage_path = @"D:\OLYA\COPY";
        public static string StoragePath { get => _storage_path; }

    }
    class Program
    {
        const double interval60Minutes = 1 * 60 * 1000; // milliseconds to one hour
        public static void Main(string[] args)
        {
            CountDown();
            Console.ReadKey();
        }
        static void CountDown()
        {
            Timer checkForTime = new Timer(interval60Minutes);
            checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            checkForTime.Enabled = true;
        }
        static void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Source folder exists : {Directory.Exists(Config.SourcePath)}");
            Console.WriteLine($"Storage folder exists : {Directory.Exists(Config.StoragePath)}");
            if (Directory.Exists(Config.SourcePath) && Directory.Exists(Config.StoragePath))
            {
                CopyAllFiles(Config.SourcePath, Config.StoragePath);
                CopyAllFolders(Config.SourcePath, Config.StoragePath);
                CountDown();
            }
        }
        public static void CopyAllFiles(string path, string save_path)
        {
            foreach (string file_name in Directory.GetFiles(path))
            {
                var splited = file_name.Split(@"\");
                int splited_length = splited.Length;
                File.Copy(file_name, save_path + $@"\{splited[splited_length - 1]}", true);
            }
        }
        public static void CopyAllFolders(string path, string save_path)
        {
            foreach (string dir_name in Directory.GetDirectories(path))
            {
                string new_dir_path = save_path + @"\" + dir_name.Split(path)[1];//Generating new folder path in Storage folder
                Directory.CreateDirectory(new_dir_path);
                CopyAllFiles(dir_name, new_dir_path);//Copyes all inner files
                CopyAllFolders(dir_name, new_dir_path);//Copyes all inner folders
            }
        }
    }

}
