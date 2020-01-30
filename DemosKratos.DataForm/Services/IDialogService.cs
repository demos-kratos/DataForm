using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DemosKratos.Services
{
    public interface IDialogService
    {
        Task Alert(DialogAlertInfo dialogAlertInfo, CancellationToken? t = null);
        Task<string> Entry(DialogEntryInfo dialogEntryInfo, CancellationToken? t = null);
        Task<string> Sheet(DialogSheetInfo dialogSheetInfo, CancellationToken? t = null);
        Task<bool> Question(DialogQuestionInfo dialogQuestionInfo, CancellationToken? t = null);
        Task<DateTime?> GetDate(string title = null, DateTime? selectedDate = null, CancellationToken? t = null);
        Task<TimeSpan?> GetTime(string title = null, TimeSpan? selectedTime = null, CancellationToken? t = null);
        void Toast(DialogToastInfo dialogToastInfo);
        void Toast(string text);
        void ShowLoading(string message);
        void HideLoading();
    }

    public class DialogAlertInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Cancel { get; set; }
    }

    public class DialogEntryInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Placeholder { get; set; }
        public string Cancel { get; set; }
        public string Ok { get; set; }
        public string Text { get; set; }
    }

    public class DialogSheetInfo
    {
        public string Title { get; set; }
        public string Cancel { get; set; }
        public string Destruction { get; set; }
        public string[] Items { get; set; }
    }

    public class DialogQuestionInfo
    {
        public string Title { get; set; }
        public string Question { get; set; }
        public string Positive { get; set; }
        public string Negative { get; set; }
    }

    public class DialogToastInfo
    {
        public string Text { get; set; }
        public bool IsLongTime { get; set; } = true;
    }
}
