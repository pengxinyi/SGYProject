
copy .\BpImplement\bin\Debug\UFIDA.U9.LH.LHPubBP.Deploy.dll  ..\..\Deploy\Portal\ApplicationLib
copy .\BpImplement\bin\Debug\UFIDA.U9.LH.LHPubBP.Deploy.pdb  ..\..\Deploy\Portal\ApplicationLib
copy .\BpAgent\bin\Debug\UFIDA.U9.LH.LHPubBP.Agent.dll  ..\..\Deploy\Portal\ApplicationLib
copy .\BpAgent\bin\Debug\UFIDA.U9.LH.LHPubBP.Agent.pdb  ..\..\Deploy\Portal\ApplicationLib

copy .\BpImplement\bin\Debug\UFIDA.U9.LH.LHPubBP.Deploy.dll  ..\..\Deploy\Portal\ApplicationServer\Libs
copy .\BpImplement\bin\Debug\UFIDA.U9.LH.LHPubBP.Deploy.pdb  ..\..\Deploy\Portal\ApplicationServer\Libs
copy .\BpAgent\bin\Debug\UFIDA.U9.LH.LHPubBP.Agent.dll  ..\..\Deploy\Portal\ApplicationServer\Libs
copy .\BpAgent\bin\Debug\UFIDA.U9.LH.LHPubBP.Agent.pdb  ..\..\Deploy\Portal\ApplicationServer\Libs
copy .\BpImplement\bin\Debug\UFIDA.U9.LH.LHPubBP.dll  ..\..\Deploy\Portal\ApplicationServer\Libs
copy .\BpImplement\bin\Debug\UFIDA.U9.LH.LHPubBP.pdb  ..\..\Deploy\Portal\ApplicationServer\Libs
copy .\BpImplement\bin\Debug\UFIDA.U9.LH.LHPubBP.ubfsvc  ..\..\Deploy\Portal\ApplicationServer\Libs



echo 请手工将该bat文件打开，将下面这段内容与..\..\Deploy\Portal\RestServices\web.config进行合并。


pause