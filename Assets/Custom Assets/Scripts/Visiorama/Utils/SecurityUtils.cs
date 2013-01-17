using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO; 

namespace Visiorama
{
	namespace Utils
	{
		public class SecurityUtils
		{
			private static byte[] Key = System.Text.ASCIIEncoding.ASCII.GetBytes ("The key to the algorithm");
			private static string Password = "my password";

			/// <summary>
			/// Encrypt a string.
			/// </summary>
			/// <param name="originalString">The original string.</param>
			/// <returns>The encrypted string.</returns>
			/// <exception cref="ArgumentNullException">This exception will be
			/// thrown when the original string is null or empty.</exception>
			public static byte[] Encrypt (byte[] clearBytes)
			{
				// Then, we need to turn the password into Key and IV 
				// We are using salt to make it harder to guess our key
				// using a dictionary attack - 
				// trying to guess a password by enumerating all possible words. 
				PasswordDeriveBytes pdb = new PasswordDeriveBytes (Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65,
				0x64, 0x65, 0x76 });
				
				// Now get the key/IV and do the encryption using the
				// function that accepts byte arrays. 
				// Using PasswordDeriveBytes object we are first getting
				// 32 bytes for the Key 
				// (the default Rijndael key length is 256bit = 32bytes)
				// and then 16 bytes for the IV. 
				// IV should always be the block size, which is by default
				// 16 bytes (128 bit) for Rijndael. 
				// If you are using DES/TripleDES/RC2 the block size is
				// 8 bytes and so should be the IV size. 
				// You can also read KeySize/BlockSize properties off
				// the algorithm to find out the sizes. 
				
				byte[] encryptedData = Encrypt (clearBytes, pdb.GetBytes (32), pdb.GetBytes (16));
				
				// Now we need to turn the resulting byte array into a string. 
				// A common mistake would be to use an Encoding class for that.
				//It does not work because not all byte values can be
				// represented by characters. 
				// We are going to be using Base64 encoding that is designed
				//exactly for what we are trying to do. 
				
				return encryptedData;
				
			}

			private static byte[] Encrypt (byte[] clearData, byte[] Key, byte[] IV)
			{
				
				MemoryStream ms = new MemoryStream ();
				
				Rijndael alg = Rijndael.Create ();
				
				alg.Key = Key;
				alg.IV = IV;
				
				CryptoStream cs = new CryptoStream (ms, alg.CreateEncryptor (), CryptoStreamMode.Write);
				
				cs.Write (clearData, 0, clearData.Length);
				
				cs.Close ();
				
				byte[] encryptedData = ms.ToArray ();
				
				return encryptedData;
			}


			/// <summary>
			/// Decrypt a crypted string.
			/// </summary>
			/// <param name="cryptedString">The crypted string.</param>
			/// <returns>The decrypted string.</returns>
			/// <exception cref="ArgumentNullException">This exception will be thrown
			/// when the crypted string is null or empty.</exception>


			// Decrypt bytes into bytes using a password
			public static byte[] Decrypt (byte[] cipherData)
			{
				
				PasswordDeriveBytes pdb = new PasswordDeriveBytes (Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65,
				0x64, 0x65, 0x76 });
				
				return Decrypt (cipherData, pdb.GetBytes (32), pdb.GetBytes (16));
			}

			public static byte[] Decrypt (byte[] cipherData, byte[] Key, byte[] IV)
			{
				
				// Create a MemoryStream that is going to accept the
				// decrypted bytes 
				MemoryStream ms = new MemoryStream ();
				
				// Create a symmetric algorithm. 
				// We are going to use Rijndael because it is strong and
				// available on all platforms. 
				// You can use other algorithms, to do so substitute the next
				// line with something like 
				//     TripleDES alg = TripleDES.Create(); 
				Rijndael alg = Rijndael.Create ();
				
				// Now set the key and the IV. 
				// We need the IV (Initialization Vector) because the algorithm
				// is operating in its default 
				// mode called CBC (Cipher Block Chaining). The IV is XORed with
				// the first block (8 byte) 
				// of the data after it is decrypted, and then each decrypted
				// block is XORed with the previous 
				// cipher block. This is done to make encryption more secure. 
				// There is also a mode called ECB which does not need an IV,
				// but it is much less secure. 
				alg.Key = Key;
				alg.IV = IV;
				
				// Create a CryptoStream through which we are going to be
				// pumping our data. 
				// CryptoStreamMode.Write means that we are going to be
				// writing data to the stream 
				// and the output will be written in the MemoryStream
				// we have provided. 
				CryptoStream cs = new CryptoStream (ms, alg.CreateDecryptor (), CryptoStreamMode.Write);
				
				// Write the data and make it do the decryption 
				cs.Write (cipherData, 0, cipherData.Length);
				
				// Close the crypto stream (or do FlushFinalBlock). 
				// This will tell it that we have done our decryption
				// and there is no more data coming in, 
				// and it is now a good time to remove the padding
				// and finalize the decryption process. 
				cs.Close ();
				
				// Now get the decrypted data from the MemoryStream. 
				// Some people make a mistake of using GetBuffer() here,
				// which is not the right way. 
				byte[] decryptedData = ms.ToArray ();
				
				return decryptedData;
			}
		}
	}
}
