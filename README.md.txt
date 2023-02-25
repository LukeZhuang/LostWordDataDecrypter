# LostWorldDataDecrypter
## How to use this decrypter
1. decompose .apk file (ch_server)
2. copy the whole assests/master into original_files directory
3. run

## Notes
Currently it will only decrypter data types which are implemented in `DataTypes`. We need to decompile libil2cpp.so using il2cppdumper to get dummy.cs, and then copy the data types from there.

## NuGet Package dependencies
1. MsgPack.Cli
2. Newtonsoft.Json
