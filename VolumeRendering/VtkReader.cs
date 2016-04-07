using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VolumeRendering
{
    class VtkReader
    {
        private int ox, oy, oz, dx, dy, dz, max;
        private double oxx, oyy, ozz, sx, sy, sz;

        public VtkReader()
        {
            oxx = oyy = ozz = sx = sy = sz = 0;
            dx = dy = dz = ox = oy = oz = max = 0;
        }

        public int Dx() { return dx; }
        public int Dy() { return dy; }
        public int Dz() { return dz; }
        public int Max() { return max; }

        public byte[] ReadVTK(string filename)
        {                        
            //filestream do ktoreho otvorime nas subor
            FileStream instream = File.Open(filename, FileMode.Open);
            //nasledne si tento stream otvorime v binarnom aj textovom readeri
            BinaryReader binreader = new BinaryReader(instream);
            StreamReader reader = new StreamReader(instream);
            int header = 10;
            for (int i = 0; i < header; i++)
            {

                string tmp = reader.ReadLine();
                if (tmp.StartsWith("DIMENSIONS"))
                {
                    dx = Convert.ToInt32(tmp.Split()[1]);
                    dy = Convert.ToInt32(tmp.Split()[2]);
                    dz = Convert.ToInt32(tmp.Split()[3]);
                }
                if (tmp.StartsWith("ORIGIN"))
                {
                    oxx = Convert.ToDouble(tmp.Split()[1]);
                    oyy = Convert.ToDouble(tmp.Split()[2]);
                    ozz = Convert.ToDouble(tmp.Split()[3]);
                }
                if (tmp.StartsWith("SPACING"))
                {
                    sx = Convert.ToDouble(tmp.Split()[1]);
                    sy = Convert.ToDouble(tmp.Split()[2]);
                    sz = Convert.ToDouble(tmp.Split()[3]);
                }
            }

            ox = (int)Math.Round(oxx / sx);
            oy = (int)Math.Round(oyy / sy);
            oz = (int)Math.Round(ozz / sz);

            // streamreader pouziva bufferovane citanie, cize aj ked sme precitali len cca 400 znakov, tak z povodneho streamu
            // streamreader precital 1024 bajtov do buffera, preto potrebujeme pouzit metodu GetActualPosition(), ktora nam vrati presnu poziciu pokial sme citali data
            // nasledne mozeme nastavit poziciu povodneho streamu na dany offset a zahodit bufferovane data
            reader.BaseStream.Position = GetActualPosition(reader);
            reader.DiscardBufferedData();
            // teraz nam binaryreader nacita zvysok suboru ako binarne data do pola
            byte[] data = binreader.ReadBytes((int)(binreader.BaseStream.Length - binreader.BaseStream.Position));
            if (max <= dx)
                max = dx;
            if (max < dy)
                max = dy;
            if (max < dz)
                max = dz;

            int tmp_dz = max - dz;
            int dolna_hranica = tmp_dz / 2;

            byte[] vtk_data = new byte[max * max * max];
            int counter = 0;
            try
            {
                for (int k = oz + dolna_hranica; k < dolna_hranica + oz + dz; k++)
                {
                    for (int j = oy; j < oy + dy; j++)
                    {
                        for (int i = ox; i < ox + dx; i++)
                        {
                            vtk_data[i + dx * (j + dy * k)] = data[counter++];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("dojeb v subore: " + ex.Message + "\n" + filename);
            }
            instream.Close();

            //centrovanie
            //byte[] centrovane = new byte[max * max * max];
            ////int tmp_dz = max - dz;
            //if (tmp_dz != 0)
            //{
            //    try
            //    {
            //        //int dolna_hranica = tmp_dz / 2;
            //        counter = 0;
            //        for (int k = oz + dolna_hranica; k < dolna_hranica + oz + dz; k++)
            //        {
            //            for (int j = oy; j < oy + dy; j++)
            //            {
            //                for (int i = ox; i < ox + dx; i++)
            //                {
            //                    centrovane[i + dx * (j + dy * k)] = vtk_data[counter++];
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Chyba pri nacitacvanie VTK suboru. \nData sa nepodarilo vycentrovat.");
            //    }
            //}
            return vtk_data;
        }

        static long GetActualPosition(StreamReader reader)
        {
            // The current buffer of decoded characters
            char[] charBuffer = (char[])reader.GetType().InvokeMember("charBuffer"
                , System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetField
                , null, reader, null);

            // The current position in the buffer of decoded characters
            int charPos = (int)reader.GetType().InvokeMember("charPos"
                , System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetField
                , null, reader, null);

            // The number of bytes that the already-read characters need when encoded.
            int numReadBytes = reader.CurrentEncoding.GetByteCount(charBuffer, 0, charPos);

            // The number of encoded bytes that are in the current buffer
            int byteLen = (int)reader.GetType().InvokeMember("byteLen"
                , System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetField
                , null, reader, null);

            return reader.BaseStream.Position - byteLen + numReadBytes;
        }
    }
}
