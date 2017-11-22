using System;
using System.Threading;

class Alarm
{
	public delegate void AlarmHandler(DateTime at);

	DateTime at_;
	AlarmHandler hnd_;
	System.Timers.Timer tmr_;
	EventWaitHandle wakeup_;

	public Alarm(DateTime at, AlarmHandler hnd)
	{
		at_ = at;
		hnd_ = hnd;
		tmr_ = new System.Timers.Timer();
		wakeup_ = new EventWaitHandle(false, EventResetMode.ManualReset);
	}
	public void on()
	{
		tmr_.Elapsed += new System.Timers.ElapsedEventHandler(timerHandler);
		tmr_.Interval = 1000;
		tmr_.Enabled = true;
		tmr_.Start();
		wakeup_.WaitOne();
	}
	public void off()
	{
		tmr_.Stop();
		wakeup_.Set();
	}
	void timerHandler(object sender, System.Timers.ElapsedEventArgs e)
	{
		if (DateTime.Now >= at_) {
			hnd_(at_);
			off();
		}
	}
}

class Program
{
	static void Main(string[] args)
	{
		Alarm.AlarmHandler hnd = new Alarm.AlarmHandler(alarmHandler);
		DateTime at = DateTime.Now;
		at.AddSeconds(10);
		Alarm alarm = new Alarm(at, hnd);
		alarm.on();
	}
	static void alarmHandler(DateTime at)
	{
		System.Console.WriteLine("Time is up!");
	}
}
