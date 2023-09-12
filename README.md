# TikaTest-IKVM.Maven.Sdk

A test to get AutoDetectParser working with C#, Tika, and [IKVM.Maven.Sdk](https://github.com/ikvmnet/ikvm-maven)

Thanks to [github user Arextion](https://github.com/Arextion) for finding that preloading assemblies made it work, on this PR discussion that may or may not still be there in the future:  https://github.com/KevM/tikaondotnet/pull/152#issuecomment-1713804124

The csproj file contains all the MavenReferences - possibly more than is actually required. I followed [this guide on Tika Logging](https://cwiki.apache.org/confluence/display/tika/Logging) which added and removed a lot of logging assemblies, and still I get warnings, because I haven't figured out which ones I need to preload.