#pragma once

class ServiceEventContext;

class ServiceTask
{
public:
  ServiceTask(const ServiceEventContext* context);
  virtual ~ServiceTask();

public:
  ///async notification to start service. 
  ///it's expected to move service to 
  ///running state as it initialization 
  ///is finished
  virtual void CallStartService() = 0;

  ///async notification
  ///it may be called while StartService command 
  ///is still running.
  ///must report service status is stopped at end.
  virtual void CallStopSerive() = 0;

protected:
  const ServiceEventContext* const myContext;

};

