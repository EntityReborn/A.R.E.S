﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace HOTSWAP
{
    class Program
    {
        public static AssetBundleFile DecompressBundle(string file, string decompFile)
        {
            var bun = new AssetBundleFile();

            var fs = (Stream)File.OpenRead(file);
            var reader = new AssetsFileReader(fs);

            bun.Read(reader, true);
            if (bun.bundleHeader6.GetCompressionType() != 0)
            {
                Stream nfs = decompFile switch
                {
                    null => new MemoryStream(),
                    _ => File.Open(decompFile, FileMode.Create, FileAccess.ReadWrite)
                };
                var writer = new AssetsFileWriter(nfs);
                bun.Unpack(reader, writer);

                nfs.Position = 0;
                fs.Close();

                fs = nfs;
                reader = new AssetsFileReader(fs);

                bun = new AssetBundleFile();
                bun.Read(reader);
                bun.Close();
            }

            return bun;
        }
        public static void CompressBundle(string file, string compFile)
        {
            var bun = DecompressBundle(file, null);
            var fs = File.OpenWrite(compFile);
            using var writer = new AssetsFileWriter(fs);
            bun.Pack(bun.reader, writer, AssetBundleCompressionType.LZMA);
        }

        static void Main(string[] args)
        {
        string compressedfile = args[0];
        string oavtrid = args[1];
        string navtrid = args[2];
        Console.WriteLine("Decompressing...");
        DecompressBundle(compressedfile, "decompressedfile");
        Console.WriteLine("Changing avatar ID...");
        var Process1 = Process.Start(@"Rewrite.exe",oavtrid + " " + navtrid);
        Process1.WaitForExit();
        Console.WriteLine("Compressing...");
        CompressBundle("decompressedfile1", "custom.vrca");
    }
}
}