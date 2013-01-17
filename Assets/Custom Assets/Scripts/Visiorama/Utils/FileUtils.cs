using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Text;

using UnityEngine;

namespace Visiorama
{
	namespace Utils
	{
		public class FileUtils
		{
			public static bool Exists (string filePath, bool isRelativePath)
			{
				filePath = isRelativePath ? Application.persistentDataPath + "/" + filePath : filePath;
				
				return File.Exists (filePath);
			}

			/* 	
			  	Tries to open a file, with a user defined number of attempt and Sleep delay between attempts.
				@param filePath The full file path to be opened
				@param fileMode Required file mode enum value
				@param fileAccess Required file access enum value
				@param fileShare Required file share enum value
				@param maximumAttempts The total number of attempts to make (multiply by attemptWaitMS for the maximum time the function with Try opening the file)
				@param attemptWaitMS The delay in Milliseconds between each attempt.
				@return A valid FileStream object for the opened file, or null if the File could not be opened after the required attempts</returns>
			*/
			public static FileStream TryOpen (string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, int maximumAttempts, int attemptWaitMS, bool isRelativePath)
			{
				
				filePath = isRelativePath ? Application.persistentDataPath + "/" + filePath : filePath;
				
				FileStream fs = null;
				int attempts = 0;
				
				while (true) {
					try {
						fs = File.Open (filePath, fileMode, fileAccess, fileShare);
						//If we get here, the File.Open succeeded, so break out of the loop and return the FileStream
						break;
					} catch (IOException ioEx) {
						// IOException is thrown if the file is in use by another process.
						// Check the numbere of attempts to ensure no infinite loop
						if (++attempts > maximumAttempts) {
							fs = null;
							break;
						} else {
							// Sleep before making another attempt
							Thread.Sleep (attemptWaitMS);
						}
					}
				}
				// Return the filestream, may be valid or null
				return fs;
			}

			/*
				This method read a file and return it in bytes.
				@param filename The name of the file to be loaded 
				@param isRelativePath If true, the given path is relative of the application path. Or else, the path is absolute.
				@return Whole file in bytes.
			*/
			public static byte[] LoadFile (string filepath, bool isRelativePath)
			{
				
				//Resolve path
				filepath = isRelativePath ? Application.persistentDataPath + "/" + filepath : filepath;
				
				//Verify if the file exists
				if (!File.Exists (filepath)) {
					throw new IOException ("The file \"" + filepath + "\" does not exists! ");
				}
				
				//Open file
				FileStream fileReader = File.Open (filepath, FileMode.Open);
				
				//read and return file in a byte array
				byte[] data = new byte[fileReader.Length];
				fileReader.Read (data, 0, data.Length);
				fileReader.Flush ();
				fileReader.Close ();
				
				return data;
			}

			/*
				Create a file and write its on the file system. The method try to create recusively the directory structure to the file.
				@param filename The name of the file to be writed. 
				@param isRelativePath If true, the given path is relative of the application path. Or else, the path is absolute.
				@return void
			*/
			public static void WriteFile (string filepath, byte[] data, bool isRelativePath)
			{
				
				filepath = isRelativePath ? Application.persistentDataPath + '/' + filepath : filepath;
				
				if (!Directory.Exists (filepath.Substring (0, filepath.LastIndexOf ('/')))) {
					Directory.CreateDirectory (filepath.Substring (0, filepath.LastIndexOf ('/')));
					Debug.LogWarning ("Directory was created on: " + filepath.Substring (0, filepath.LastIndexOf ('/')));
				}
				
				FileStream fileWriter;
				
				if (File.Exists (filepath)) {
					Debug.Log ("File exists. The file will be ovewritten");
					fileWriter = File.Open(filepath, FileMode.Create);
				} else {
					Debug.Log ("File does not exists.");
					fileWriter = File.Create (filepath);
				}
				
				fileWriter.Write (data, 0, data.Length);
				fileWriter.Flush ();
				fileWriter.Close ();
			}

			/*
				Delete files on a selected directory.
				@param directory The directory where are the files to be deleted. 
				@param filenames The name of the files to be deleted.
				@param except If true, delete all files except the files in the parameter "filenames". If false delete all files int the parameter "filenames"
				@param relativeDirectory If true, the directory is relative of the application. Or else, the path of the directory is absolute.
				@return void
			*/
			public static void DeleteFiles (string directory, string[] filenames, bool except, bool relativeDirectory)
			{
				
				directory = relativeDirectory ? Application.persistentDataPath + '/' + directory : directory;
				
				List<string> filesToDelete = new List<string> ();
				
				if (except) {
					
					string[] filesInDirectory = Directory.GetFiles (directory);
					List<string> lFilenames = new List<string> (filenames);
					
					for (int i = 0; i < lFilenames.Count; ++i) {
						lFilenames[i] = directory + lFilenames[i];
					}
					
					for (int i = 0; i < filesInDirectory.Length; ++i) {
						//If the lFilenames list doesn't contains the file, delete the file. 
						if (!lFilenames.Contains (filesInDirectory[i])) {
							filesToDelete.Add (filesInDirectory[i]);
						}
					}
				} else {
					for (int i = 0; i < filenames.Length; ++i) {
						filesToDelete.Add (directory + '/' + filenames[i]);
					}
				}
				
				foreach (string file in filesToDelete) {
					if (File.Exists (file)) {
						File.Delete (file);
					} else {
						throw new IOException ("The file \"" + file + "\" does not exists! ");
					}
				}
			}
			
			#region encrypt
			static public void WriteEncryptedFile(string filepath, byte[] data, bool isRelativePath){
				WriteFile(filepath,Visiorama.Utils.SecurityUtils.Encrypt(data),isRelativePath);
			}
			
			#endregion
			
			#region decrypt
			public static byte[] LoadEncryptedFile (string filepath, bool isRelativePath){
				return Visiorama.Utils.SecurityUtils.Decrypt(LoadFile(filepath,isRelativePath));
			}
			
			#endregion
			
		}
	}
}