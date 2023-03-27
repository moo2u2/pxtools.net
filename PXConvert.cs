namespace pxtools
{
    public class PXConvert
    {
        private static int VALUE_IS_NULL = 10;
        private static int VALUE_OK = 0;
        private static int VALUE_ERROR = -1;

        /*************************
		   taken from c't 1997 - 15
		   ftp://ftp.heise.de/pub/ct/ct9715.zip

		   input:   julian date
					(day since 1.1.4713 before Chr.)
		   output:  year
					month (1=Jan, 2=Feb, ... 12 = Dec)
					day   (1...31)
		   modified algorithm by R. G. Tantzen
		*/
        public static void gdate(long jd, ref int jahr, ref int monat, ref int tag)
        {
            long j, m, t;
            jd -= 1721119L;

            j = (4L * jd - 1L) / 146097L;
            jd = (4L * jd - 1L) % 146097L;
            t = jd / 4L;

            jd = (4L * t + 3L) / 1461L;
            t = (4L * t + 3L) % 1461L;
            t = (t + 4L) / 4L;

            m = (5L * t - 3L) / 153L;
            t = (5L * t - 3L) % 153L;
            t = (t + 5L) / 5L;

            j = 100L * j + jd;
            if (m < 10L)
            {
                m += 3;
            }
            else
            {
                m -= 9;
                j++;
            }

            jahr = (int)j;
            monat = (int)m;
            tag = (int)t;
        }

        public static long jdatum(int jahr, int monat, int tag)
        {
            long c, y;
            if (monat > 2)
            {
                monat -= 3;
            }
            else
            {
                monat += 9;
                jahr--;
            }
            tag += (153 * monat + 2) / 5;
            c = (146097L * (((long)jahr) / 100L)) / 4L;
            y = (1461L * (((long)jahr) % 100L)) / 4L;
            return c + y + (long)tag + 1721119L;
        }

        /*
		   taken for c't 1997 - 15
		**************************/

        public static void copy_from_be(byte[] _dst, byte[] src, int len)
        {

            int i;
            for (i = 0; i < len; i++)
            {
                if (!BitConverter.IsLittleEndian)
                    _dst[i] = src[i];
                else
                    _dst[len - i - 1] = src[i];
            }
        }

        public static void copy_from_le(ref byte _dst, Span<byte> src)
        {
            _dst = src[0];
        }

        public static void copy_from_le(Span<byte> _dst, Span<byte> src, int len)
        {
            int i;
            for (i = 0; i < len; i++)
            {
                if (!BitConverter.IsLittleEndian)
                    _dst[len - i - 1] = src[i];
                else
                    _dst[i] = src[i];
            }
        }

        public static void copy_from_le(ref short _dst, Span<byte> src, int len)
        {
            int i;
            Span<byte> dst = new byte[len];
            for (i = 0; i < len; i++)
            {
                if (!BitConverter.IsLittleEndian)
                    dst[len - i - 1] = src[i];
                else
                    dst[i] = src[i];
            }
            _dst = BitConverter.ToInt16(dst);
        }

        public static void copy_from_le(ref ushort _dst, Span<byte> src, int len)
        {
            int i;
            Span<byte> dst = new byte[len];
            for (i = 0; i < len; i++)
            {
                if (!BitConverter.IsLittleEndian)
                    dst[len - i - 1] = src[i];
                else
                    dst[i] = src[i];
            }
            _dst = BitConverter.ToUInt16(dst);
        }

        public static void copy_from_le(ref uint _dst, Span<byte> src, int len)
        {
            int i;
            Span<byte> dst = new byte[len];
            for (i = 0; i < len; i++)
            {
                if (!BitConverter.IsLittleEndian)
                    dst[len - i - 1] = src[i];
                else
                    dst[i] = src[i];
            }
            _dst = BitConverter.ToUInt32(dst);
        }

        public static void copy_from_le(ref int _dst, Span<byte> src, int len)
        {
            int i;
            Span<byte> dst = new byte[len];
            for (i = 0; i < len; i++)
            {
                if (!BitConverter.IsLittleEndian)
                    dst[len - i - 1] = src[i];
                else
                    dst[i] = src[i];
            }
            _dst = BitConverter.ToInt32(dst);
        }

        public static void copy_from_le(ref ulong _dst, Span<byte> src, int len)
        {
            int i;
            Span<byte> dst = new byte[len];
            for (i = 0; i < len; i++)
            {
                if (!BitConverter.IsLittleEndian)
                    dst[len - i - 1] = src[i];
                else
                    dst[i] = src[i];
            }
            _dst = BitConverter.ToUInt64(dst);
        }

        public static void fix_sign(Span<byte> dst, int len)
        {
            if (!BitConverter.IsLittleEndian)
                dst[0] &= 0x7f;
            else
                dst[len - 1] = (byte)(dst[len - 1] & 0x7f);
        }

        public static void set_sign(Span<byte> dst, int len)
        {
            if (!BitConverter.IsLittleEndian)
                dst[0] |= 0x80;
            else
                dst[len - 1] = (byte)(dst[len - 1] | 0x80);
        }

        public static int PXtoLong(/*ulong*/ byte[] number, ref ulong ret, int type)
        {
            if(number.Length < 8)
                Array.Resize(ref number, 8);
            byte[] s = number;// BitConverter.GetBytes(number);
            byte[] d = new byte[s.Length];

            switch (type)
            {
                case PXTypes.PX_Field_Type_Logical:
                    copy_from_be(d, s, 1);

                    if ((s[0] & 0x80) != 0)
                    {
                        fix_sign(d, 1);
                    }
                    else if (BitConverter.ToUInt64(d) == 0)
                    {
                        return VALUE_IS_NULL;
                    }
                    else
                    {
                        set_sign(d, 1);
                    }
                    break;

                case PXTypes.PX_Field_Type_ShortInt:
                    copy_from_be(d, s, 2);

                    if ((s[0] & 0x80) != 0)
                    {
                        fix_sign(d, 2);
                    }
                    else if (BitConverter.ToUInt64(d) == 0)
                    {
                        return VALUE_IS_NULL;
                    }
                    else
                    {
                        set_sign(d, 2);
                    }
                    break;
                case PXTypes.PX_Field_Type_Incremental:
                case PXTypes.PX_Field_Type_LongInt:
                    copy_from_be(d, s, 4);

                    if ((s[0] & 0x80) != 0)
                    {
                        fix_sign(d, 4);
                    }
                    else if (BitConverter.ToUInt64(d) == 0)
                    {
                        return VALUE_IS_NULL;
                    }
                    else
                    {
                        set_sign(d, 4);
                    }
                    break;
                default:
                    Console.Error.WriteLine(string.Format("%s.%d: Can't convert type (%02x)!\n", "PXConvert", 183, type));
                    return VALUE_ERROR;

            }
            ret = BitConverter.ToUInt64(d);
            return VALUE_OK;
        }

        public static int PXtoDouble(ulong /*long*/ number, ref double ret, int type)
        {
            double retval = 0;
            /*unsigned*/
            byte[] s = BitConverter.GetBytes(number);
            /*unsigned*/
            byte[] d = BitConverter.GetBytes(retval);

            switch (type)
            {
                case PXTypes.PX_Field_Type_Currency:
                case PXTypes.PX_Field_Type_Number:
                    copy_from_be(d, s, 8);

                    if ((s[0] & 0x80) != 0)
                    {
                        /* positive */
                        fix_sign(d, 8);
                    }
                    else if (retval == 0)
                    {
                        return VALUE_IS_NULL;
                    }
                    else
                    {
                        int i;
                        for (i = 0; i < 8; i++)
                            d[i] = (byte)~d[i];
                    }
                    break;
                default:
                    Console.Error.WriteLine(string.Format("%s.%d: Can't convert type (%02x)!\n", "PXConvert", 222, type));
                    return VALUE_ERROR;
            }
            ret = retval;
            return VALUE_OK;
        }

        public static int PXtoTM(ulong number, ref tm _tm, int type)
        {
            long t = 0;
            byte[] s = BitConverter.GetBytes(number);
            byte[] d = BitConverter.GetBytes(0L);
            //long jd;
            //int y, m, dy;

            //tm _tm = tm;

            switch (type)
            {
                case PXTypes.PX_Field_Type_Date:
                    copy_from_be(d, s, 4);

                    if ((s[0] & 0x80) != 0)
                    {
                        fix_sign(d, 4);
                    }
                    else if (BitConverter.ToInt64(d) == 0)
                    {
                        return VALUE_IS_NULL;
                    }
                    else
                    {
                        Console.Error.WriteLine($"DATE can't be nagative");
                        return VALUE_ERROR;
                    }

                    /* This is Y2K workaround !!!
                    ** if the date is before 1.1.1970 i add 100 years (365*100 + 24)
                    ** (seem not to be valid for paradox 7.0)
                    */
#if Y2K_WORKAROUND
			if (retval < 719528) {
				retval += 36524;
			}
#endif
                    //#if 1
                    //			jd = jdatum(1,1,1);
                    //			jd += retval - 1;

                    //			gdate(jd, &y, &m, &dy);

                    //			/* if the date has more than letters 
                    //			   we assume that it some inserted correctly 
                    //			   (not as an 2 letter short cut.) */

                    //			if (y >= 100) 
                    //				_tm.tm_year	= y - 1900;
                    //			else 
                    //				_tm.tm_year	= y;

                    //			_tm.tm_mon	= m - 1;
                    //			_tm.tm_mday	= dy;

                    //#else
                    t = (BitConverter.ToInt64(d) - 719528 + 365) * 24 * 60 * 60;

                    DateTime now = DateTime.Now;
                    _tm = new tm();
                    _tm.tm_sec = now.Second;
                    _tm.tm_min = now.Minute;
                    _tm.tm_hour = now.Hour;
                    //#endif
                    break;
                case PXTypes.PX_Field_Type_Time:
                    copy_from_be(d, s, 4);

                    if ((s[0] & 0x80) != 0)
                    {
                        fix_sign(d, 4);

                        long retval = BitConverter.ToInt64(d);

                        retval /= 1000; // lost miliseconds !!
                        _tm.tm_sec = (int)(retval % 60);
                        retval /= 60;
                        _tm.tm_min = (int)(retval % 60);
                        _tm.tm_hour = (int)(retval / 60);
                    }
                    else if (BitConverter.ToInt64(d) == 0)
                    {
                        return VALUE_IS_NULL;
                    }
                    else
                    {
                        Console.Error.WriteLine(string.Format("TIMESTAMP(%016llx . %016llx) can't be nagative\n", number, BitConverter.ToInt64(d)));
                        return VALUE_ERROR;
                    }
                    break;
                case PXTypes.PX_Field_Type_Timestamp:
                    copy_from_be(d, s, 8);

                    if ((s[0] & 0x80) != 0)
                    {
                        fix_sign(d, 8);

                        long retval = BitConverter.ToInt64(d);

                        /* the last byte is unused */
                        retval >>= 8;

                        /* the timestamp seems to have a resolution of 1/500s */
                        retval /= 500;

                        /* the adjustment that is neccesary to convert paradox
                        ** timestamp to unix-timestamp [. time()] 
                        **
                        ** FIXME: this value is guessed !! 
                        ** the garantued precission is +/- 1 second 
                        */

                        retval -= 37603860709183;

                        t = retval;

                        DateTime nowUtc = new DateTime(t);

                        _tm = new tm();
                        _tm.tm_sec = nowUtc.Second;
                        _tm.tm_min = nowUtc.Minute;
                        _tm.tm_hour = nowUtc.Hour;
                    }
                    else if (BitConverter.ToInt64(d) == 0)
                    {
                        return VALUE_IS_NULL;
                    }
                    else
                    {
                        Console.Error.WriteLine($"TIMESTAMP({number:016llx}.{BitConverter.ToInt64(d):016llx}) can't be nagative");
                        return VALUE_ERROR;
                    }
                    break;
                default:
                    Console.Error.WriteLine($"Can't convert type ({type:x2})!");
                    return -1;
            }

            //tm = _tm;
            return VALUE_OK;
        }

        public static void PXtoQuotedString(byte[] dst, byte[] src, int type)
        {
            switch (type)
            {
                case PXTypes.PX_Field_Type_Alpha:
                case PXTypes.PX_Field_Type_MemoBLOB:
                    break;
                default:
                    Console.Error.WriteLine($"PXConvert.{418}: Can't convert type ({type:x2})!");
                    throw new Exception();
            }

            int s = 0;
            while (s < src.Length)
            {
                switch ((int)src[s])
                {
                    /* cp431(??) . latin1 */
                    case 0x81: dst[s] = (byte)'ü'; break;
                    case 0x84: dst[s] = (byte)'ä'; break;
                    case 0x8e: dst[s] = (byte)'Ä'; break;
                    case 0x94: dst[s] = (byte)'ö'; break;
                    case 0x99: dst[s] = (byte)'Ö'; break;
                    case 0x9a: dst[s] = (byte)'Ü'; break;
                    case 0xe1: dst[s] = (byte)'ß'; break;
                    default: dst[s] = src[s]; break;
                }
                s++;
            }
            dst[s] = Convert.ToByte('\0');
        }

        public static byte[] PXNametoQuotedName(byte[] str)
        {
            for (int s = 0; s < str.Length; s++)
            {
                switch ((int)str[s])
                {
                    case 0x81: str[s] = (byte)'ü'; break;
                    case 0x84: str[s] = (byte)'ä'; break;
                    case 0x8e: str[s] = (byte)'Ä'; break;
                    case 0x94: str[s] = (byte)'ö'; break;
                    case 0x99: str[s] = (byte)'Ö'; break;
                    case 0x9a: str[s] = (byte)'Ü'; break;
                    case 0xe1: str[s] = (byte)'ß'; break;
                    case 0x20: str[s] = (byte)'_'; break;
                    case '-': str[s] = (byte)'_'; break;
                }
            }
            return str;
        }

        public struct mb_type2_pointer
        {
            public byte type;
            public ushort size_div_4k;
            public ulong length;
            public ushort mod_count;
        };

        public struct mb_type3_pointer
        {
            public byte offset;
            public byte length_div_16;
            public ushort mod_count;
            public byte length_mod_16;
        };

        public static byte[]? PXMEMOtoString(byte[] blob, int size, string blobname)
        {
            UInt32 offset = 0;
            int length = 0;
            ushort mod_number = 0;
            byte index = 0;
            byte[]? str = null;

            /* if the MEMO field contains less than 10 bytes it is stored in the .DB file */
            if (size < 10)
                return null;

            copy_from_le(ref offset, blob[(size - 10)..], 4);
            copy_from_le(ref length, blob[(size - 6)..], 4);
            copy_from_le(ref mod_number, blob[(size - 2)..], 2);

            copy_from_le(ref index, blob[(size - 10)..]);

            offset &= 0xffffff00;
//#if DEBUG
//            Console.WriteLine($"[BLOB] offset: {offset:x08l}, length: {length:08lx}, mod_number: {mod_number:x04}, index: {index:x02}");
//#endif
            if (index == 0x00)
                return null;

            if (string.IsNullOrEmpty(blobname))
            {
                Console.Error.WriteLine($"[BLOB] offset: {offset:x08l}, length: {length:x08l}, mod_number: {mod_number:x04}, index: {index:x02} - do I need a BLOB-filename '-b ...' ?");
                throw new Exception("Blobname is null");
            }

            try
            {
                using (FileStream fd = File.Open(blobname, FileMode.Open, FileAccess.Read))
                {
                    if (index == 0xff)
                    {
                        /* type 02 block */
                        mb_type2_pointer idx = new mb_type2_pointer();
                        byte[] head = new byte[9];

                        /* go to the right block */
                        fd.Seek(offset, SeekOrigin.Begin);

                        fd.Read(head);

                        copy_from_le(ref idx.type, head);
                        copy_from_le(ref idx.size_div_4k, head[1..], sizeof(ushort));
                        copy_from_le(ref idx.length, head[3..], sizeof(ulong));
                        copy_from_le(ref idx.mod_count, head[7..], sizeof(ushort));
                        if (idx.type != 0x02)
                        {
                            Console.Error.WriteLine($"Expected a type 02 blob");
                            throw new Exception("Invalid IDX type");
                        }

                        if ((int)length != (int)idx.length)
                        {
                            Console.Error.WriteLine($"Type 02 blob length doesn't match with BLOB length ({length} != {idx.length})");
                            throw new Exception("Type 02 blob length doesn't match BLOB length");
                        }
                        //#if 0
                        //            		fprintf(stderr, "%02d, %04d, %04ld, %04d, %04d\n",
                        //            			idx.type,
                        //            			idx.size_div_4k * 4096,
                        //            			idx.length,
                        //            			idx.mod_count,
                        //            			length);
                        //#endif
                        str = new byte[length + 1];

                        if (fd.Read(str, 0, length) != length)
                            Console.Error.WriteLine($"Read less than requested");
                        str[length] = (byte)'\0';
                    }
                    else
                    {
                        int _start, _length;
                        byte[] head = new byte[5];
                        mb_type3_pointer idx = new mb_type3_pointer();

                        /* go to the right block and skip the header */
                        fd.Seek(offset + (12 + (5 * index)), SeekOrigin.Begin);

                        fd.Read(head);

                        copy_from_le(ref idx.offset, head);
                        copy_from_le(ref idx.length_div_16, head[1..]);
                        copy_from_le(ref idx.mod_count, head[2..], sizeof(ushort));
                        copy_from_le(ref idx.length_mod_16, head[4..]);

                        _start = idx.offset * 16;
                        _length = idx.length_div_16 * 16 + idx.length_mod_16;

                        fd.Seek(offset + _start, SeekOrigin.Begin);

                        str = new byte[length + 1];

                        int read = fd.Read(str, 0, length);
                        if (read != length)
                            Console.Error.WriteLine($"Read less than requested");
                        str[length] = (byte)'\0';

                        if (!blob[..(size - 10)].SequenceEqual(str[..(size - 10)]))
                        {
                            Console.Error.WriteLine($"Extract failed: |{blob[..(size - 10)].ToCleanString()}| != |{str[..(size - 10)].ToCleanString()}|");
                            str = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Exception opening blob file: {e}");
                throw;
            }
            return str;
        }


        public static int PXBLOBtoBinary(byte[] blob, int size, string blobname, ref byte[]? binstorage, ref uint binsize)
        {
            UInt32 offset = 0;
            int length = 0;
            ushort mod_number = 0;
            /*unsigned char*/
            byte index = 0; 
            byte[]? str = null; //char[]
            FileStream fd;

            /* if the MEMO field contains less than 10 bytes it is stored in the .DB file */
            if (size < 10)
                return 0;

            copy_from_le(ref offset, blob[(size - 10)..], 4);
            copy_from_le(ref length, blob[(size - 6)..], 4);
            copy_from_le(ref mod_number, blob[(size - 2)..], 2);

            copy_from_le(ref index, blob[(size - 10)..]);

            offset &= 0xffffff00;
//#if DEBUG
//            fprintf(stderr, "[BLOB] offset: %08lx, length: %08lx, mod_number: %04x, index: %02x\n", offset, length, mod_number, index);
//#endif
            if (index == 0x00)
                return 0;

            //if (!blobname)
            //{
            //    Console.Error.WriteLine(string.Format("[BLOB] offset: %08lx, length: %08lx, mod_number: %04x, index: %02x - do I need a BLOB-filename '-b ...' ?\n", offset, length, mod_number, index));
            //    return -1;
            //}
            //#if O_BINARY
            //    fd = open(blobname, O_RDONLY | O_BINARY);
            //#else
            try
            {
                fd = File.Open(blobname, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return -1;
            }
            //#endif
            //if (fd == -1)
            //    return -1;

            if (index == 0xff)
            {
                /* type 02 block */
                mb_type2_pointer idx = new mb_type2_pointer();
                byte[] head = new byte[9]; // char[]

                /* go to the right block */
                //lseek(fd, offset, SEEK_SET);
                fd.Seek(offset, SeekOrigin.Begin);

                fd.Read(head, 0, head.Length);

                copy_from_le(ref idx.type, head);
                copy_from_le(ref idx.size_div_4k, head[1..], sizeof(ushort));
                copy_from_le(ref idx.length, head[3..], sizeof(ulong));
                copy_from_le(ref idx.mod_count, head[7..], sizeof(ushort));
                if (idx.type != 0x02)
                {
                    Console.Error.WriteLine(string.Format("%s.%d: expected a type 02 blob\n", "PXConvert", 632));
                    return -1;
                }

                if ((int)length != (int)idx.length)
                {
                    Console.Error.WriteLine(string.Format("%s.%d: type 02 blob length doesn't match with BLOB length\n", "PXConvert", 638));
                    return -1;
                }
//#if 0
//            		fprintf(stderr, "%02d, %04d, %04ld, %04d, %04d\n",
//            			idx.type,
//            			idx.size_div_4k * 4096,
//            			idx.length,
//            			idx.mod_count,
//            			length);
//#endif
                str = new byte[length + 1];

                if (fd.Read(str, 0, length) != length)
                {
                    Console.Error.WriteLine(string.Format("%s.%d: Read less than requested\n", "PXConvert", 653));
                }
                str[length] = 0;// '\0';

            }
            else
            {
                int _start, _length;
                byte[] head = new byte[5];
                mb_type3_pointer idx = new mb_type3_pointer();

                /* go to the right block and skip the header */
                //lseek(fd, offset + (12 + (5 * index)), SEEK_SET);
                fd.Seek(offset + (12 + (5 * index)), SeekOrigin.Begin);

                fd.Read(head, 0, head.Length);

                copy_from_le(ref idx.offset, head);
                copy_from_le(ref idx.length_div_16, head[1..]);
                copy_from_le(ref idx.mod_count, head[2..], sizeof(ushort));
                copy_from_le(ref idx.length_mod_16, head[4..]);

                _start = idx.offset * 16;
                _length = idx.length_div_16 * 16 + idx.length_mod_16;

                //lseek(fd, offset + _start, SEEK_SET);
                fd.Seek(offset + _start, SeekOrigin.Begin);

                str = new byte[length + 1];

                if (fd.Read(str, (int)(offset + _start), (int)length) != length)
                {
                    Console.Error.WriteLine(string.Format("%s.%d: Read less than requested\n", "PXConvert", 684));
                }
                str[length] = 0;//'\0';

                if (blob[(size - 10)..] != str[(size - 10)..])
                {
                    Console.Error.WriteLine(string.Format("%s.%d: Extract failed: %s != %s", "PXConvert", 690, blob, str));

                    str = null;
                }
            }

            fd.Close();

            binsize = Convert.ToUInt32(length);
            binstorage = str;

            return 0;
        }

    }
}
