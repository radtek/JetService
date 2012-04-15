#include "StdAfx.h"
#include "ProcessInterruptConsoleControlHandler.h"
#include "Logger.h"

const Logger LOG(L"ProcessInterruptConsoleControlHandler");


ProcessInterruptConsoleControlHandler::ProcessInterruptConsoleControlHandler(const ServiceTaskSettings* settings)
  : ProcessInterruptTimeoutHandler(settings)
{
}


ProcessInterruptConsoleControlHandler::~ProcessInterruptConsoleControlHandler(void)
{
}


bool ProcessInterruptConsoleControlHandler::ExecuteInterruptAction(PROCESS_INFORMATION& info) {
  LOG.LogInfo(L"Sending CRTL+C to the service process");    
  if (0 == GenerateConsoleCtrlEvent(CTRL_C_EVENT, info.dwProcessId)) {
    LOG.LogWarnFormat(L"Failed to stop process with CTRL+C. %s", LOG.GetLastError());
    return false;
  }
  return true;
}

