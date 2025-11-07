using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinFormsApp4.fabric;

namespace WinFormsApp4
{
    public interface NotFab
    {
        public void Notif();
    }
    //enum с вариантами
    public enum Exportnotification
    {
        ExportExcel,
        ExportWord,
        ExportPDF,
        ExportHTML
    }
    // уведомление для excel
    public class ExporttoExcel : NotFab
    {
        // конструктор
        private readonly string _outPath;
        public ExporttoExcel(string outpath)
        {
            _outPath = outpath;
        }
        // метод создания и  вызова уведомления
        public void Notif()
        {
            var notification = new NotifyIcon();
            {
                notification.Icon = SystemIcons.Application;
                notification.Visible = true;
                notification.Text = "FinazzznceTrack";
            }
            notification.ShowBalloonTip(4000, "Экспорт успешно завершен!", $"Экспорт завершен, можете возвращаться к вашей статистике в удобном для вас формате! Excel по пути {_outPath}", ToolTipIcon.Info);
            Timerdispose(notification, 5000);
        }
        // таймер с автоочисткой
        public void Timerdispose(NotifyIcon notifyIcon, int delay)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delay;
            timer.Elapsed += (s, e) =>
            {
                notifyIcon.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }
    // уведомление для Word
    public class ExporttoWord : NotFab
    {
        // конструктор
        private readonly string _outPath;

        public ExporttoWord(string outpath)
        { 
            _outPath= outpath;
        }
        // метод создания и и вызова уведомления
        public void Notif()
        {
            var notification = new NotifyIcon();
            {
                notification.Icon = SystemIcons.Application;
                notification.Visible = true;
                notification.Text = "FinanceTrack";
            }

            notification.ShowBalloonTip(4000, "Экспорт успешно завершен!", $"Экспорт завершен, можете возвращаться к вашей статистике в удобном для вас формате Word! по пути {_outPath}", ToolTipIcon.Info);
            Timerdispose(notification, 5000);
        }
        // таймер с автоочисткой
        public void Timerdispose(NotifyIcon notifyIcon, int delay)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delay;
            timer.Elapsed += (s, e) =>
            {
                notifyIcon.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }
    // уведомление для Pdf
    public class ExporttoPdf : NotFab
    {
        public readonly string _outPath;
        // конструктор
        public ExporttoPdf(string outpath)
        { 
            _outPath = outpath;
        }
        // метод создания и и вызова уведомления
        public void Notif()
        {
            var notidicationn = new NotifyIcon();
            {
                notidicationn.Icon = SystemIcons.Application;
                notidicationn.Visible = true;
                notidicationn.Text = "FinanceTrack";
            }

            notidicationn.ShowBalloonTip(4000, "Экспорт успешно завершен!", $"Экспорт завершен, можете возвращаться к вашей статистике в удобном для вас формате Word! по пути {_outPath}", ToolTipIcon.Info);
            Timerdispose(notidicationn, 5000);
        }
        // таймер с автоочисткой
        public void Timerdispose(NotifyIcon notifyIcon, int delay)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delay;
            timer.Elapsed += (s, e) =>
            {
                notifyIcon.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }
    // уведомление для Html
    public class ExporttoHTML : NotFab
    {
        public readonly string _outPath;
        // конструктор
        public ExporttoHTML(string outpath)
        { 
            _outPath= outpath;
        }
        // метод создания и и вызова уведомления
        public void Notif()
        {
            var notification = new NotifyIcon();
            { 
                notification.Icon = SystemIcons.Application;
                notification.Visible = true;
                notification.Text="FinanceTrack";
            }

            notification.ShowBalloonTip(4000, "Экспорт успешно завершен!", $"Экспорт завершен, можете возвращаться к вашей статистике в удобном для вас формате Word! по пути {_outPath}", ToolTipIcon.Info);
            Timerdispose(notification,5000);
        }
        // таймер с автоочисткой
        public void Timerdispose(NotifyIcon notifyIcon, int delay)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delay;
            timer.Elapsed += (s, e) =>
            {
                notifyIcon.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }

    public class notificationexport
    {
        public NotFab Fabricexports(Exportnotification exportformat, string outpath)
        {
            return exportformat switch
            {
                Exportnotification.ExportExcel => new ExporttoExcel(outpath),
                Exportnotification.ExportWord => new ExporttoWord(outpath),
                Exportnotification.ExportPDF => new ExporttoPdf(outpath),
                Exportnotification.ExportHTML => new ExporttoHTML(outpath)
            };
        }
    }
}
