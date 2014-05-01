using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.Helpers
{
    /* vim: set expandtab shiftwidth=4 softtabstop=4 tabstop=4: */

    /**
     * RC4.NET 1.0
     *
     * RC4.NET is a petite library that allows you to use RC4
     * encryption easily in the .NET Platform. It's OO and can
     * produce outputs in binary and hex.
     *
     * (C) Copyright 2006 Mukul Sabharwal [http://mjsabby.com]
     *	   All Rights Reserved
     *
     * @link http://rc4dotnet.devhome.org
     * @author Mukul Sabharwal <mjsabby@gmail.com>
     * @version $Id: RC4.cs,v 1.0 2006/03/19 15:35:24 mukul Exp $
     * @copyright Copyright &copy; 2006 Mukul Sabharwal
     * @license http://www.gnu.org/copyleft/lesser.html
     * @package RC4.NET
     */

    /**
     * RC4 Class
     * @package RC4.NET
     */
    public class RC4
    { /* inherits Page for ASP.NET */
        /**
         * Get ASCII Integer Code
         *
         * @param char ch Character to get ASCII value for
         * @access private
         * @return int
         */
        private static int ord(char ch)
        {
            return (int)(Encoding.GetEncoding("utf-8").GetBytes(ch + "")[0]);
        }
        /**
         * Get character representation of ASCII Code
         *
         * @param int i ASCII code
         * @access private
         * @return char
         */
        private static char chr(int i)
        {
            byte[] bytes = new byte[1];
            bytes[0] = (byte)i;
            return Encoding.GetEncoding("utf-8").GetString(bytes, 0, 1)[0];
        }
        /**
         * Convert Hex to Binary (hex2bin)
         *
         * @param string packtype Rudimentary in this implementation
         * @param string datastring Hex to be packed into Binary
         * @access private
         * @return string
         */
        private static string pack(string packtype, string datastring)
        {
            int i, j, datalength, packsize;
            byte[] bytes;
            char[] hex;
            string tmp;

            datalength = datastring.Length;
            packsize = (datalength / 2) + (datalength % 2);
            bytes = new byte[packsize];
            hex = new char[2];

            for (i = j = 0; i < datalength; i += 2)
            {
                hex[0] = datastring[i];
                if (datalength - i == 1)
                    hex[1] = '0';
                else
                    hex[1] = datastring[i + 1];
                tmp = new string(hex, 0, 2);
                try { bytes[j++] = byte.Parse(tmp, System.Globalization.NumberStyles.HexNumber); }
                catch { } /* grin */
            }
            return Encoding.GetEncoding("utf-8").GetString(bytes, 0, bytes.Length);
        }
        /**
         * Convert Binary to Hex (bin2hex)
         *
         * @param string bindata Binary data
         * @access public
         * @return string
         */
        public static string bin2hex(string bindata)
        {
            int i;
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(bindata);
            string hexString = "";
            for (i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("x2");
            }
            return hexString;
        }
        /**
         * The symmetric encryption function
         *
         * @param string pwd Key to encrypt with (can be binary of hex)
         * @param string data Content to be encrypted
         * @param bool ispwdHex Key passed is in hexadecimal or not
         * @access public
         * @return string
         */
        public static string EncryptIt(string pwd, string data, bool ispwdHex, bool IsDecrypt)
        {
            int a, i, j, k, tmp, pwd_length, data_length;
            int[] key, box;
            byte[] cipher;
            //string cipher;

            byte[] input;

            if (IsDecrypt == false)
            {
                // 進行加密
                input = Encoding.UTF8.GetBytes(data);
            }
            else
            {
                //string kdata = data.Substring(0, data.Length / 2);
                //input = Encoding.GetEncoding(1252).GetBytes(kdata);
                // 進行解密
                // 該傳入字串為十六進位碼，要將其轉換成整數陣列，實際 input 大小為該字串的一半
                int len = data.Length / 2;
                input = new byte[len];
                string tmpHex = "";
                for (int si = 0; si < len; si++)
                {
                    tmpHex = data.Substring(si * 2, 2);
                    try
                    {
                        input[si] = byte.Parse(tmpHex, System.Globalization.NumberStyles.HexNumber);
                    }
                    catch
                    {
                        input[si] = 0;
                    }
                }
            }

            if (ispwdHex)
                pwd = pack("H*", pwd); // valid input, please!
            pwd_length = pwd.Length;
            data_length = input.Length;
            key = new int[256];
            box = new int[256];
            cipher = new byte[input.Length];
            //cipher = "";

            for (i = 0; i < 256; i++)
            {
                key[i] = ord(pwd[i % pwd_length]);
                box[i] = i;
            }

            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }

            for (a = j = i = 0; i < data_length; i++)
            {
                a = (a + 1) % 256;
                j = (j + box[a]) % 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)((input[i]) ^ k);
                //cipher += chr(ord(data[i]) ^ k);
            }
            if (IsDecrypt == false)
            {
                //return Encoding.GetEncoding(1252).GetString(cipher, 0, cipher.Length);
                // 進行加密
                string HexCipher = "";
                for (int si = 0; si < cipher.Length; si++)
                {
                    byte tmpByte = cipher[si];
                    int tmpInt = int.Parse(tmpByte.ToString());
                    string tmpHex = string.Format("{0:X}", tmpInt);
                    if (tmpHex.Length == 1)
                    {
                        tmpHex = "0" + tmpHex;
                    }
                    HexCipher += tmpHex;
                }
                return HexCipher;
            }
            else
            {
                // 進行解密
                return Encoding.UTF8.GetString(cipher, 0, cipher.Length);
            }
            //return cipher;
        }
        /**
         * Decryption, recall encryption
         *
         * @param string pwd Key to decrypt with (can be binary of hex)
         * @param string data Content to be decrypted
         * @param bool ispwdHex Key passed is in hexadecimal or not
         * @access public
         * @return string
         */
        public static string Decrypt(string pwd, string data, bool ispwdHex)
        {
            Base64Decoder myDecoder;
            data = EncryptIt(pwd, data, ispwdHex, true);
            myDecoder = new Base64Decoder(data);
            data = myDecoder.GetDecoded();

            return data;
        }

        public static string Encrypt(string pwd, string data, bool ispwdHex)
        {
            Base64Encoder myEncoder;
            myEncoder = new Base64Encoder(data);
            data = myEncoder.GetEncoded();

            return EncryptIt(pwd, data, ispwdHex, false);
        }
    }

    /// <summary>
    /// Summary description for Base64Encoder.
    /// </summary>
    public class Base64Encoder
    {
        byte[] source;
        int length, length2;
        int blockCount;
        int paddingCount;
        public Base64Encoder(string EncoderString)
        {
            byte[] input = Encoding.UTF8.GetBytes(EncoderString);
            source = input;
            length = input.Length;
            if ((length % 3) == 0)
            {
                paddingCount = 0;
                blockCount = length / 3;
            }
            else
            {
                paddingCount = 3 - (length % 3);//need to add padding
                blockCount = (length + paddingCount) / 3;
            }
            length2 = length + paddingCount;//or blockCount *3
        }

        public string GetEncoded()
        {
            byte[] source2;
            source2 = new byte[length2];
            //copy data over insert padding
            for (int x = 0; x < length2; x++)
            {
                if (x < length)
                {
                    source2[x] = source[x];
                }
                else
                {
                    source2[x] = 0;
                }
            }

            byte b1, b2, b3;
            byte temp, temp1, temp2, temp3, temp4;
            byte[] buffer = new byte[blockCount * 4];
            char[] result = new char[blockCount * 4];
            for (int x = 0; x < blockCount; x++)
            {
                b1 = source2[x * 3];
                b2 = source2[x * 3 + 1];
                b3 = source2[x * 3 + 2];

                temp1 = (byte)((b1 & 252) >> 2);//first

                temp = (byte)((b1 & 3) << 4);
                temp2 = (byte)((b2 & 240) >> 4);
                temp2 += temp; //second

                temp = (byte)((b2 & 15) << 2);
                temp3 = (byte)((b3 & 192) >> 6);
                temp3 += temp; //third

                temp4 = (byte)(b3 & 63); //fourth

                buffer[x * 4] = temp1;
                buffer[x * 4 + 1] = temp2;
                buffer[x * 4 + 2] = temp3;
                buffer[x * 4 + 3] = temp4;

            }

            for (int x = 0; x < blockCount * 4; x++)
            {
                result[x] = sixbit2char(buffer[x]);
            }

            //covert last "A"s to "=", based on paddingCount
            switch (paddingCount)
            {
                case 0: break;
                case 1: result[blockCount * 4 - 1] = '='; break;
                case 2: result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default: break;
            }

            string RetString = Encoding.UTF8.GetString(Encoding.GetEncoding("utf-8").GetBytes(result), 0, result.Length);
            return RetString;
        }

        private char sixbit2char(byte b)
        {
            char[] lookupTable = new char[64]
          {  'A','B','C','D','E','F','G','H','I','J','K','L','M',
            'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z',
            '0','1','2','3','4','5','6','7','8','9','+','/'};

            if ((b >= 0) && (b <= 63))
            {
                return lookupTable[(int)b];
            }
            else
            {
                //should not happen;
                return ' ';
            }
        }
    }

    /// <summary>
    /// Summary description for Base64Decoder.
    /// </summary>
    public class Base64Decoder
    {
        char[] source;
        int length, length2, length3;
        int blockCount;
        int paddingCount;
        public Base64Decoder(string EncoderString)
        {
            byte[] input = new byte[EncoderString.Length];
            input = Encoding.UTF8.GetBytes(EncoderString);
            char[] input1 = new char[input.Length];
            input1 = Encoding.GetEncoding("utf-8").GetChars(input);
            int temp = 0;
            source = input1;
            length = input.Length;

            //find how many padding are there
            for (int x = 0; x < 2; x++)
            {
                if (input[length - x - 1] == '=')
                    temp++;
            }
            paddingCount = temp;
            //calculate the blockCount;
            //assuming all whitespace and carriage returns/newline were removed.
            blockCount = length / 4;
            length2 = blockCount * 3;
        }

        public string GetDecoded()
        {
            byte[] buffer = new byte[length];//first conversion result
            byte[] buffer2 = new byte[length2];//decoded array with padding

            for (int x = 0; x < length; x++)
            {
                buffer[x] = char2sixbit(source[x]);
            }

            byte b, b1, b2, b3;
            byte temp1, temp2, temp3, temp4;

            for (int x = 0; x < blockCount; x++)
            {
                temp1 = buffer[x * 4];
                temp2 = buffer[x * 4 + 1];
                temp3 = buffer[x * 4 + 2];
                temp4 = buffer[x * 4 + 3];

                b = (byte)(temp1 << 2);
                b1 = (byte)((temp2 & 48) >> 4);
                b1 += b;

                b = (byte)((temp2 & 15) << 4);
                b2 = (byte)((temp3 & 60) >> 2);
                b2 += b;

                b = (byte)((temp3 & 3) << 6);
                b3 = temp4;
                b3 += b;

                buffer2[x * 3] = b1;
                buffer2[x * 3 + 1] = b2;
                buffer2[x * 3 + 2] = b3;
            }
            //remove paddings
            length3 = length2 - paddingCount;
            byte[] result = new byte[length3];

            for (int x = 0; x < length3; x++)
            {
                result[x] = buffer2[x];
            }

            string RetString = Encoding.UTF8.GetString(result, 0, result.Length);
            return RetString;
        }

        private byte char2sixbit(char c)
        {
            char[] lookupTable = new char[64]
          {  

    'A','B','C','D','E','F','G','H','I','J','K','L','M','N',
    'O','P','Q','R','S','T','U','V','W','X','Y', 'Z',
    'a','b','c','d','e','f','g','h','i','j','k','l','m','n',
    'o','p','q','r','s','t','u','v','w','x','y','z',
    '0','1','2','3','4','5','6','7','8','9','+','/'};
            if (c == '=')
                return 0;
            else
            {
                for (int x = 0; x < 64; x++)
                {
                    if (lookupTable[x] == c)
                        return (byte)x;
                }
                //should not reach here
                return 0;
            }

        }

    }
}
