using UnityEngine;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if !UNITY_WEBPLAYER
using ICSharpCode;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
#endif                      

public static class ZipWrapper : System.Object
{

#if UNITY_WEBPLAYER
	public static void CompressDirectoryToFile (string sourcePath, string outputFile){}
	public static void CompressDirectoryToStream (this Stream target, string sourcePath,
                                         Func<string, bool> excludeFromCompression){}
	public static void DecompressToDirectory (this Stream source, string targetPath, string pwd,
                                             Func<string, bool> excludeFromDecompression){}
#else                      
	public static void CompressDirectoryToFile (string sourcePath, string outputFile)
	{
		FastZip fz = new FastZip ();
		fz.CreateZip (	outputFile,
						sourcePath,
						true,
						".*", "");
	}

	public static void CompressDirectoryToStream (this Stream target, string sourcePath,
                                         Func<string, bool> excludeFromCompression)
	{
		sourcePath = Path.GetFullPath (sourcePath);

		string parentDirectory = Path.GetDirectoryName (sourcePath);

		int trimOffset = (string.IsNullOrEmpty (parentDirectory)
                              ? Path.GetPathRoot (sourcePath).Length
                              : parentDirectory.Length);

		List<string> fileSystemEntries = new List<string> ();

		fileSystemEntries
            .AddRange (Directory.GetDirectories (sourcePath, "*", SearchOption.AllDirectories)
                          .Select (d => d + "\\"));

		fileSystemEntries
            .AddRange (Directory.GetFiles (sourcePath, "*", SearchOption.AllDirectories));

		using (ZipOutputStream compressor = new ZipOutputStream(target))
		{
			compressor.SetLevel (0);

			byte[] data = new byte[2048];

			foreach (string filePath in fileSystemEntries) {

//				Debug.Log ("filepath: " + filePath);

				if (excludeFromCompression (filePath)) {
					continue;
				}

				compressor.PutNextEntry (new ZipEntry (filePath.Substring (trimOffset)));

				if (filePath.EndsWith (@"\")) {
					continue;
				}

				using (FileStream input = File.OpenRead(filePath)) {
					int bytesRead;

					while ((bytesRead = input.Read(data, 0, data.Length)) > 0) {
						compressor.Write (data, 0, bytesRead);
					}
				}
				compressor.Close ();
			}
		}
	}

	public static void DecompressToDirectory (this Stream source, string targetPath, string pwd,
                                             Func<string, bool> excludeFromDecompression)
	{
		targetPath = Path.GetFullPath (targetPath);

		using (ZipInputStream decompressor = new ZipInputStream(source)) {
			if (!string.IsNullOrEmpty (pwd)) {
				decompressor.Password = pwd;
			}

			ZipEntry entry;

			while ((entry = decompressor.GetNextEntry()) != null) {
				if (excludeFromDecompression (entry.Name)) {
					continue;
				}

				string filePath = Path.Combine (targetPath, entry.Name);

				string directoryPath = Path.GetDirectoryName (filePath);


				if (!string.IsNullOrEmpty (directoryPath) && !Directory.Exists (directoryPath)) {
					Directory.CreateDirectory (directoryPath);
				}

				if (entry.IsDirectory) {
					continue;
				}

				byte[] data = new byte[2048];
				using (FileStream streamWriter = File.Create(filePath)) {
					int bytesRead;
					while ((bytesRead = decompressor.Read(data, 0, data.Length)) > 0) {
						streamWriter.Write (data, 0, bytesRead);
					}
				}
			}
		}
	}
	#endif
}
