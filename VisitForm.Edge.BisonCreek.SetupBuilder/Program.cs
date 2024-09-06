
using System.Diagnostics;

Console.WriteLine("Setup Builder Start");

Process publishProcess = new Process();
ProcessStartInfo publishStartInfo = new ProcessStartInfo();

publishStartInfo.FileName = "dotnet";

string csproj = @"C:\git\BisonCreek\BisonCreek\VisitForm.Edge.BisonCreek.csproj";
publishStartInfo.Arguments = $"publish {csproj} -c Release -r win-x64 --self-contained";

publishProcess.StartInfo = publishStartInfo;

publishProcess.Start();
publishProcess.WaitForExit();

Process process = new Process();
ProcessStartInfo startInfo = new ProcessStartInfo();

startInfo.FileName = @"C:\Program Files (x86)\NSIS\makensis.exe";


string a1 = "/D" + "SERVICENAME" + "=VisitFormEdgeSvc";

string a2 = "/D" + "SERVICEDISPLAYNAME" + "=\"VisitForm Edge Service\"";

string a3 = "/D" + "VERSION" + "=5.0.0";

string a4 = "/D" + "PUBLISHPATH" + "=" + @"C:\git\BisonCreek\BisonCreek\bin\Release\net8.0\win-x64\publish";

string a5 = "/D" + "SERVICEBIN" + "=VisitForm.Edge.BisonCreek";

string basepath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
string nsiFile = Path.Combine(basepath, "SetupScript", "setup.nsi");

string a6= "\"" + nsiFile + "\"";

string argument = $"{a1} {a2} {a3} {a4} {a5} {a6}";


startInfo.Arguments = argument;

process.StartInfo = startInfo;
process.Start();

// .nsi file located in
// C:\git\VisitForm\VisitForm\Edge\Tools\Setups\VisitForm.Edge.GenericEdge.SetupBuilder\bin\Debug\net8.0\SetupScript