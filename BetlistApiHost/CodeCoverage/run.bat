@echo off

setlocal ENABLEDELAYEDEXPANSION
REM Set project name

set project=%1

if "%project%" == "" goto missing_project_name

set fileList=
for %%y in (%*) do (

set v1=%%y
call set v2=x%%v1:UnitTests=%%%%

if not !v2!==x%%y (set fileList=!fileList! UnitTests\%%y\bin\Debug\%%y.dll)

)

REM echo %fileList%

REM create output folder
if not exist ..\cireport\testcoveragereport mkdir ..\cireport\testcoveragereport
if not exist ..\cireport\coberturareport mkdir ..\cireport\coberturareport

REM Set working dir
pushd ..\

REM NUnit
REM OpenCover
echo OpenCover
set command=..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"..\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" -targetargs:"%fileList% --result=UnitTestResult_%project%.xml;format=nunit2 --process=Separate" -output:"UnitTestCoverage_%project%.xml" -filter:"+[*]* -[Fanex.Athena.UnitTest.Common.Helpers]* -[Fanex.Athena.BetList.Model]* -[Fanex.Result.Core.*]* -[Fanex.Athena.Resources]* -[AutoFixture]* -[CasinoLibrary]* -[nunit.framework]*"
call %command%

if %errorlevel% neq 0 goto error

REM REM ReportGenerator
echo ReportGenerator
set command=..\packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe -reports:"UnitTestCoverage_%project%.xml" -targetdir:"cireport\testcoveragereport\"
call %command%
if %errorlevel% neq 0 goto error


REM REM CoberturaReport
echo CoberturaReport
set command=..\packages\OpenCoverToCoberturaConverter.0.3.2\tools\OpenCoverToCoberturaConverter.exe -input:"UnitTestCoverage_%project%.xml" -output:"cireport\coberturareport\UnitTestCobertura.xml" -sources:".\"
call %command%
if %errorlevel% neq 0 goto error



echo Success.
goto end

:missing_project_name
echo Please provide name of your test project.
goto end

:error
echo Failure.
pause

:end
popd
