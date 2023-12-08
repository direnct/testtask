using System;
using System.IO;
using System.Threading;

class FolderSync
{
    static void Main()
    {
        Console.WriteLine("Folder Sync");

        Console.Write("Enter the source folder path: ");
        string sourceFolderPath = Console.ReadLine();

        Console.Write("Enter the replica folder path: ");
        string replicaFolderPath = Console.ReadLine();

        Console.Write("Enter the synchronization period (Sec): ");
        int syncPeriodInSeconds = int.Parse(Console.ReadLine());

        Console.Write("Enter the log file location: ");
        string destinationLogFile = Console.ReadLine();

        string logFilePath = Path.Combine(destinationLogFile, "sync_log.txt");

        Console.WriteLine("Synchronization started. To stop press ESC");


        while (true)
        {
            SyncFolderContents(sourceFolderPath, replicaFolderPath, logFilePath);

            // Option to stop the program
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Exiting the program.");
                break;
            }
            // For the sync period // 
            Thread.Sleep(syncPeriodInSeconds * 1000);
        }
    }




    private static void SyncFolderContents(string sourceFolder, string replicaFolder, string logFilePath)
    {
   
        // First delete the contents of the replica folder
        DeleteFolderContents(replicaFolder,logFilePath);

        // Copy files
        foreach (var file in Directory.GetFiles(sourceFolder))
        {
            string replicaFile = Path.Combine(replicaFolder, Path.GetFileName(file));
            File.Copy(file, replicaFile);

            Log($"{DateTime.Now} - Copied: {file} to {replicaFile}", logFilePath);
            Console.WriteLine("Copied:" + file + " to " + replicaFile );
        }

        // Copy subfolders
        foreach (var subfolder in Directory.GetDirectories(sourceFolder))
        {
            string replicaSubfolder = Path.Combine(replicaFolder, Path.GetFileName(subfolder));
            CopyFolder(subfolder, replicaSubfolder,logFilePath);
        }

   
       
}



    private static void DeleteFolderContents(string folderPath, string logFilePath)
    {
       
            foreach (var file in Directory.GetFiles(folderPath))
            {
                File.Delete(file);

                Log($"{DateTime.Now} - Deleted : {file} from {folderPath}", logFilePath);
                Console.WriteLine("Deleted:" + file + " from " + folderPath );
            }

            foreach (var subfolder in Directory.GetDirectories(folderPath))
            {
                Directory.Delete(subfolder, true);

                Log($"{DateTime.Now} - Deleted : {subfolder} from {folderPath}", logFilePath);
                Console.WriteLine("Deleted:" + subfolder + " from " + folderPath );

            }
    }
     
    

    private static void Log(string message, string logFilePath)
    {
            //log operations
            File.AppendAllText(logFilePath, message + Environment.NewLine);
              
    }

    private static void CopyFolder(string sourceFolder, string replicaFolder, string logFilePath)
    {

        // Copy files
        foreach (var file in Directory.GetFiles(sourceFolder))
        {
            string replicaFile = Path.Combine(replicaFolder, Path.GetFileName(file));
            File.Copy(file, replicaFile);

            Log($"{DateTime.Now} - Copied: {file} to {replicaFile}", logFilePath);
            Console.WriteLine("Copied:" + file + " to " + replicaFile );
        }

        // Copy subfolders
        foreach (var subfolder in Directory.GetDirectories(sourceFolder))
        {
            string replicaSubfolder = Path.Combine(replicaFolder, Path.GetFileName(subfolder));
            CopyFolder(subfolder, replicaSubfolder, logFilePath);
        }
    }
}
