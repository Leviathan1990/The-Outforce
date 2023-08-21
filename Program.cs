/*
 * =======================================================
 * *.box archive extractor tool V2.0.
 * Designed for the game "The Outforce"
 * See changelog for detailed info...
 * Full support for *.box archive.
 * Developed by: Krisztian Kispeti.
 * =======================================================
 * e-Contact    : 
 * Discord      : https://discord.gg/7RbzqN9
 * Moddb        : https://www.moddb.com/games/the-outforce
 * Archive org  : https://archive.org/details/@winters1990
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
// *. bor archive structure
public struct BoxItem
{
    public string Filename;
    public uint Offset; // uint32_t 
    public uint Size; // uint32_t
}
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("*.box archive extractor tool v2.0       ");
        Console.ResetColor();
        Console.WriteLine("Designed for the game The Outforce      ");
        Console.WriteLine("https://www.moddb.com/games/the-outforce");
        Console.WriteLine("Discord:https://discord.gg/7RbzqN9      ");
        Console.WriteLine("By: Krisztian Kispeti                   ");
        Console.WriteLine("========================================\n");

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("-Enter the .box file name:           -\t");
        Console.WriteLine("----------------------------------------");
        string boxFileName = Console.ReadLine();

        if (!File.Exists(boxFileName))
        {
            Console.WriteLine("Error! Can't read input *.box file!.");
            return;
        }

        List<BoxItem> toc;
        uint directoryOffset;

        try
        {
            using (FileStream fs = new FileStream(boxFileName, FileMode.Open))
            {
                toc = ExtractFilesFromBox(fs, out directoryOffset);
            }

            // Print the number of files and total file size
            Console.WriteLine($"Number of files in the .box file: {toc.Count}"); // The Num.of files can be found in the .box archive
            long totalSize = 0;
            foreach (var item in toc)
            {
                totalSize += item.Size;
            }
            Console.WriteLine($"Total size of the .box file: {totalSize} bytes ({totalSize / 1024.0 / 1024.0} MB)\n");

            // List the files in the .box file with numbering
            Console.WriteLine("--------------------------");
            Console.WriteLine("Contents of the .box file:");
            Console.WriteLine("--------------------------");
            for (int i = 0; i < toc.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {toc[i].Filename}");
            }

            Console.WriteLine("\nEnter 'extract' to extract the files: ");
            string userInput = Console.ReadLine();

            if (userInput != null && userInput.Trim().ToLower() == "extract")
            {
                try
                {
                    // Extract the files
                    ExtractFiles(boxFileName, toc);
                    Console.WriteLine("\nFiles extracted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nExtraction canceled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    public static List<BoxItem> ExtractFilesFromBox(FileStream fs, out uint directoryOffset)
    {
        List<BoxItem> toc = new List<BoxItem>();

        byte[] numFilesBuffer = new byte[4];
        fs.Seek(-4, SeekOrigin.End);
        fs.Read(numFilesBuffer, 0, 4);
        directoryOffset = BitConverter.ToUInt32(numFilesBuffer, 0);

        fs.Seek(directoryOffset, SeekOrigin.Begin);
        byte[] numFilesBytes = new byte[4];
        fs.Read(numFilesBytes, 0, 4);
        uint numFiles = BitConverter.ToUInt32(numFilesBytes, 0);

        using (BinaryReader reader = new BinaryReader(fs))
        {
            for (int i = 0; i < numFiles; i++)
            {
                BoxItem item = new BoxItem();
                item.Filename = ReadNullTerminatedString(reader);
                item.Offset = reader.ReadUInt32();
                item.Size = reader.ReadUInt32();
                toc.Add(item);
            }
        }
        return toc;
    }
    public static void ExtractFiles(string boxFileName, List<BoxItem> toc)
    {
        string outputDirectory = Path.GetFileNameWithoutExtension(boxFileName) + "";
        Directory.CreateDirectory(outputDirectory);

        using (FileStream fs = new FileStream(boxFileName, FileMode.Open))
        {
            BinaryReader reader = new BinaryReader(fs);

            foreach (var item in toc)
            {
                try
                {
                    fs.Seek(item.Offset, SeekOrigin.Begin);

                    byte[] fileData = reader.ReadBytes((int)item.Size);

                    // Build the output path based on the original folder structure
                    string outputPath = Path.Combine(outputDirectory, item.Filename.Replace('\\', Path.DirectorySeparatorChar));

                    // Ensure the directory structure exists
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                    File.WriteAllBytes(outputPath, fileData);
                    Console.WriteLine($"File '{item.Filename}' extracted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error extracting file '{item.Filename}': {ex.Message}");
                }
            }
        }
    }
    private static string ReadNullTerminatedString(BinaryReader reader)
    {
        List<byte> bytes = new List<byte>();
        byte b;

        while ((b = reader.ReadByte()) != 0)
        {
            bytes.Add(b);
        }

        return Encoding.ASCII.GetString(bytes.ToArray());
    }
}