namespace pxtools
{
    public class PXParse
    {

        //private void HEAD_COPY(x, px_header header, ref i)
        //{
        //    copy_from_le(ref header.x, unp_head + i, sizeof(header.x));
        //    //         if (sizeof(header.x) == 1) \
        //    //	printf("Index: 0x%02x + %i (%s):\t %02x\n", i, sizeof(header.x),#x, header.x); \
        //    //else if (sizeof(header.x) == 2) \
        //    //	printf("Index: 0x%02x + %i (%s):\t %04x\n", i, sizeof(header.x),#x, header.x); \
        //    //else if (sizeof(header.x) == 4) \
        //    //	printf("Index: 0x%02x + %i (%s):\t %08x\n", i, sizeof(header.x),#x, header.x); \
        //    //else \
        //    //	printf("Index: 0x%02x + %i (%s):\t %08x\n", i, sizeof(header.x),#x, header.x); \
        //    i += sizeof(header.x);
        //}

        public static int PXparseHeader(Span<byte> unp_head, ref px_header header)
        {
            int i = 0;

            //HEAD_COPY(recordSize, header, i);
            PXConvert.copy_from_le(ref header.recordSize, unp_head[i..], sizeof(short));
            i += sizeof(short);

            //HEAD_COPY(headerSize, header, i);
            PXConvert.copy_from_le(ref header.headerSize, unp_head[i..], sizeof(short));
            i += sizeof(short);

            //HEAD_COPY(fileType);
            PXConvert.copy_from_le(ref header.fileType, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(maxTableSize);
            PXConvert.copy_from_le(ref header.maxTableSize, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(numRecords);
            PXConvert.copy_from_le(ref header.numRecords, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(usedBlocks);
            PXConvert.copy_from_le(ref header.usedBlocks, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(fileBlocks);
            PXConvert.copy_from_le(ref header.fileBlocks, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(firstBlock);
            PXConvert.copy_from_le(ref header.firstBlock, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(lastBlock);
            PXConvert.copy_from_le(ref header.lastBlock, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(dummy_1);
            PXConvert.copy_from_le(ref header.dummy_1, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(modifiedFlags1);
            PXConvert.copy_from_le(ref header.modifiedFlags1, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(IndexFieldNumber);
            PXConvert.copy_from_le(ref header.IndexFieldNumber, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(primaryIndexWorkspace);   /* pointer */
            PXConvert.copy_from_le(ref header.primaryIndexWorkspace, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(dummy_2);         /* pointer */
            PXConvert.copy_from_le(ref header.dummy_2, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(indexRootBlock);
            PXConvert.copy_from_le(ref header.indexRootBlock, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(indexLevels);
            PXConvert.copy_from_le(ref header.indexLevels, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(numFields);
            PXConvert.copy_from_le(ref header.numFields, unp_head[i..], sizeof(short));
            i += sizeof(short);

            //HEAD_COPY(primaryKeyFields);
            PXConvert.copy_from_le(ref header.primaryKeyFields, unp_head[i..], sizeof(short));
            i += sizeof(short);

            //HEAD_COPY(encryption1);
            PXConvert.copy_from_le(ref header.encryption1, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(sortOrder);
            PXConvert.copy_from_le(ref header.sortOrder, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(modifiedFlags2);
            PXConvert.copy_from_le(ref header.modifiedFlags2, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(dummy_5);
            PXConvert.copy_from_le(ref header.dummy_5, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(changeCount1);
            PXConvert.copy_from_le(ref header.changeCount1, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(changeCount2);
            PXConvert.copy_from_le(ref header.changeCount2, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(dummy_6);
            PXConvert.copy_from_le(ref header.dummy_6, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(tableNamePtr);        /* pointer */
            PXConvert.copy_from_le(ref header.tableNamePtr, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(fieldInfo);           /* pointer */
            PXConvert.copy_from_le(ref header.fieldInfo, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(writeProtected);
            PXConvert.copy_from_le(ref header.writeProtected, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(fileVersionID);
            PXConvert.copy_from_le(ref header.fileVersionID, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(maxBlocks);
            PXConvert.copy_from_le(ref header.maxBlocks, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(dummy_7);
            PXConvert.copy_from_le(ref header.dummy_7, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(auxPasswords);
            PXConvert.copy_from_le(ref header.auxPasswords, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(dummy_8);
            PXConvert.copy_from_le(ref header.dummy_8, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(cryptInfoStart);      /* pointer */
            PXConvert.copy_from_le(ref header.cryptInfoStart, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(cryptInfoEnd);        /* pointer */
            PXConvert.copy_from_le(ref header.cryptInfoEnd, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(dummy_9);
            PXConvert.copy_from_le(ref header.dummy_9, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(autoInc);
            PXConvert.copy_from_le(ref header.autoInc, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(dummy_a);
            PXConvert.copy_from_le(ref header.dummy_a, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(indexUpdateRequired);
            PXConvert.copy_from_le(ref header.indexUpdateRequired, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(dummy_b);
            PXConvert.copy_from_le(ref header.dummy_b, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY(dummy_c);
            PXConvert.copy_from_le(ref header.dummy_c, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(refIntegrity);
            PXConvert.copy_from_le(ref header.refIntegrity, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(dummy_d);
            PXConvert.copy_from_le(ref header.dummy_d, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            return 0;
        }

        //private void HEAD_COPY2(x)
        //{
        //    copy_from_le(&(header.x), unp_head + i, sizeof(header.x));
        //    if (sizeof(header.x) == 1)
        //        Console.WriteLine("Index: 0x%02x + %i (%s):\t %02x", i + 0x58, sizeof(header.x),#x, header.x); 
        // else if (sizeof(header.x) == 2)
        //        Console.WriteLine("Index: 0x%02x + %i (%s):\t %04x", i + 0x58, sizeof(header.x),#x, header.x); 
        // else if (sizeof(header.x) == 4)
        //        Console.WriteLine("Index: 0x%02x + %i (%s):\t %08x", i + 0x58, sizeof(header.x),#x, header.x); 
        // else
        //        Console.WriteLine("Index: 0x%02x + %i (%s):\t %08x", i + 0x58, sizeof(header.x),#x, header.x); 
        // i += sizeof(header.x);
        //}

        public static int PXparseHeaderV4(Span<byte> unp_head, ref px_header header)
        {
            int i = 0;

            //HEAD_COPY2(fileVersionID2);
            PXConvert.copy_from_le(ref header.fileVersionID2, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY2(fileVersionID3);
            PXConvert.copy_from_le(ref header.fileVersionID3, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY2(encryption2);
            PXConvert.copy_from_le(ref header.encryption2, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY2(fileUpdateTime);
            PXConvert.copy_from_le(ref header.fileUpdateTime, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY2(hiFieldID);
            PXConvert.copy_from_le(ref header.hiFieldID, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY2(hiFieldIDInfo);
            PXConvert.copy_from_le(ref header.hiFieldIDInfo, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY2(sometimesNumFields);
            PXConvert.copy_from_le(ref header.sometimesNumFields, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY2(dosGlobalCodePage);
            PXConvert.copy_from_le(ref header.dosGlobalCodePage, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY2(dummy_e);
            PXConvert.copy_from_le(ref header.dummy_e, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY2(changeCount4);
            PXConvert.copy_from_le(ref header.changeCount4, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY2(dummy_f);
            PXConvert.copy_from_le(ref header.dummy_f, unp_head[i..], sizeof(uint));
            i += sizeof(uint);

            //HEAD_COPY2(dummy_10);
            PXConvert.copy_from_le(ref header.dummy_10, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            return 0;
        }

        static bool isHeaderSupported(px_header header)
        {
            //if (header == null)  
            //	return 0;

            switch (header.fileVersionID)
            {
                case 0x03:
                case 0x04:
                case 0x05:
                case 0x06:
                case 0x07:
                case 0x08:
                case 0x09:
                case 0x0a:
                case 0x0b:
                case 0x0c: break;
                default:
                    Console.Error.WriteLine("unknown Fileversion ID");
                    return false;
            }

            switch (header.fileType)
            {
                case 0x00:
                case 0x01:
                case 0x02:
                case 0x03:
                case 0x04:
                case 0x05:
                case 0x06:
                case 0x07:
                case 0x08: break;
                default:
                    Console.Error.WriteLine("unknown FileType ID");
                    return false;
            }
            if (header.numRecords > 0 && header.firstBlock != 1)
            {
                Console.Error.WriteLine($"warning: numRecords > 0 ({header.numRecords}) && firstBlock != 1 ({header.firstBlock})");
                /*		return 0;*/
            }

            return true;
        }

        public static px_fieldInfo[] PXparseCompleteHeader(BinaryReader fd, ref px_header header)
        {
            int file_index = 0;
            Span<byte> unp_head = new byte[0x58]; //char
            Span<byte> unp_head4 = new byte[0x20]; //char[]
            /*unsigned*/
            Span<byte> d = new byte[2]; //char[]
            // 	void *ptr;
            Span<byte> ptr = new byte[4]; //char[]

            int i;
            byte[] c = new byte[1];

            px_fieldInfo[] felder;

            file_index += fd.Read(unp_head);
            PXparseHeader(unp_head, ref header);

            if (!isHeaderSupported(header))
            {
                Console.Error.WriteLine("unsupported Header");
                throw new Exception("unsupported Header");
            }

            if (header.fileVersionID >= 0x05 &&
                header.fileType != 0x1 &&
                header.fileType != 0x4 &&
                header.fileType != 0x7)
            {

                file_index += fd.Read(unp_head4);
                PXparseHeaderV4(unp_head4, ref header);
            }


            felder = new px_fieldInfo[header.numFields];

            /* FieldType/Size (0x78) */
            for (i = 0; i < header.numFields; i++)
            {
                felder[i] = new px_fieldInfo();
                felder[i].name = new byte[80];
                file_index += fd.Read(d);

                felder[i].type = d[0];
                felder[i].size = d[1];
            }
            /* tablenameptr - skipped */
            file_index += fd.Read(ptr);

            /* fieldnameptr - skipped */
            if (header.fileType == 0x0 ||
                header.fileType == 0x2 ||
                header.fileType == 0x8 ||
                header.fileType == 0x6
                )
            {
                for (i = 1; i <= header.numFields; i++)
                {
                    file_index += fd.Read(ptr);
                }
            }

            /* tablename */
            file_index += fd.Read(header.tableName);

            /* fix for tablename longer then 79 chars (PX 7.0)*/
            c[0] = 0;
            while (c[0] == 0)
            {
                file_index += fd.Read(c);
            }
            file_index--;
            fd.BaseStream.Seek(-1, SeekOrigin.Current);

            if (header.fileVersionID >= 0x04 &&
                header.fileType != 0x1 &&
                header.fileType != 0x4 &&
                header.fileType != 0x7)
            {
                /* FieldNames*/
                for (i = 1; i <= header.numFields; i++)
                {
                    int j = 0;
                    c[0] = (byte)' ';
                    while (c[0] != 0)
                    {
                        file_index += fd.Read(c);
                        felder[i - 1].name[j++] = c[0];
                        //#if DEBUG
                        //						if (c.Length > 0) 
                        //							Console.Write($"{(char)c[0]}");
                        //#endif
                    }
                    //#if DEBUG
                    //					Console.Write("\n");
                    //#endif
                }
                if (header.auxPasswords != 0)
                {
                    Span<byte> str = new byte[256]; //char[]
                    file_index += fd.Read(str);
                }
                /* field-position */
                for (i = 1; i <= header.numFields; i++)
                {
                    ushort s = fd.ReadUInt16();
                    file_index += sizeof(UInt16);
                    //#if DEBUG
                    //                    Console.WriteLine($"{s}");
                    //#endif
                }
                c[0] = (byte)' ';
                //#if DEBUG
                //				Console.Write("SortOrderID: ");
                //#endif
                while (c[0] != 0)
                {
                    file_index += fd.Read(c);
                    //#if DEBUG
                    //					if (c.Length > 0) 
                    //						Console.Write($"{(char)c[0]}");
                    //#endif
                }
                //#if DEBUG
                //				Console.Write("\n");
                //#endif
                if (header.fileType == PXTypes.PX_Filetype_XGn_Inc ||
                    header.fileType == PXTypes.PX_Filetype_XGn_NonInc)
                {
                    c[0] = (byte)' ';
                    //#if DEBUG
                    //					Console.WriteLine("IndexName: ");
                    //#endif
                    while (c[0] != 0)
                    {
                        file_index += fd.Read(c);
                        //#if DEBUG
                        //						if (c.Length > 0) 
                        //                            Console.Write($"{(char)c[0]}");
                        //#endif
                    }
                    //#if DEBUG
                    //					Console.Write("\n");
                    //#endif
                }

            }

            //#if DEBUG
            //			Console.WriteLine($">HeaderSize: {header.headerSize:X4}<");
            //#endif
            while (file_index < header.headerSize)
            {
                file_index += fd.Read(c);
            }

            return felder;
        }

        public static px_blocks[] PXparseBlocks(FileStream fd, px_header header)
        {
            int n = 0, i;
            /* support for maxTableSize == 16 */
            /*unsigned char[]*/
            Span<byte> block = new byte[0x400 * header.maxTableSize]; // 4096 * 8
            px_blocks[]? blocks = null;

            int block_nr = 0;
            long file_index = fd.Seek(0, SeekOrigin.Current);

            ushort nextBlock = 0, prevBlock = 0;

            //#if DEBUG
            //			Console.WriteLine($"FD-IDX: {file_index:X}");
            //#endif
            blocks = new px_blocks[header.fileBlocks];

            while ((n = fd.Read(block)) > 0)
            {
                short addDataSize = 0, numRecsInBlock;
                int block_index = 0;

                file_index += n;
                //#if DEBUG
                //                Console.WriteLine($"FD-IDX: {file_index:x}");
                //#endif

                PXConvert.copy_from_le(ref nextBlock, block[block_index..], sizeof(ushort));
                //#if DEBUG
                //Console.WriteLine($"Bl-IDX1: {block_index:x4}");
                //#endif
                block_index += sizeof(ushort);

                PXConvert.copy_from_le(ref prevBlock, block[block_index..], sizeof(ushort));
                //#if DEBUG
                //Console.WriteLine($"Bl-IDX1: {block_index:x4}");
                //#endif
                block_index += sizeof(ushort);

                PXConvert.copy_from_le(ref addDataSize, block[block_index..], sizeof(short));
                //#if DEBUG
                //Console.WriteLine($"Bl-IDX1: {block_index:x4}");
                //#endif
                block_index += sizeof(short);

                numRecsInBlock = (short)((addDataSize / header.recordSize) + 1);
                //#if DEBUG
                //				Console.WriteLine($"NextBlock:\t{nextBlock:X4}");
                //				Console.WriteLine($"PrevBlock:\t{prevBlock:X4}");
                //				Console.WriteLine($"AddDataSize:\t{addDataSize}");
                //				Console.WriteLine($"numRecsInBlock:\t{numRecsInBlock}");
                //#endif
                blocks[block_nr] = new px_blocks();
                blocks[block_nr].prevBlock = prevBlock;
                blocks[block_nr].nextBlock = nextBlock;
                blocks[block_nr].records = new byte[numRecsInBlock][];
                blocks[block_nr].numRecsInBlock = numRecsInBlock;

                for (i = 0; i < numRecsInBlock; i++)
                {
                    blocks[block_nr].records[i] = new byte[header.recordSize];
                    block[block_index..(block_index + header.recordSize)].CopyTo(blocks[block_nr].records[i]);
                    //#if DEBUG
                    //Console.WriteLine($"Bl-IDX1: {block_index:X4}");
                    //#endif
                    block_index += header.recordSize;
                }
                //#if DEBUG
                //				Console.WriteLine($"Curr: {(block_nr + 1):X4}. Prev: {prevBlock:X4}; Next: {nextBlock:X4}");
                //#endif
                block_nr++;
            }

            if (block_nr != header.fileBlocks)
            {
                Console.WriteLine($"Berr: {block_nr} . {header.fileBlocks}");
            }

            if (n == -1)
            {
                Console.Write("Fehler beim Lesen der Spalten");
            }
            //#if DEBUG
            //			n = header.firstBlock - 1;

            //			for (i = 0; i < header.usedBlocks; i++)
            //			{
            //				Console.WriteLine($"{blocks[n].prevBlock:X4} <- {n + 1:X4} . {blocks[n].nextBlock:X4}");
            //				n = blocks[n].nextBlock - 1;
            //			}
            //#endif

            return blocks;
        }

        public struct mb_type0
        {
            /*unsigned*/
            public /*char*/ byte type;
            public ushort num_blocks;
            public ushort mod_count;
            public ushort size_block;
            public ushort size_sub_block;
            /*unsigned*/
            public /*char*/ byte size_sub_chunk;
            public ushort num_sub_chunk;
            public ushort thres_sub_chunk;
        };

        public int PXparseMBType0(Span<byte> unp_head)
        {
            int i = 0;
            mb_type0 header = new mb_type0();

            //HEAD_COPY(type);
            PXConvert.copy_from_le(ref header.type, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(num_blocks);
            PXConvert.copy_from_le(ref header.num_blocks, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(mod_count);
            PXConvert.copy_from_le(ref header.mod_count, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            /* jump to 0xb */
            i = 0xb;

            //HEAD_COPY(size_block);
            PXConvert.copy_from_le(ref header.size_block, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(size_sub_block);
            PXConvert.copy_from_le(ref header.size_sub_block, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            /* jump to 0x10 */
            i = 0x10;

            //HEAD_COPY(size_sub_chunk);
            PXConvert.copy_from_le(ref header.size_sub_chunk, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(num_sub_chunk);
            PXConvert.copy_from_le(ref header.num_sub_chunk, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //HEAD_COPY(thres_sub_chunk);
            PXConvert.copy_from_le(ref header.thres_sub_chunk, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            //#if DEBUG
            //		Console.WriteLine(string.Format("type: %d", header.type));
            //		Console.WriteLine(string.Format("num_blocks: %d", header.num_blocks));
            //		Console.WriteLine(string.Format("mod_count: %d", header.mod_count));
            //		Console.WriteLine(string.Format("size_blocks: %04x", header.size_block));
            //		Console.WriteLine(string.Format("size_sub_blocks: %04x", header.size_sub_block));
            //		Console.WriteLine(string.Format("size_sub_chunk: %04x", header.size_sub_chunk));
            //		Console.WriteLine(string.Format("num_sub_chunk: %04x", header.num_sub_chunk));
            //		Console.WriteLine(string.Format("thres_sub_chunk: %04x", header.thres_sub_chunk));
            //#endif
            return 0;
        }

        public struct mb_type3_pointer
        {
            public /*unsigned char*/ byte offset;
            public /*unsigned char*/ byte length_div_16;
            public ushort mod_count;
            public /*unsigned char*/ byte length_mod_16;
        };

        public struct mb_type3
        {
            public /*unsigned char*/ byte type;
            public ushort num_blocks;

            public mb_type3_pointer[] blob_pointer/*[64]*/;
        };

        //        private void HEAD_COPY3(int x, ref int i)
        //        {
        //#if DEBUG_HEAD_COPY

        //	copy_from_le(&(header.x), unp_head + i, sizeof(header.x)); 
        //	if (sizeof(header.x) == 1) 
        //		printf("Index: 0x%02x + %i (%s):\t %02x\n", i, sizeof(header.x),#x, header.x); 
        //	else if (sizeof(header.x) == 2) 
        //		printf("Index: 0x%02x + %i (%s):\t %04x\n", i, sizeof(header.x),#x, header.x); 
        //	else if (sizeof(header.x) == 4) 
        //		printf("Index: 0x%02x + %i (%s):\t %08x\n", i, sizeof(header.x),#x, header.x); 
        //	else 
        //		printf("Index: 0x%02x + %i (%s):\t %08x\n", i, sizeof(header.x),#x, header.x); 
        //	i += sizeof(header.x);
        //    }
        //#else
        //            copy_from_le(&(header.x), unp_head + i, sizeof(header.x));
        //            i += sizeof(header.x);
        //#endif
        //        }

        public int PXparseMBType3(Span<byte> unp_head)
        {
            int i = 0, j;
            mb_type3 header = new mb_type3();

            //HEAD_COPY(type);
            PXConvert.copy_from_le(ref header.type, unp_head[i..]);
            i += sizeof(byte);

            //HEAD_COPY(num_blocks);
            PXConvert.copy_from_le(ref header.num_blocks, unp_head[i..], sizeof(ushort));
            i += sizeof(ushort);

            i = 12;

            for (j = 0; j < 64; j++)
            {
                //#if DEBUG
                //	Console.WriteLine(string.Format("Index (%d): %x", j, i));
                //#endif
                //HEAD_COPY3(blob_pointer[j].offset, ref i);
                PXConvert.copy_from_le(ref header.blob_pointer[j].offset, unp_head[i..]);
                i += sizeof(byte);

                //HEAD_COPY3(blob_pointer[j].length_div_16, ref i);
                PXConvert.copy_from_le(ref header.blob_pointer[j].length_div_16, unp_head[i..]);
                i += sizeof(byte);

                //HEAD_COPY3(blob_pointer[j].mod_count, ref i);
                PXConvert.copy_from_le(ref header.blob_pointer[j].mod_count, unp_head[i..], sizeof(ushort));
                i += sizeof(ushort);

                //HEAD_COPY3(blob_pointer[j].length_mod_16, ref i);
                PXConvert.copy_from_le(ref header.blob_pointer[j].length_mod_16, unp_head[i..]);
                i += sizeof(byte);

            }
            //#if DEBUG
            //Console.WriteLine(string.Format("Index: %x\n", i));

            //Console.WriteLine(string.Format("type: %d\n", header.type));
            //Console.WriteLine(string.Format("num_blocks: %d\n", header.num_blocks));
            //#endif
            /* jump to 0x150. start of the chunks */

            i = 0x150;

            for (j = 0; j < 64; j++)
            {
                int start = header.blob_pointer[j].offset * 16;
                int length = header.blob_pointer[j].length_div_16 * 16 + header.blob_pointer[j].length_mod_16;
                byte[] str;
                //#if DEBUG
                //	Console.WriteLine(string.Format("offset      [%d]: %d\n", j, header.blob_pointer[j].offset));
                //	Console.WriteLine(string.Format("length / 16 [%d]: %d\n", j, header.blob_pointer[j].length_div_16));
                //	Console.WriteLine(string.Format("mod_count   [%d]: %d\n", j, header.blob_pointer[j].mod_count));
                //	Console.WriteLine(string.Format("length %% 16 [%d]: %d\n", j, header.blob_pointer[j].length_mod_16));
                //#endif
                str = new byte[length + 1];

                Array.Copy(unp_head[start..].ToArray(), str, length);
                str[length] = 0;

                if (length > 0)
                {
                    Console.Error.WriteLine(string.Format("Offset: %d, Length: %d", start, length));
                    Console.Error.WriteLine(string.Format("String      [%d]: %s", j, str));
                }

                i += length;
            }

            return 0;
        }

        public int PXparseMBHeader(FileStream fd)
        {
            int file_index = 0, n;
            Span<byte> mb_type = new byte[1];
            ushort mb_num_blocks = 0;

            Span<byte> unp_header = new byte[4096];

            /* rewind to the first byte */
            fd.Seek(0, SeekOrigin.Begin);

            while ((n = fd.Read(unp_header)) > 0)
            {
                file_index += n;

                PXConvert.copy_from_le(mb_type, unp_header, 1);
                PXConvert.copy_from_le(ref mb_num_blocks, unp_header[1..], 2);

                switch (mb_type[0])
                {
                    case 00: /* mb header */
                        PXparseMBType0(unp_header);
                        break;
                    case 02: break;
                    case 03:
                        PXparseMBType3(unp_header);
                        break;
                    case 04: break;
                    default:
                        return -1;
                }
            }

            return 0;
        }

    }
}
