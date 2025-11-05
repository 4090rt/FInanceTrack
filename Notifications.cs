using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WinFormsApp4
{
    internal class Notifications
    {
    }

    public interface Notificationss
    {
        public void Not();
    }


    public class Notral1:Notificationss
    {
        
        public void Not()
        {
            var notifyIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text ="FinanceTrack"
            };

            notifyIcon.ShowBalloonTip(4000, "Успех!", "Вы успешно зарегистрировались", ToolTipIcon.Info);

            SetupAutoDispose(notifyIcon, 5000);
        }

        private void SetupAutoDispose(NotifyIcon notifyIcon, int delayMs)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delayMs;
            timer.Elapsed += (s, e) =>
            {
                notifyIcon.Dispose();  
                timer.Dispose();       
            };
            timer.Start();
        }
    }

    public class Notral2 : Notificationss
    {
        public void Not()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "FinanceTrack"
            };

            notification.ShowBalloonTip(5000,"Авторизация", "Вы успешно авторизовались!", ToolTipIcon.Info);

            SetupAutoDispose(notification, 5000);
        }

        private void SetupAutoDispose(NotifyIcon notificaton, int delayMs)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delayMs;
            timer.Elapsed += (e, s) =>
            {
                notificaton.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }

    public class Notral3 : Notificationss
    {
        public void Not()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "FinanceTrack"
            };

            notification.ShowBalloonTip(5000, "Смена пароля", "Вы успешно сменили пароль!", ToolTipIcon.Info);

            SetupAutoDispose(notification, 5000);
        }

        private void SetupAutoDispose(NotifyIcon notificaton, int delayMs)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delayMs;
            timer.Elapsed += (e, s) =>
            {
                notificaton.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }

    public class Notral4 : Notificationss
    {
        public void Not()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "FinanceTrack"
            };

            notification.ShowBalloonTip(5000, "Смена валюты", "Вы успешно сменили валюту!", ToolTipIcon.Info);

            SetupAutoDispose(notification, 5000);
        }

        private void SetupAutoDispose(NotifyIcon notificaton, int delayMs)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delayMs;
            timer.Elapsed += (e, s) =>
            {
                notificaton.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}

