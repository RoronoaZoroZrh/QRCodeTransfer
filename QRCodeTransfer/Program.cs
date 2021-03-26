using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace QRCodeTransfer
{
    class Program
    {
        public static void Main(string[] args)
        {
            string sIFile = "";
            string sOFile = "";
            int nWidth = 0;
            for (int i = 0; i < args.Length; ++i)
            {
                if (String.Equals("-help", args[i].ToLower()) ||
                    String.Equals("-h", args[i].ToLower()))
                {
                    OutputMSG();
                    return;
                }

                if (String.Equals("-if", args[i].ToLower()))
                {
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        if (!File.Exists(args[i + 1]))
                        {
                            Console.WriteLine("input file don't exist");
                            return;
                        }
                        sIFile = args[i + 1].Trim();
                    }
                    else
                    {
                        OutputMSG();
                        return;
                    }
                }

                if (String.Equals("-of", args[i].ToLower()))
                {
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        sOFile = args[i + 1].Trim();
                    }
                    else
                    {
                        OutputMSG();
                        return;
                    }
                }

                if (String.Equals("-w", args[i].ToLower()))
                {
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        nWidth = int.Parse(args[i + 1].Trim());
                    }
                    else
                    {
                        OutputMSG();
                        return;
                    }
                }
            }

            if (File.Exists(sOFile))
            {
                File.SetAttributes(sOFile, FileAttributes.Normal);
                File.Delete(sOFile);
            }

            string sDir = Path.GetDirectoryName(sOFile);
            if (!String.IsNullOrEmpty(sDir) && !Directory.Exists(sDir))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sOFile));
            }

            Bitmap lBitmap = new Bitmap(nWidth, nWidth);
            int nTmpW = 0;
            int nTmpH = 0;
            int nTmpT = 0;
            using (StreamReader lReader = new StreamReader(sIFile))
            {
                string sTmp = "";
                while (!lReader.EndOfStream)
                {
                    sTmp = lReader.ReadLine();
                    for (int k = 0; k < sTmp.Length; ++k)
                    {
                        if (sTmp[k] == '0')
                        {
                            lBitmap.SetPixel(nTmpW, nTmpH, Color.White);
                            nTmpT++;
                            nTmpW = nTmpT / nWidth;
                            nTmpH = nTmpT % nWidth;
                        }
                        else if (sTmp[k] == '1')
                        {
                            lBitmap.SetPixel(nTmpW, nTmpH, Color.Black);
                            nTmpT++;
                            nTmpW = nTmpT / nWidth;
                            nTmpH = nTmpT % nWidth;
                        }
                    }
                }
                lBitmap.Save(sOFile, ImageFormat.Png);
            }
        }

        private static void OutputMSG()
        {
            Console.WriteLine("Binary To PNG : QRCodeTransfer -if 1.txt -of 1.png -w 25");
            Console.WriteLine("-if input file");
            Console.WriteLine("-of output file");
            Console.WriteLine("-w the width of png");
        }
    }
}