//получаем имя к файла, версию которого необходимо извлечь
var args = WScript.Arguments;
var fileName = args.Item(0);
//объект для работы с файловой системой
var fso = new ActiveXObject("Scripting.FileSystemObject");
//версия файла
var fileVersion = fso.GetFileVersion(fileName);
//пишем в outstream
WScript.Echo(fileVersion);